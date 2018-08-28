using UnityEngine;
using System.Collections;

public class Team1Note : MonoBehaviour {
    private float StartPositionY = 0;

    [SerializeField] private Team1NoteManager manager;
    [SerializeField] private SoundManager sound;
    [SerializeField] private Transform[] muzzlePositions;
    [SerializeField] private GameObject lineObj;
    private GameObject line;

    private bool isCreated;

    public Team1NoteData Data { get; private set; }

    public void SetData(Team1NoteData data) {
        Data = data;
        gameObject.SetActive(true);
        line = Instantiate(lineObj);
        line.transform.SetParent(manager.transform);
    }

    private void Start() {
        transform.position = muzzlePositions[Data.Type].position;
        GetComponent<ParticleSystem>().Play();
    }
    void Update() {
        var t = (Data.Time - sound.Time);
        var rate = t / Team1NoteManager.DisplayTime;


        line.transform.localPosition = new Vector3(
            muzzlePositions[Data.Type].position.x,
            Mathf.Lerp(muzzlePositions[Data.Type].position.y, 76, rate),
            0
        );

        if (t < -Team1NoteManager.MissTime) {
            manager.Evaluate(this, false);
        }
    }

    public void Fired(GameObject fireWorkObj) {
        var obj = Instantiate(fireWorkObj, transform.position + Vector3.up * 76, Quaternion.identity) as GameObject;
        obj.GetComponent<ParticleSystem>().Play();
    }
}
