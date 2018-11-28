using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YaoguaiMinion : AIUnit {

    void Awake()
    {
        GameObject controller = GameObject.Find("map_tiles");                            //get reference of GameController
        mc = controller.GetComponent<Map_Control>();
        GameObject controller2 = GameObject.Find("turn_control");                            //get reference of GameController
        turn_ctr = controller2.GetComponent<Turn_Control>();

        attackRange = new List<int>() { 2, -2, 20, -20 };
    }
}
