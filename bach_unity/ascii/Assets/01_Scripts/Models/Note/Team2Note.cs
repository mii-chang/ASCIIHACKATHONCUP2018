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

    private void Start() {
        transform.position = muzzlePositions[Data.Type].position;
        GetComponent<ParticleSystem>().Play();
    }

    void Update() {
        var t = (Data.Time - sound.Time);

        if (t < -Team2NoteManager.MissTime) {
            manager.Evaluate(this, false);
        }
    }

    public void Fired(GameObject fireWorkObj) {
        var obj = Instantiate(fireWorkObj, transform.position + Vector3.up * 76, Quaternion.identity) as GameObject;
        obj.GetComponent<ParticleSystem>().Play();
    }
}
