using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class DebugInput : MonoBehaviour {

    [SerializeField] private OscController oscController;
    DeviceData deviceData1;

    void Start() {
        deviceData1 = new DeviceData(1);
        this.UpdateAsObservable()
            .Subscribe(x =>
            {
                if (Input.GetKeyDown(KeyCode.A)) {
                    deviceData1.isJump = true;
                    deviceData1.isLoudVoice = false;
                    oscController.deviceDataSubject.OnNext(deviceData1);

                }
                if (Input.GetKeyDown(KeyCode.S)) {
                    deviceData1.isJump = true;
                    deviceData1.isLoudVoice = true;
                    oscController.deviceDataSubject.OnNext(deviceData1);

                }
                if (Input.GetKeyDown(KeyCode.D)) {
                    deviceData1.isJump = false;
                    deviceData1.isLoudVoice = true;
                    oscController.deviceDataSubject.OnNext(deviceData1);

                }
            });
    }
}
