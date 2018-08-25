//
//  ViewController.swift
//  OscTest
//
//  Created by Miyuu Okabe on 2018/08/25.
//  Copyright © 2018年 Miyuu Okabe. All rights reserved.
//

import UIKit

class ViewController: UIViewController {
    
    @IBOutlet var button: UIButton!
    @IBAction func click(sender: AnyObject) {
        var osc = OscSendViewController()
        osc.viewDidLoad()
    }
    
    @IBAction func get(sender: AnyObject) {
        var oscGet = OscSendViewController()
        oscGet.viewDidLoad()
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

