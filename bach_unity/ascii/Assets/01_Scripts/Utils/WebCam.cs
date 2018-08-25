using UnityEngine;
using System.Collections;
using System.IO;

public class WebCam : MonoBehaviour {
    public int Width = 1280;
    public int Height = 720;
    public int FPS = 15;

    public Material material;

    WebCamTexture webcamTexture;

    void Start() {
        WebCamDevice[] devices = WebCamTexture.devices;

        // display all cameras
        for (var i = 0; i < devices.Length; i++) {
            // get camera name
            string camname = devices[i].name;
            print(i + ":" + camname);

            webcamTexture = new WebCamTexture(camname, Width, Height, FPS);
            print(webcamTexture);
            material.mainTexture = webcamTexture;
            webcamTexture.Play();
            break;
        }
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            if (webcamTexture != null) {
                SaveToPNGFile(webcamTexture.GetPixels(), Application.dataPath + "/../SavedScreen.png");
            }
        }
    }

    void SaveToPNGFile(Color[] texData, string filename) {
        Texture2D takenPhoto = new Texture2D(Width, Height, TextureFormat.ARGB32, false);

        takenPhoto.SetPixels(texData);
        takenPhoto.Apply();

        byte[] png = takenPhoto.EncodeToPNG();
        Destroy(takenPhoto);

        // For testing purposes, also write to a file in the project folder
        File.WriteAllBytes(filename, png);
    }
}