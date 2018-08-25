using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataDrawer : MonoBehaviour {

    FileLoader fileLoader;
    int index = 0;
    int counter;

    private void Awake() {
        fileLoader = GetComponent<FileLoader>();
    }

    void Update() {
        counter++;
        if (counter < 10) return;
        counter = 0;
        string fileName = index.ToString("000000000000") + "_keypoints";
        if (System.IO.File.Exists(Application.dataPath + "/Resources/outputs/" + fileName + ".json") != false) {
            var textAsset = Resources.Load("outputs/" + fileName) as TextAsset;
            OpenPose data = fileLoader.ReadOpenPoseJson(textAsset.text);
            fileLoader.DrawOpenPoseData(data);
            index++;
        }
    }
}
