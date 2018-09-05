using UnityEngine;
using System.Collections;

public class Team1Note : MonoBehaviour {
    private float StartPositionY = 0;

    [SerializeField] private Team1NoteManager manager;
    [SerializeField] private SoundManager sound;
    [SerializeField] private Transform[] muzzlePositions;
    [SerializeField] private GameObject lineObj;
    [SerializeField] private GameObject puffObj;
    private GameObject line;
    private GameObject puff;

    public Team1NoteData Data { get; private set; }

    public void SetData(Team1NoteData data) {
        Data = data;
        gameObject.SetActive(true);
    }

    private void Start() {
        transform.position = muzzlePositions[Data.Type].position;
        GetComponent<ParticleSystem>().Play();
        line = Instantiate(lineObj);
        line.transform.SetParent(transform);
    }
    void Update() {
        var t = (Data.Time - sound.Time);
        var rate = t / Team1NoteManager.DisplayTime;


        line.transform.localPosition = new Vector3(
            0,
            0,
            Mathf.Lerp(76, 0, rate)
        );

        if (t < -Team1NoteManager.MissTime) {
            Destroy(line);
            if (Random.value > 0.6f)
                manager.Evaluate(this, false);
            else
                manager.Evaluate(this, true);
        }
    }

    public void Fired(GameObject fireWorkObj) {
        var obj = Instantiate(fireWorkObj, transform.position + Vector3.up * 76, Quaternion.identity) as GameObject;
        obj.GetComponent<ParticleSystem>().Play();
    }

    public void Falling() {
        puff = Instantiate(puffObj);
        puff.transform.position = muzzlePositions[Data.Type].position + Vector3.up * 76;
        puff.SetActive(true);
        puff.GetComponent<ParticleSystem>().Play();
    }
}
