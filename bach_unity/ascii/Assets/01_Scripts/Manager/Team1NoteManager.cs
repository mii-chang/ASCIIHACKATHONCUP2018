using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;

public class Team1NoteManager : MonoBehaviour {
    public const int BPM = 170;
    public const float BeatTime = 60f / BPM / 4f;
    public const float DisplayTime = 1.5f;
    public const float StartTime = 0.485f;
    public const float MissTime = 0.2f;

    public const int FireWorkMaxType = 3;

    [SerializeField] private Team1ComboManager combo;
    [SerializeField] private SoundManager sound;
    [SerializeField] private TextAsset data;
    [SerializeField] private Team1Note noteBase;
    [SerializeField] private GameObject[] fireWorkObj;
    [SerializeField] private OSCController oscController;
    [SerializeField] private WebCam webCam;



    private Queue<Team1NoteData> noteDatas = new Queue<Team1NoteData>();
    private List<Team1Note> notes = new List<Team1Note>();

    public int PerfectCount { get; private set; }
    public int MissCount { get; private set; }

    void Start() {
        float time = StartTime;
        foreach (var s in data.text.Split(',')) {
            foreach (var c in s) {
                var pos = c - '0';
                if (pos >= 0 && pos < FireWorkMaxType) {
                    noteDatas.Enqueue(new Team1NoteData(pos, time));
                }
            }
            time += BeatTime;
        }

        oscController.onDeviceDataObservable
                     .Where(x => x.teamNum == 1)
                     .Subscribe(x =>
                     {
                         for (int i = 0; i < FireWorkMaxType; i++) {
                             if (i == 0 && !x.isJump) continue;
                             if (i == 2 && !x.isLoudVoice) continue;
                             if (i == 1 && !(x.isJump && x.isLoudVoice)) continue;

                             var note = notes.FirstOrDefault(n => n.Data.Type == i);
                             if (note == null) {
                                 continue;
                             }

                             if (Mathf.Abs(note.Data.Time - sound.Time) < MissTime) {
                                 Evaluate(note, true);
                             }
                         }
                     });
    }

    void Update() {
        if (noteDatas.Count != 0 && noteDatas.Peek().Time - DisplayTime < sound.Time) {
            var note = Instantiate(noteBase);
            var data = noteDatas.Dequeue();
            note.transform.SetParent(transform);
            note.SetData(data);
            notes.Add(note);
        }
    }

    public void Evaluate(Team1Note note, bool isPerfect) {
        if (isPerfect) {
            sound.PlaySE();
            note.Fired(fireWorkObj[note.Data.Type]);
            PerfectCount++;
            combo.AddScore();
            webCam.SaveImage();
        } else {
            combo.Reset();
            MissCount++;
        }

        notes.Remove(note);
        Destroy(note.gameObject);
    }
}

public class Team1NoteData {
    public int Type { get; private set; }
    public float Time { get; private set; }

    public Team1NoteData(int pos, float time) {
        Type = pos;
        Time = time;
    }
}