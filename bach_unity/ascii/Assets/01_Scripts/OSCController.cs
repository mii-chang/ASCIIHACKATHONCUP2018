using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityOSC;
using UniRx;

public class OSCController : MonoBehaviour {
    #region Network Settings
    public int InComingPort;
    #endregion
    private Dictionary<string, ServerLog> servers;
    private DeviceData team1DeviceData;
    private DeviceData team2DeviceData;


    public IObservable<DeviceData> onDeviceDataObservable
    {
        get
        {
            return deviceDataSubject.AsObservable();
        }
    }

    private Subject<DeviceData> deviceDataSubject = new Subject<DeviceData>();

    void Start() {
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

                UnityEngine.Debug.Log(String.Format("SERVER: {0} ADDRESS: {1} VALUE 0: {2}",
                item.Key, // Server name
                item.Value.packets[lastPacketIndex].Address, // OSC address
                item.Value.packets[lastPacketIndex].Data[0])); //First data value

                if (add == "#bundle") {

                }

                print("address: " + item.Value.packets[lastPacketIndex].Address);

                OSCPacket oscPacket = item.Value.packets[lastPacketIndex].Data[0] as OSCPacket;

                print("packet" + oscPacket.Data[1]);

                IEnumerable receiveEnumerable = item.Value.packets[lastPacketIndex].Data as IEnumerable;
                print("count: " + item.Value.packets[lastPacketIndex].Data.Count);
                foreach (object element in receiveEnumerable) {
                    var hoge = (string)element;
                    print(hoge);

                }
            }
        }
    }
}

public class DeviceData {
    public int teamNum;
    public int voiceLevel;
    public bool isJump;
}

