using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SoundManager : MonoBehaviour {
    [SerializeField] private Team1NoteManager team1NoteManager;
    [SerializeField] private Team2NoteManager team2NoteManager;
    [SerializeField] private Team1ComboManager team1ComboManager;
    [SerializeField] private Team2ComboManager team2ComboManager;
    [SerializeField] private AudioSource bgm;
    [SerializeField] private AudioClip se;

    public float Time { get { return bgm.time; } }

    private bool isLoading;

    void Update() {
        if (!bgm.isPlaying && !isLoading) {
            StartCoroutine(loadResult());
        }
    }

    private IEnumerator loadResult() {
        isLoading = true;
        var loader = SceneManager.LoadSceneAsync("Result", LoadSceneMode.Additive);
        yield return loader;

        FindObjectOfType<ResultManager>().SetData(
            team1ComboManager.MaxCombo,
            team1NoteManager.PerfectCount,
            team1NoteManager.MissCount,
            team2ComboManager.MaxCombo,
            team2NoteManager.PerfectCount,
            team2NoteManager.MissCount
        );
        SceneManager.UnloadScene("Live");
    }

    public void PlaySE() {
        bgm.PlayOneShot(se);
    }
}
