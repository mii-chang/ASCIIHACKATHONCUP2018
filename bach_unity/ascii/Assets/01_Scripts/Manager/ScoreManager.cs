using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class ScoreManager : MonoBehaviour {

    public Dictionary<int, ScoreData> scoreDataDic = new Dictionary<int, ScoreData>();
    [SerializeField] private NoteManager noteManager;

    private void Start() {
        scoreDataDic.Add(1, new ScoreData());
        scoreDataDic.Add(2, new ScoreData());
        PerfectDeal();
        MissDeal();
    }

    private void PerfectDeal() {
        noteManager.onDecesionResultObservable
                   .Where(result => result.result == Const.DecesionResult.Perfect)
                   .Subscribe(result =>
                   {
                       var scoreData = scoreDataDic[result.teamNum];
                       scoreData.perfectCount++;
                       scoreData.comboCount++;
                       scoreData.maxCombo = Mathf.Max(scoreData.maxCombo, scoreData.comboCount);
                   });
    }

    private void MissDeal() {
        noteManager.onDecesionResultObservable
                   .Where(result => result.result == Const.DecesionResult.Miss)
                   .Subscribe(result =>
                   {
                       var scoreData = scoreDataDic[result.teamNum];
                       scoreData.missCount++;
                       scoreData.comboCount = 0;
                   });
    }


}

public class ScoreData {
    public int comboCount;
    public int maxCombo;
    public int score;
    public int perfectCount;
    public int missCount;
}