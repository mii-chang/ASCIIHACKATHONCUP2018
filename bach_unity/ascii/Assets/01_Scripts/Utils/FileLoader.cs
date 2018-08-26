using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

public class FileLoader : MonoBehaviour {

    string tempMes = "[";

    public GameObject[] user1TextObjs;
    public Text[] user1Text;
    public GameObject[] user2TextObjs;
    public Text[] user2Text;
    static float x;
    static float y;
    List<Vector2> player1 = new List<Vector2>();
    List<Vector2> player2 = new List<Vector2>();

    private void Awake() {
        //user1TextObjs = GameObject.FindGameObjectsWithTag("user1_texts");
        //user1Text = user1TextObjs.Select(obj => obj.GetComponent<Text>()).Reverse().ToArray();

        //user2TextObjs = GameObject.FindGameObjectsWithTag("user2_texts");
        //user2Text = user2TextObjs.Select(obj => obj.GetComponent<Text>()).Reverse().ToArray();
    }

    public float GetAngle(Vector2 basic_point, Vector2 point1, Vector2 point2){
        double direction1_x  = point1.x - basic_point.x;
        double direction1_y  = point1.y - basic_point.y;
        double direction2_x  = point2.x - basic_point.x;
        double direction2_y  = point2.y - basic_point.y;

        double cos = (direction1_x * direction2_x + direction1_y * direction2_y) / Math.Sqrt(Math.Pow(direction1_x, 2) + Math.Pow(direction1_y, 2)) / Math.Sqrt(Math.Pow(direction2_x, 2) + Math.Pow(direction2_y, 2));
        double radius = Math.Acos(cos) * 180 / Math.PI;
        return (float)radius;
    }


    public OpenPose ReadOpenPoseJson(string json) {
        return JsonUtility.FromJson<OpenPose>(json);
    }

    public void DrawOpenPoseData(OpenPose data)
    {
        int index = 0;
        if (data.people.Count == 0) return;
        data.people[0].pose_keypoints.ForEach(points =>
        {
            if (index % 3 == 0)
            {
                tempMes += points.ToString("F1") + ", ";
                x = points;
            }
            else if (index % 3 == 1)
            {
                tempMes += points.ToString("F1") + "]";
                y = points;
            }

            if (index % 3 == 2)
            {
                player1.Add(new Vector2(x, y));
                user1Text[index / 3].text = tempMes;
                tempMes = "[";
            }
            index++;
        });
        if (data.people.Count == 1) return;
        index = 0;

        data.people[2].pose_keypoints.ForEach(points =>
        {
            if (index % 3 == 0)
            {
                tempMes += points.ToString("F1") + ", ";
                x = points;
            }
            else if (index % 3 == 1)
            {
                tempMes += points.ToString("F1") + "]";
                y = points;
            }

            if (index % 3 == 2)
            {
                player2.Add(new Vector2(x, y));
                user2Text[index / 3].text = tempMes;
                tempMes = "[";
            }
            index++;
        });

        float center_x = (player1[11][0] - player1[8][0]) / 2 + player1[8][0];
        float center_y = (player1[11][1] - player1[8][1]) / 2 + player1[8][1];
        Vector2 center = new Vector2(
            center_x, center_y
        );

        float leg_radius = GetAngle(center, player1[10], player1[13]);
        float leg_point  = leg_radius * 100 / 180;

        float arm_radius = GetAngle(player1[1], player1[4], player1[7]);
        float arm_point  = 100 - Math.Abs(120 - arm_radius);

        print(leg_point);
        print(arm_point);
    }
}
