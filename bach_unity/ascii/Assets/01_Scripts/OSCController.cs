using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityOSC;
using UniRx;

public class OscController : MonoBehaviour {
    #region Network Settings
    public int InComingPort;
    #endregion
    private Dictionary<string, ServerLog> servers;
    private DeviceData team1DeviceData;
    private DeviceData team2DeviceData;


    public IObservable<DeviceData> onDeviceDataObservable {
        get { return deviceDataSubject.AsObservable(); }
    }

    public Subject<DeviceData> deviceDataSubject = new Subject<DeviceData>();

    void Start() {
        team1DeviceData = new DeviceData(Const.Team.team1);
        team2DeviceData = new DeviceData(Const.Team.team2);
        OSCHandler.Instance.Init(InComingPort);
        servers = new Dictionary<string, ServerLog>();
    }

    void Update() {
        OSCHandler.Instance.UpdateLogs();

        servers = OSCHandler.Instance.Servers;
        foreach (KeyValuePair<string, ServerLog> item in servers) {
            if (item.Value.log.Count > 0) {
                int lastPacketIndex = item.Value.packets.Count - 1;
                var add = item.Value.packets[lastPacketIndex].Address;

                if (add == "#bundle") {
                    OSCPacket oscPacket = item.Value.packets[lastPacketIndex].Data[0] as OSCPacket;
                    team1DeviceData.isJump = oscPacket.Data[0].ToString() == "0" ? false : true;
                    team2DeviceData.isJump = oscPacket.Data[1].ToString() == "0" ? false : true;
                } else {
                    IEnumerable receiveEnumerable = item.Value.packets[lastPacketIndex].Data as IEnumerable;
                    int elementCount = 0;
                    int team = 1;

                    foreach (object element in receiveEnumerable) {

                        if (elementCount == 0) {
                            team = (int)element;
                        }
                        if (elementCount == 1) {
                            if (team == 1) {
                                team1DeviceData.isLoudVoice = (int)element == 0 ? false : true;
                                Debug.Log("aaaa");
                            }
                            if (team == 2) team2DeviceData.isLoudVoice = (int)element == 0 ? false : true;
                        }
                        elementCount++;
                    }
                }
            }
            deviceDataSubject.OnNext(team1DeviceData);
            deviceDataSubject.OnNext(team2DeviceData);
        }
    }
}

public class DeviceData {
    public Const.Team team;
    public bool isLoudVoice;
    public bool isJump;
    public DeviceData(Const.Team team) {
        this.team = team;
    }
}

