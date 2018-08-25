using UnityEngine;
using System.Collections;

public class Team2Note : MonoBehaviour {
    private float StartPositionY = -657f;

    [SerializeField] private Team2NoteManager manager;
    [SerializeField] private SoundManager sound;
    [SerializeField] private Transform[] muzzlePositions;

    public Team2NoteData Data { get; private set; }

    public void SetData(Team2NoteData data) {
        Data = data;
        gameObject.SetActive(true);
    }

    void Update() {
        var t = (Data.Time - sound.Time);
        var rate = t / Team2NoteManager.DisplayTime;
        var targetPosition = muzzlePositions[Data.Type].position;
        transform.localPosition = new Vector3(
            targetPosition.x,
            Mathf.Lerp(targetPosition.y, StartPositionY, rate),
            0
        );

        if (t < -Team2NoteManager.MissTime) {
            manager.Evaluate(this, false);
        }
    }
}
