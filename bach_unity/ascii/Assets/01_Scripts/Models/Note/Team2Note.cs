using UnityEngine;
using System.Collections;

public class Team2Note : MonoBehaviour {

    [SerializeField] private Team2NoteManager manager;
    [SerializeField] private SoundManager sound;
    [SerializeField] private Transform[] muzzlePositions;
    [SerializeField] private GameObject lineObj;
    private GameObject line;


    public Team2NoteData Data { get; private set; }

    public void SetData(Team2NoteData data) {
        Data = data;
        gameObject.SetActive(true);
    }

    private void Start() {
        transform.position = muzzlePositions[Data.Type].position;
        GetComponent<ParticleSystem>().Play();
        line = Instantiate(lineObj);
        line.transform.SetParent(manager.transform);
    }

    void Update() {
        var t = (Data.Time - sound.Time);
        var rate = t / Team2NoteManager.DisplayTime;

        line.transform.localPosition = new Vector3(
            muzzlePositions[Data.Type].position.x,
            Mathf.Lerp(muzzlePositions[Data.Type].position.y, 5, rate),
            0
        );

        if (t < -Team2NoteManager.MissTime) {
            manager.Evaluate(this, false);
        }
    }

    public void Fired(GameObject fireWorkObj) {
        var obj = Instantiate(fireWorkObj, transform.position + Vector3.up * 76, Quaternion.identity) as GameObject;
        obj.GetComponent<ParticleSystem>().Play();
    }
}
