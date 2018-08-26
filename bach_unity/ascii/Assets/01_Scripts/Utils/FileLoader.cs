using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class FileLoader : MonoBehaviour {

    string tempMes = "[";

    //public GameObject[] user1TextObjs;
    //public Text[] user1Text;
    //public GameObject[] user2TextObjs;
    //public Text[] user2Text;

    List<Vector2> player1 = new List<Vector2>();
    List<Vector2> player2 = new List<Vector2>();

    public float x;
    public float y;

    //private void Awake() {
    //    user1TextObjs = GameObject.FindGameObjectsWithTag("user1_texts");
    //    user1Text = user1TextObjs.Select(obj => obj.GetComponent<Text>()).Reverse().ToArray();

    //    user2TextObjs = GameObject.FindGameObjectsWithTag("user2_texts");
    //    user2Text = user2TextObjs.Select(obj => obj.GetComponent<Text>()).Reverse().ToArray();
    //}


    public OpenPose ReadOpenPoseJson(string json) {
        return JsonUtility.FromJson<OpenPose>(json);
    }

    public float GetAngle(Vector2 basic_point, Vector2 point1, Vector2 point2) {
        float direction1_x = point1.x - basic_point.x;
        float direction1_y = point1.y - basic_point.y;
        float direction2_x = point2.x - basic_point.x;
        float direction2_y = point2.y - basic_point.y;

        float cos = (direction1_x * direction2_x + direction1_y * direction2_y) / Mathf.Sqrt(Mathf.Pow(direction1_x, 2) + Mathf.Pow(direction1_y, 2)) / Mathf.Sqrt(Mathf.Pow(direction2_x, 2) + Mathf.Pow(direction2_y, 2));
        float radius = Mathf.Acos(cos) * 180 / Mathf.PI;
        return (float)radius;
    }


    public ResultPoint DrawOpenPoseData(OpenPose data) {
        int index = 0;
        if (data.people.Count != 2) return null;
        data.people[0].pose_keypoints.ForEach(points =>
        {
            if (index % 3 == 0) {
                tempMes += points.ToString("F1") + ", ";
                x = points;
            } else if (index % 3 == 1) {
                tempMes += points.ToString("F1") + "]";
                y = points;
            }

            if (index % 3 == 2) {
                player1.Add(new Vector2(x, y));
                tempMes = "[";
            }
            index++;
        });
        data.people[1].pose_keypoints.ForEach(points =>
        {
            if (index % 3 == 0) {
                tempMes += points.ToString("F1") + ", ";
                x = points;
            } else if (index % 3 == 1) {
                tempMes += points.ToString("F1") + "]";
                y = points;
            }

            if (index % 3 == 2) {
                player2.Add(new Vector2(x, y));
                tempMes = "[";
            }
            index++;
        });

        float center_x = (player1[11][0] - player1[8][0]) / 2 + player1[8][0];
        float center_y = (player1[11][1] - player1[8][1]) / 2 + player1[8][1];
        Vector2 center = new Vector2(
            center_x, center_y
        );

        float player1_leg_radius = GetAngle(center, player1[10], player1[13]);
        float player1_leg_point = player1_leg_radius * 100 / 180;
        float player1_arm_radius = GetAngle(player1[1], player1[4], player1[7]);
        float player1_arm_point = 100 - Mathf.Abs(120 - player1_arm_radius);

        float player2_leg_radius = GetAngle(center, player2[10], player2[13]);
        float player2_leg_point = player2_leg_radius * 100 / 180;
        float player2_arm_radius = GetAngle(player2[1], player2[4], player2[7]);
        float player2_arm_point = 100 - Mathf.Abs(120 - player2_arm_radius);

        print(player1_leg_point);
        print(player1_arm_point);

        var resultPoint = new ResultPoint();

        resultPoint.team1Score = Mathf.FloorToInt(player1_arm_point) + Mathf.FloorToInt(player1_leg_point);
        resultPoint.team2Score = Mathf.FloorToInt(player2_arm_point) + Mathf.FloorToInt(player2_leg_point);

        return resultPoint;
    }

}
