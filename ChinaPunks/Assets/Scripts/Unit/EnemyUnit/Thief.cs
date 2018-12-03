using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thief : AIUnit {
    void Awake()
    {
        GameObject controller = GameObject.Find("map_tiles");                            //get reference of GameController
        mc = controller.GetComponent<Map_Control>();
        GameObject controller2 = GameObject.Find("turn_control");                            //get reference of GameController
        turn_ctr = controller2.GetComponent<Turn_Control>();

        moveRange = 3;
    }
}
