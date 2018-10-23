using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserUnit : Unit {

    public bool isClicked = false;
    public List<int> attackRange = new List<int>() { 1, -1, 10, -10 };

    private float cd = 0.2f;
    private float next;
    private bool walking = false;
    

    // Use this for initialization
    void Start () {
        //currentPos = 11;
        //moveRange = 2;

        GameObject controller = GameObject.Find("map_tiles");                            //get reference of GameController
        mc = controller.GetComponent<Map_Control>();                                     //same as above

        mc.units_state[currentPos] = this.gameObject;

        mapInfo = mc.map_tiles;                                                          //get map info from GameController
        Vector3 xyPosition = mapInfo[currentPos].transform.position;
        transform.position = new Vector3(xyPosition.x, xyPosition.y + 0.8f, xyPosition.z - 1.0f);      //initialize my current position on map

    }

    // Update is called once per frame
    void Update() {

        if (isClicked)
        {
            TurnUpdate();                                                                //call turn update
        }

        if (walking)                                                                     //if the unit need to move then...
        {
            if (mc.path.Count > 0 && mc.path[0] == currentPos)                           //check if my current position is same as the first node of the node list I get from map
            {
                if (Time.time > next)
                {
                    Vector3 moveDestination = mapInfo[mc.path[0]].transform.position;    //get the destination/goalnode I need to move to
                    moveDestination = new Vector3(moveDestination.x, moveDestination.y + 0.8f, moveDestination.z - 1.0f);
                    transform.position = moveDestination;                                //set my position to destination
                    next = Time.time + cd;

                    mc.path.RemoveAt(0);                                                 //remove past node
                    if (mc.path.Count > 0)                                               //if the node list is empty
                    {
                        currentPos = mc.path[0];                                         //set my current position to the grid#
                    }
                }
            }
            else{                                                                        //the node list is now empty
                walking = false;                                                         //set the walking flag to false (unit movement is end)
                mc.units_state[currentPos] = this.gameObject;
                isClicked = false;
            }
        }
    }

    public override void TurnUpdate()
    {
        base.TurnUpdate();

        if (mc.path.Count > 0)                                          //check if the map now has the node list
        {
            walking = true;                                                              //this unit(myself) can move right now
            mc.units_state[currentPos] = null;
        }
    }
}
