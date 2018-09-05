using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Team1ComboManager : MonoBehaviour {


    public int MaxCombo { get; private set; }
    private int comboCount;

    // Use this for initialization
    void Start() {
        Reset();
    }

    public void AddScore() {
        comboCount++;
        MaxCombo = Mathf.Max(MaxCombo, comboCount);
    }

    public void Reset() {
        comboCount = 0;
    }
}
