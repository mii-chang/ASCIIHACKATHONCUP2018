//
//  SensorValue.swift
//  OscTest
//
//  Created by Miyuu Okabe on 2018/08/25.
//  Copyright © 2018年 Miyuu Okabe. All rights reserved.
//

import Foundation
class SensorValue {
    var volume = 0.0
    
    var volumeValue:Float {
        get{
            return Float(self.volume)
        }
        
        set(value){
            volume = Double(value)
        }
    }
}
