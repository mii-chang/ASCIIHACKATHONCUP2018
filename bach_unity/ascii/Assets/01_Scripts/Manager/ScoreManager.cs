using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour {

    public ScoreData team1ScoreData { get; private set; }
    public ScoreData team2ScoreData { get; private set; }

    private void Start() {

    }
}

public class ScoreData {
    public int maxCombo;
    public int score;
    public int perfectCount;
    public int missCount;
}