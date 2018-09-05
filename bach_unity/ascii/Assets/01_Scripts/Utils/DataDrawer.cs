using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataDrawer : MonoBehaviour {

    FileLoader fileLoader;
    int index = 0;
    int counter;

    public ResultPoint resultPoint;


    private void Awake() {
        fileLoader = GetComponent<FileLoader>();
    }
    private void Start() {
        resultPoint = new ResultPoint();
    }

    void Update() {
        counter++;
        if (counter < 10) return;
        counter = 0;
        string fileName = index.ToString("00000000") + "_keypoints";
        if (System.IO.File.Exists(Application.dataPath + "/../outputs/" + fileName + ".json") != false) {
            var textAsset = NonResources.Load<TextAsset>(Application.dataPath + "/../outputs/" + fileName);
            OpenPose data = fileLoader.ReadOpenPoseJson(textAsset.text);
            var result = fileLoader.DrawOpenPoseData(data);
            resultPoint.team1Score += result.team1Score;
            resultPoint.team2Score += result.team2Score;
            index++;
        }
    }
}

public class ResultPoint {
    public int team1Score;
    public int team2Score;
}