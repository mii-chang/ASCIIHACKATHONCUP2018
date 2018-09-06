using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class ScoreManager : SingletonMonoBehaviour<ScoreManager> {

    public Dictionary<Const.Team, ScoreData> scoreDataDic = new Dictionary<Const.Team, ScoreData>();
    private NoteManager noteManager;

    private void Start() {
        scoreDataDic.Add(Const.Team.team1, new ScoreData());
        scoreDataDic.Add(Const.Team.team2, new ScoreData());
        noteManager = NoteManager.Instance;
        PerfectDeal();
        MissDeal();
    }

    private void PerfectDeal() {
        noteManager.onDecesionResultObservable
                   .Where(result => result.result == Const.DecisionResult.Perfect)
                   .Subscribe(result =>
                   {
                       var scoreData = scoreDataDic[result.team];
                       scoreData.perfectCount++;
                       scoreData.comboCount++;
                       scoreData.maxCombo = Mathf.Max(scoreData.maxCombo, scoreData.comboCount);
                   });
    }

    private void MissDeal() {
        noteManager.onDecesionResultObservable
                   .Where(result => result.result == Const.DecisionResult.Miss)
                   .Subscribe(result =>
                   {
                       var scoreData = scoreDataDic[result.team];
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