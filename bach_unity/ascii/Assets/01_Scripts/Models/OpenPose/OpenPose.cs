using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class OpenPose
{
    public string version;
    public List<People> people;
}

[Serializable]
public class People
{
    public List<float> pose_keypoints;
    public List<float> face_keypoints;
    public List<float> hand_left_keypoints;
    public List<float> hand_right_keypoints;
}
