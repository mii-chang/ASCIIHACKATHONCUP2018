using UnityEngine;
using System.Collections;

public class ResultManager : MonoBehaviour {
    [SerializeField] private int team1MaxCombo;
    [SerializeField] private int team1PerfectCount;
    [SerializeField] private int team1MissCount;
    [SerializeField] private int team2MaxCombo;
    [SerializeField] private int team2PerfectCount;
    [SerializeField] private int team2MissCount;
    [SerializeField] private int team1Score;
    [SerializeField] private int team2Score;

    public void SetData(int team1maxCombo, int team1perfectCount, int team1missCount, int team1Score,
                        int team2maxCombo, int team2perfectCount, int team2missCount, int team2Score) {
        this.team1MaxCombo = team1maxCombo;
        this.team1PerfectCount = team1perfectCount;
        this.team1MissCount = team1missCount;
        this.team2MaxCombo = team2maxCombo;
        this.team2PerfectCount = team2perfectCount;
        this.team2MissCount = team2missCount;
        this.team1Score = team1Score;
        this.team2Score = team2Score;
    }
}
