﻿using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public class SoundManager : SingletonMonoBehaviour<SoundManager> {

    [SerializeField] private AudioSource bgm;
    [SerializeField] private AudioClip se;
    [SerializeField] private DataDrawer dataDrawer;
    public float Time { get { return bgm.time; } }

    private NoteManager noteManager;
    private ScoreManager scoreManager;
    private bool isLoading;

    private void Start() {
        bgm.time = 50f;
        scoreManager = ScoreManager.Instance;
        noteManager = NoteManager.Instance;

        this.UpdateAsObservable()
            .Where(_ => !bgm.isPlaying && !isLoading)
            .Subscribe(_ => StartCoroutine(loadResult()));
    }

    private IEnumerator loadResult() {
        isLoading = true;
        var loader = SceneManager.LoadSceneAsync("Result", LoadSceneMode.Additive);
        yield return loader;

        ResultManager.Instance.SetData(
            scoreManager.scoreDataDic[Const.Team.team1],
            scoreManager.scoreDataDic[Const.Team.team2]
        );

        ResultManager.Instance.Show();
        SceneManager.UnloadSceneAsync("Sub");
    }

    public void PlaySE() {
        bgm.PlayOneShot(se);
    }
}
