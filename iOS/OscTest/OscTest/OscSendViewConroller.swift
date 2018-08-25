//
//  OscSendViewConroller.swift
//  OscTest
//
//  Created by Miyuu Okabe on 2018/08/25.
//  Copyright © 2018年 Miyuu Okabe. All rights reserved.
//

import UIKit

class OscSendViewController: UIViewController {
    
    //osc送信のためのoscClientの初期化
    let oscClient = F53OSCClient.init()
    
    override func viewDidLoad() {
        super.viewDidLoad()
        
        //OSCを送りたい先のIPアドレスを指定
        oscClient.host = "192.168.100.101"
        //贈りたい先のport番号を指定
        oscClient.port = 8000
        sendOSC()
    }
    
     func sendOSC(){
        //123というメッセージをOSCで送信
        let message : [Int] = [1,2,3,4]
        self.sendMessage(client: oscClient, addressPattern: "/value", arguments: message)
        //複数の値を送る場合はarguments:[123,231,312]
    }
    
    //osc送信のためのメソッド
    func sendMessage(client: F53OSCClient, addressPattern: String, arguments: [Int]) {
        let message = F53OSCMessage(addressPattern: addressPattern, arguments: arguments)
        client.send(message)
        print("Sent '\(message)' to \(client.host):\(client.port)")
    }
}
