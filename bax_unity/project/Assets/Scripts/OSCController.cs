using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityOSC;

public class OSCController : MonoBehaviour
{
    #region Network Settings
    public string TargetAddr;
    public int OutGoingPort;
    public int InComingPort;
    #endregion
    private Dictionary<string, ServerLog> servers;

    // Script initialization
    void Start()
    {
        OSCHandler.Instance.Init(TargetAddr, OutGoingPort, InComingPort);
        servers = new Dictionary<string, ServerLog>();
    }

    // NOTE: The received messages at each server are updated here
    // Hence, this update depends on your application architecture
    // How many frames per second or Update() calls per frame?
    void Update()
    {
        // must be called before you try to read value from osc server
        OSCHandler.Instance.UpdateLogs();

        // データ受信部
        servers = OSCHandler.Instance.Servers;
        foreach (KeyValuePair<string, ServerLog> item in servers)
        {
            // If we have received at least one packet,
            // show the last received from the log in the Debug console
            if (item.Value.log.Count > 0)
            {
                int lastPacketIndex = item.Value.packets.Count - 1;

                UnityEngine.Debug.Log(String.Format("SERVER: {0} ADDRESS: {1} VALUE 0: {2}",
                item.Key, // Server name
                item.Value.packets[lastPacketIndex].Address, // OSC address
                item.Value.packets[lastPacketIndex].Data[0].ToString())); //First data value
                // data型に合わせて出力処理
            }
        }

        // データ送信部
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("SendMessage");
            var sampleVals = new List<int>() { 1, 2, 3 };
            OSCHandler.Instance.SendMessageToClient("iPhoneTouchOSCApp", "/hoge/a", sampleVals);
        }
    }
}
