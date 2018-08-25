using UnityEngine;
using System.Collections;

public class Team1Note : MonoBehaviour {
    private float StartPositionY = 0;

    [SerializeField] private Team1NoteManager manager;
    [SerializeField] private SoundManager sound;
    [SerializeField] private Transform[] muzzlePositions;
    [SerializeField] private GameObject fireWorkObj;

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
        var rate = t / Team1NoteManager.DisplayTime;
        //var targetPosition = muzzlePositions[Data.Type].position;
        //transform.localPosition = new Vector3(
        //    targetPosition.x,
        //    Mathf.Lerp(targetPosition.y, StartPositionY, rate),
        //    0
        //);
        if (!isCreated && rate < 0.1f) {
            isCreated = true;
            Fired();
        }

        if (t < -Team1NoteManager.MissTime) {
            manager.Evaluate(this, false);
        }
    }

    public void Fired() {
        var obj = Instantiate(fireWorkObj, transform.position + Vector3.up * 76, Quaternion.identity) as GameObject;
        obj.GetComponent<ParticleSystem>().Play();
    }
}
