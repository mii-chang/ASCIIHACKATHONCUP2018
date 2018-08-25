//
//  ViewController.swift
//  voice
//
//  Created by kamano yurika on 2018/08/25.
//  Copyright © 2018年 litech. All rights reserved.
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

class ViewController: UIViewController {
    
    // 音声入力用のキューと監視用タイマーの準備
    var queue: AudioQueueRef!
    var recordingTimer: Timer!
    
    @IBOutlet var label: UILabel!
    @IBOutlet var levelMeterlabel: UILabel!
    
    //ジャイロセンサー
    @IBOutlet weak var xGyroLabel: UILabel!
    @IBOutlet weak var yGyroLabel: UILabel!
    @IBOutlet weak var zGyroLabel: UILabel!
    @IBOutlet weak var xGravityLabel: UILabel!
    @IBOutlet weak var yGravityLabel: UILabel!
    @IBOutlet weak var zGravityLabel: UILabel!
    @IBOutlet weak var pitchLabel: UILabel!
    @IBOutlet weak var rollLabel: UILabel!
    @IBOutlet weak var yawLabel: UILabel!

    let cmManager = CMMotionManager()

    @IBAction func tapStart(sender: AnyObject) {
        cmManager.deviceMotionUpdateInterval = 0.1
        let handler:CMDeviceMotionHandler = {
            (motionData: CMDeviceMotion?, error: NSError?) -> Void in
            self.motionAnimation(motionData: motionData, error: error! as NSError)
            } as! CMDeviceMotionHandler
        cmManager.startDeviceMotionUpdates(to: OperationQueue.main, withHandler: handler)
    }

    @IBAction func tapStop(sender: AnyObject) {
        if (cmManager.isDeviceMotionActive) {
            cmManager.stopDeviceMotionUpdates()
        }
    }

    func motionAnimation(motionData: CMDeviceMotion?, error: NSError?) {
        if let motion = motionData {
            xGyroLabel.text = String(format:"%.2f", motion.rotationRate.x)
            yGyroLabel.text = String(format:"%.2f", motion.rotationRate.y)
            zGyroLabel.text = String(format:"%.2f", motion.rotationRate.z)

            xGravityLabel.text = String(format:"%.2f", motion.gravity.x)
            yGravityLabel.text = String(format:"%.2f", motion.gravity.y)
            zGravityLabel.text = String(format:"%.2f", motion.gravity.z)

            pitchLabel.text = String(format:"%.2f", motion.attitude.pitch)
            rollLabel.text = String(format:"%.2f", motion.attitude.roll)
            yawLabel.text = String(format:"%.2f", motion.attitude.yaw)
        }
    }

    //ここまでジャイロ
    
    override func viewDidLoad() {
        super.viewDidLoad()
        // 録音レベルの検知を開始する
        self.startUpdatingVolume()
        
        if motionManager.isAccelerometerAvailable {
            //intervalの設定[sec]
            motionManager.accelerometerUpdateInterval = 0.2
            
            //センサー値の取得開始
            self.motionManager.startAccelerometerUpdates(
                to: OperationQueue.current!,
                withHandler: {(accelData: CMAccelerometerData?, errorOC: Error?) in
                    self.lowpassFilter(acceleration: accelData!.acceleration)
            })
            
        }
    }
    
    override func viewDidDisappear(_ animated: Bool) {
        super.viewDidDisappear(animated)
        // 録音レベルの検知を停止する
        self.stopUpdatingVolume()
    }
    
    //MotionManager
    let motionManager = CMMotionManager()
    
    //3 axes
    @IBOutlet var accelerometerX: UILabel!
    @IBOutlet var accelerometerY: UILabel!
    @IBOutlet var accelerometerZ: UILabel!
    
    var acceleX: Double = 0
    var acceleY: Double = 0
    var acceleZ: Double = 0
    let Alpha = 0.4
    var flg: Bool = false
    
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
    
    // 録音レベル検知処理を停止
    func stopUpdatingVolume() {
        // Finish observation
        self.recordingTimer.invalidate()
        self.recordingTimer = nil
        AudioQueueFlush(self.queue)
        AudioQueueStop(self.queue, false)
        AudioQueueDispose(self.queue, true)
    }
    
    var levelMeter = AudioQueueLevelMeterState()
    func detectVolume(timer: Timer) {
        var propertySize = UInt32(MemoryLayout<AudioQueueLevelMeterState>.size)
        
        AudioQueueGetProperty(
            self.queue,
            kAudioQueueProperty_CurrentLevelMeterDB,
            &levelMeter,
            &propertySize)
        
        // 取得した録音レベルに応じてimageを切り替える
        //statusImage.isHidden = (levelMeter.mPeakPower >= -1.0) ? false : true
        levelMeterlabel.text = String(levelMeter.mPeakPower)
        print(levelMeter.mPeakPower)
        setLabelText()
    }
    
    
    // 録音レベルの値によって行いたい処理(サンプル)
    func setLabelText () {
        if levelMeter.mPeakPower > -8.0 {
            label.text = String("👺")
            print("👺")
        } else {
            label.text = String("🤫")
            print("🤫")
        }
    }
    
    func lowpassFilter(acceleration: CMAcceleration){
        
        acceleX = Alpha * acceleration.x + acceleX * (1.0 - Alpha);
        acceleY = Alpha * acceleration.y + acceleY * (1.0 - Alpha);
        acceleZ = Alpha * acceleration.z + acceleZ * (1.0 - Alpha);
        
        accelerometerX.text = String(format: "%.3f", acceleX)
        accelerometerY.text = String(format: "%.3f", acceleY)
        accelerometerZ.text = String(format: "%.3f", acceleZ)
    }
    
    //どこかに実装
    func stopAccelerometer(){
        if (motionManager.isAccelerometerActive) {
            motionManager.stopAccelerometerUpdates()
        }
    }
    
    override func didReceiveMemoryWarning() {
        super.didReceiveMemoryWarning()
        
    }
}
