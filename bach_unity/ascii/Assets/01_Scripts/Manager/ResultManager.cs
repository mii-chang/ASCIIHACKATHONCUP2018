using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ResultManager : SingletonMonoBehaviour<ResultManager> {

    [SerializeField] private List<UserScoreObj> userScoreObj;
    private Dictionary<int, ScoreData> scoreDataDic = new Dictionary<int, ScoreData>();

    public void SetData(ScoreData team1ScoreData, ScoreData team2ScoreData) {
        scoreDataDic.Add(1, team1ScoreData);
        scoreDataDic.Add(2, team2ScoreData);
    }

    public void Show() {
        foreach (var item in userScoreObj) {
            item.userScores.comboText.text =
                    "MaxCombo: " + scoreDataDic[item.team.ToInt()].maxCombo;
            item.userScores.perfectText.text =
                    "Perfect: " + scoreDataDic[item.team.ToInt()].perfectCount;
            item.userScores.missText.text =
                    "Miss: " + scoreDataDic[item.team.ToInt()].missCount;
            item.userScores.scoreText.text =
                    "Score: " + scoreDataDic[item.team.ToInt()].score;
        }
    }
}

[System.Serializable]
public class UserScores {
    public Text comboText;
    public Text perfectText;
    public Text missText;
    public Text scoreText;
}

[System.Serializable]
public class UserScoreObj {
    public Const.Team team;
    public UserScores userScores;
}