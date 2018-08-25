using UnityEngine;
using System.Collections;

public class ResultManager : MonoBehaviour {
    [SerializeField] private Team1Number team1MaxCombo;
    [SerializeField] private Team1Number team1PerfectCount;
    [SerializeField] private Team1Number team1MissCount;
    [SerializeField] private Team2Number team2MaxCombo;
    [SerializeField] private Team2Number team2PerfectCount;
    [SerializeField] private Team2Number team2MissCount;

    public void SetData(int team1maxCombo, int team1perfectCount, int team1missCount,
                        int team2maxCombo, int team2perfectCount, int team2missCount) {
        this.team1MaxCombo.SetNumber(team1maxCombo);
        this.team1PerfectCount.SetNumber(team1perfectCount);
        this.team1MissCount.SetNumber(team1missCount);
        this.team2MaxCombo.SetNumber(team2maxCombo);
        this.team2PerfectCount.SetNumber(team2perfectCount);
        this.team2MissCount.SetNumber(team2missCount);
    }
}
