using UnityEngine;
using System.Collections;

public class Team1Note : MonoBehaviour {
    private float StartPositionY = 0;

    [SerializeField] private Team1NoteManager manager;
    [SerializeField] private SoundManager sound;
    [SerializeField] private Transform[] muzzlePositions;

    private bool isCreated;

    public Team1NoteData Data { get; private set; }

    public void SetData(Team1NoteData data) {
        Data = data;
        gameObject.SetActive(true);
    }

    private void Start() {
        transform.position = muzzlePositions[Data.Type].position;
        GetComponent<ParticleSystem>().Play();
    }
    void Update() {
        var t = (Data.Time - sound.Time);

        if (t < -Team1NoteManager.MissTime) {
            manager.Evaluate(this, true);
        }
    }

    public void Fired(GameObject fireWorkObj) {
        var obj = Instantiate(fireWorkObj, transform.position + Vector3.up * 76, Quaternion.identity) as GameObject;
        obj.GetComponent<ParticleSystem>().Play();
    }
}
