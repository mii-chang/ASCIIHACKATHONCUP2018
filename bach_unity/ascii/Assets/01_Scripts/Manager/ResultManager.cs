using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ResultManager : MonoBehaviour {

    [SerializeField] private Text user1Text;
    [SerializeField] private Text user2Text;

    private Dictionary<int, ScoreData> scoreDataDic = new Dictionary<int, ScoreData>();

    public void SetData(ScoreData team1ScoreData, ScoreData team2ScoreData) {
        scoreDataDic.Add(1, team1ScoreData);
        scoreDataDic.Add(2, team2ScoreData);
    }
}
