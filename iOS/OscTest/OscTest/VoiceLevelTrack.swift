//
//  VoiceLevelTrackController.swift
//  OscTest
//
//  Created by Miyuu Okabe on 2018/08/25.
//  Copyright © 2018年 Miyuu Okabe. All rights reserved.
//

import UIKit
import AudioToolbox
import CoreMotion

// 録音の必要はないので AudioInputCallback は空にする
private func AudioQueueInputCallback(
    inUserData: UnsafeMutableRawPointer?,
    inAQ: AudioQueueRef,
    inBuffer: AudioQueueBufferRef,
    inSrartTime: UnsafePointer<AudioTimeStamp>,
    inNumberPacketDescriptions: UInt32,
    inPacketDescs: UnsafePointer<AudioStreamPacketDescription>?) {
}

class VoiceLevelTrack {
    
    // 音声入力用のキューと監視用タイマーの準備
    var queue: AudioQueueRef!
    var recordingTimer: Timer!
    
    var levelMeter = AudioQueueLevelMeterState()
    var sensorValue = SensorValue()
    
    //    @IBOutlet var label: UILabel!
    //    @IBOutlet var levelMeterlabel: UILabel!
//    
//    override func viewDidLoad() {
//        super.viewDidLoad()
//        
//    }
    
    init() {
        // 録音レベルの検知を開始する
        self.startUpdatingVolume()
    }
    
    
    
    
    // 以下の処理を実行したいタイミングでタイマーをスタートさせるだけで録音レベルが検知できる
    // MARK: - 録音レベルを取得する処理
    func startUpdatingVolume() {
        // 録音データを記録するフォーマットを決定
        var dataFormat = AudioStreamBasicDescription(
            mSampleRate: 44100.0,
            mFormatID: kAudioFormatLinearPCM,
            mFormatFlags: AudioFormatFlags(kLinearPCMFormatFlagIsBigEndian |
                kLinearPCMFormatFlagIsSignedInteger |
                kLinearPCMFormatFlagIsPacked),
            mBytesPerPacket: 2,
            mFramesPerPacket: 1,
            mBytesPerFrame: 2,
            mChannelsPerFrame: 1,
            mBitsPerChannel: 16,
            mReserved: 0
        )
        // オーディオキューのデータ型を定義
        var audioQueue: AudioQueueRef? = nil
        // エラーハンドリング
        var error = noErr
        // エラーハンドリング
        error = AudioQueueNewInput(
            &dataFormat,
            AudioQueueInputCallback,
            UnsafeMutableRawPointer(Unmanaged.passUnretained(self).toOpaque()),
            .none,
            .none,
            0,
            &audioQueue
        )
        if error == noErr {
            self.queue = audioQueue
        }
        AudioQueueStart(self.queue, nil)
        
        var enabledLevelMeter: UInt32 = 1
        AudioQueueSetProperty(
            self.queue,
            kAudioQueueProperty_EnableLevelMetering,
            &enabledLevelMeter,
            UInt32(MemoryLayout<UInt32>.size)
        )
        
        self.recordingTimer = Timer.scheduledTimer(withTimeInterval: 1/30, repeats: true, block: { (timer) in
            self.detectVolume(timer: timer)
        })
        
        
        self.recordingTimer?.fire()
    }
    
    func detectVolume(timer: Timer) {
        var propertySize = UInt32(MemoryLayout<AudioQueueLevelMeterState>.size)
        
        AudioQueueGetProperty(
            self.queue,
            kAudioQueueProperty_CurrentLevelMeterDB,
            &levelMeter,
            &propertySize)
        
        // 取得した録音レベルに応じてimageを切り替える
        //statusImage.isHidden = (levelMeter.mPeakPower >= -1.0) ? false : true
        //        levelMeterlabel.text = String(levelMeter.mPeakPower)
//        print(levelMeter.mPeakPower)
        
        sensorValue.volumeValue = levelMeter.mPeakPower
    }
    
    // 録音レベルの値によって行いたい処理(サンプル)
    func setLabelText () {
        if levelMeter.mPeakPower > -8.0 {
            //            label.text = String("👺")
            print("👺")
        } else {
            //            label.text = String("🤫")
            print("🤫")
        }
    }
    
    // 録音レベル検知処理を停止
    func stopUpdatingVolume() {
        // Finish observation
        self.recordingTimer.invalidate()
        self.recordingTimer = nil
        AudioQueueFlush(self.queue)
        AudioQueueStop(self.queue, false)
        AudioQueueDispose(self.queue, true)
    }
    
    
//    override func viewDidDisappear(_ animated: Bool) {
//        super.viewDidDisappear(animated)
//        // 録音レベルの検知を停止する
//        self.stopUpdatingVolume()
//    }
//
//    override func didReceiveMemoryWarning() {
//        super.didReceiveMemoryWarning()
//
//    }
}
