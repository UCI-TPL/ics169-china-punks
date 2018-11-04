using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Peach : Unit {

	// Use this for initialization
	void Start () {
        currentPos = 99;

        GameObject controller = GameObject.Find("map_tiles");                            //get reference of GameController
        mc = controller.GetComponent<Map_Control>();                                     //same as above

        mc.units_state[currentPos] = this.gameObject;

        mapInfo = mc.map_tiles;                                                          //get map info from GameController
        Vector3 xyPosition = mapInfo[currentPos].transform.position;
        transform.position = new Vector3(xyPosition.x, xyPosition.y + 0.4f, xyPosition.z - 1.0f);      //initialize my current position on map
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
