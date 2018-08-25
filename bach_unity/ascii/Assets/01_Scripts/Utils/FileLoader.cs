using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class FileLoader : MonoBehaviour {

    string tempMes = "[";

    public GameObject[] user1TextObjs;
    public Text[] user1Text;
    public GameObject[] user2TextObjs;
    public Text[] user2Text;

    private void Awake() {
        user1TextObjs = GameObject.FindGameObjectsWithTag("user1_texts");
        user1Text = user1TextObjs.Select(obj => obj.GetComponent<Text>()).Reverse().ToArray();

        user2TextObjs = GameObject.FindGameObjectsWithTag("user2_texts");
        user2Text = user2TextObjs.Select(obj => obj.GetComponent<Text>()).Reverse().ToArray();
    }


    public OpenPose ReadOpenPoseJson(string json) {
        return JsonUtility.FromJson<OpenPose>(json);
    }

    public void DrawOpenPoseData(OpenPose data) {
        int index = 0;
        if (data.people.Count == 0) return;
        data.people[0].pose_keypoints.ForEach(points =>
        {
            if (index % 3 == 0) {
                tempMes += points.ToString("F1") + ", ";
            } else if (index % 3 == 1) {
                tempMes += points.ToString("F1") + "]";
            }

            if (index % 3 == 2) {
                user1Text[index / 3].text = tempMes;
                tempMes = "[";
            }
            index++;
        });
        if (data.people.Count == 1) return;
        data.people[1].pose_keypoints.ForEach(points =>
        {
            if (index % 3 == 0) {
                tempMes += points.ToString("F1") + ", ";
            } else if (index % 3 == 1) {
                tempMes += points.ToString("F1") + "]";
            }

            if (index % 3 == 2) {
                user2Text[index / 3].text = tempMes;
                tempMes = "[";
            }
            index++;
        });
    }

}
