//
//  OscRecieveViewController.swift
//  OscTestTests
//
//  Created by Miyuu Okabe on 2018/08/25.
//  Copyright © 2018年 Miyuu Okabe. All rights reserved.
//

import UIKit

//デリゲートまわりでF53OSCPacketDestinationの追加
class OscRecieveViewController: UIViewController,F53OSCPacketDestination {
    func take(_ message: F53OSCMessage!) {
        
    }
    
    
    //osc受信のためのoscServerの初期化
    let oscServer = F53OSCServer.init()
    
    override func viewDidLoad() {
        super.viewDidLoad()
        //portの指定など
        oscServer.port = 8000
        oscServer.delegate = self
        if oscServer.startListening() {
            print("Listening for messages on port \(oscServer.port)")
        } else {
            print("Error: Server was unable to start listening on port \(oscServer.port)")
        }
    }
    
    //OSC受信するためのメソッド
    func takeMessage(message: F53OSCMessage!) {
        
        print(message)
        
        //OSCmessageによる比較
        if message.addressPattern == "/value" {
            
            //OSCargumentsによる比較
            if message.arguments[0] as! Int == 123{
                print("hello 123")
            }else if message.arguments[0] as! Int == 321{
                print("hello 321")
            }
        }
    }
    
}
