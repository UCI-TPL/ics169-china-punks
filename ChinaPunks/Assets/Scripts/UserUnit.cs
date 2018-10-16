using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserUnit : Unit {
    GameController gc;
    List<GameObject> mapInfo = new List<GameObject>(100);

    public int currentPos = 12;
    public int moveRange = 2;

    private float cd = 0.2f;

    private float next;
    private bool walking = false;

    private int ID = 0;
    private bool myTrun = false;

    // Use this for initialization
    void Start () {
        GameObject controller = GameObject.Find("Controller");                              //get reference of GameController
        gc = controller.GetComponent<GameController>();                                     //same as above
        gc.unitPos.Add(currentPos);                                                         //add my current grid# to the position list of map
        ID = gc.unitPos.Count-1;                                                            //get my ID in the position list
        mapInfo = gc.mapInfo;                                                               //get map info from GameController
        Vector3 xyPosition = mapInfo[currentPos - 1].transform.position;
        transform.position = new Vector3(xyPosition.x, xyPosition.y + 1.0f, -1.0f);         //initialize my current position on map

    }

    // Update is called once per frame
    void Update() {

        TurnUpdate();                                                                       //call turn update

        if (walking)                                                                        //if the unit need to move then...
        {
            if (gc.pickedTile.Count > 0 && gc.pickedTile[0] == currentPos)                  //check if my current position is same as the first node of the node list I get from map
            { 
                if (Time.time > next)
                {
                    Vector3 moveDestination = mapInfo[gc.pickedTile[0] - 1].transform.position;        //get the destination/goalnode I need to move to
                    moveDestination = new Vector3(moveDestination.x, moveDestination.y + 1.0f, -1.0f);
                    transform.position = moveDestination;                                   //set my position to goal node
                    next = Time.time + cd;

                    gc.pickedTile.RemoveAt(0);                                              //remove past node
                    if (gc.pickedTile.Count > 0)                                            //if the node list is empty
                    {
                        currentPos = gc.pickedTile[0];                                      //set my current position to the grid#
                        gc.unitPos[ID] = currentPos;                                        //set my position in the position list to current position
                    }
                }
            }
            else{                                                                           //the node list is now empty
                gc.mouseUp = false;                                                         //set the mouseUp flag to false
                walking = false;                                                            //set the walking flag to false (unit movement is end)
            }

        }

    }

    public override void TurnUpdate()
    {
        base.TurnUpdate();

        if (gc.mouseUp && gc.pickedTile.Count > 0)                                          //check if the map now has the node list
        {
            walking = true;                                                                 //this unit(myself) can move right now
        }
    }

}
