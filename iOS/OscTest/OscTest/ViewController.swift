//
//  ViewController.swift
//  OscTest
//
//  Created by Miyuu Okabe on 2018/08/25.
//  Copyright © 2018年 Miyuu Okabe. All rights reserved.
//

import UIKit

class ViewController: UIViewController {
    var osc = OscSend()
    var timer = Timer()
    var timerInterval = 0.05
    var playerId = 1

    @IBOutlet var button: UIButton!
    @IBOutlet var segmentControl: UISegmentedControl!
    
    @IBAction func segment(senfer: UISegmentedControl){
        switch senfer.selectedSegmentIndex {
        case 0:
            playerId = 1
            break
        case 1:
            playerId = 2
            break
        default:
            playerId = 1
            break
        }
    }
    
    
    @IBAction func start(sender: AnyObject) {
        var message:[Int] = [0]
        let voiceLevelTrack = VoiceLevelTrack()
        /**
         * 0 = false
         * 1 = true
         */
        var isLoudVoice = 0
        timer = Timer.scheduledTimer(withTimeInterval: timerInterval,
                                     repeats: true,
                                     block: {_ in
                                        /**
                                         *message = [deviceType, playerId, isLoudVoice]
                                         */
                                        print(voiceLevelTrack.sensorValue.volumeValue)
                                        if(voiceLevelTrack.sensorValue.volumeValue > -1){
                                            isLoudVoice = 1
                                        }else {
                                            isLoudVoice = 0
                                        }
                                        message = [self.playerId, isLoudVoice]
                                        self.osc.sendOSC(message:message)
        })
    }
    
    @IBAction func stop(sender: AnyObject){
        timer.invalidate()
    }
    
    override func viewDidLoad() {
        super.viewDidLoad()
        // Do any additional setup after loading the view, typically from a nib.
    }
    
    override func didReceiveMemoryWarning() {
        super.didReceiveMemoryWarning()
        // Dispose of any resources that can be recreated.
    }
    
    
}

