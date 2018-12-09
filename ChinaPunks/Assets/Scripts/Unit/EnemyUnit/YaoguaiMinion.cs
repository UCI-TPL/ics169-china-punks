using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YaoguaiMinion : AIUnit {

    void  Start()
    {
        //GameObject controller = GameObject.Find("map_tiles");                            //get reference of GameController
        //mc = controller.GetComponent<Map_Control>();
        //GameObject controller2 = GameObject.Find("turn_control");                            //get reference of GameController
        //turn_ctr = controller2.GetComponent<Turn_Control>();

        mc.units_state[currentPos] = gameObject;
        mc.AI_units.Add(gameObject);
        myIndex = mc.AI_units.Count;



        mapInfo = mc.map_tiles;                                                          //get map info from GameController
        Vector3 xyPosition = mapInfo[currentPos].transform.position;
        transform.position = new Vector3(xyPosition.x, xyPosition.y + 0.7f, xyPosition.z - 1.0f);      //initialize my current position on map
        _fire_cd = fire_cd;
        current_health = health;

        attackRange = new List<int>() { 2, -2, 20, -20 };
    }

}
