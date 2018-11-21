using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AIUnit : Unit
{

    //Variables for AI unit moving and attacking
    public List<int> attackRange = new List<int>() { 1, -1, 10, -10 };
    public List<int> pickRange = new List<int>() { 1, -1, 10, -10 };
    public bool hasPeach = false;
    public bool acting = false;
    public bool walking = false;
    public int moveRange;
    private float cd = 0.2f;
    private float next;

    public Animator anim;
    public Image healthFillImage;

    // Use this for initialization
    void Start()
    {      
        GameObject controller = GameObject.Find("map_tiles");                            //get reference of GameController
        mc = controller.GetComponent<Map_Control>();                                     //same as above

        mc.units_state[currentPos] = this.gameObject;
        mc.AI_units.Add(this.gameObject);

        mapInfo = mc.map_tiles;                                                          //get map info from GameController
        Vector3 xyPosition = mapInfo[currentPos].transform.position;
        transform.position = new Vector3(xyPosition.x, xyPosition.y + 0.8f, xyPosition.z - 1.0f);      //initialize my current position on map
    }

    // Update is called once per frame
    void Update()
    {

        if (acting)
        {
            TurnUpdate();                                                                //call turn update
        }

        if (walking)                                                                     //if the unit need to move then...
        {
            if (hasPeach){
                mc.peach_pos = currentPos;
            }

            if (mc.path.Count > 0 && mc.path[0] == currentPos)                           //check if my current position is same as the first node of the node list I get from map
            {
                if (Time.time > next)
                {
                    Vector3 moveDestination = mapInfo[mc.path[0]].transform.position;    //get the destination/goalnode I need to move to
                    moveDestination = new Vector3(moveDestination.x, moveDestination.y + 0.8f, moveDestination.z - 1.0f);
                    transform.position = moveDestination;                                //set my position to destination
                    next = Time.time + cd;

                    mc.path.RemoveAt(0);                                                 //remove past node
                    if (mc.path.Count > 0)
                    {                                                                    //if the node list is empty
                        mc.units_state[currentPos] = null;
                        currentPos = mc.path[0];                                         //set my current position to the grid#
                        mc.units_state[currentPos] = this.gameObject;
                    }
                }
            }
            else
            {                                                                        //the node list is now empty
                                                                                     //Finish moving, check if the unit can attack or cast spell here
                foreach (int position in attackRange)
                {
                    if (currentPos + position >= 0 && currentPos + position <= mc.map_size * mc.map_size - 1)
                    {
                        if (mc.units_state[currentPos + position] != null && mc.units_state[currentPos + position].gameObject.tag != this.gameObject.tag
                            && !mc.units_state[currentPos + position].gameObject.CompareTag("Peach") && !hasPeach)
                        {
                            mc.units_state[currentPos + position].GetComponent<Unit>().Health_Change(attack_damge);
                            break;
                        }
                        else if (mc.units_state[currentPos + position] != null && mc.units_state[currentPos + position].gameObject.CompareTag("Peach")
                                 && !hasPeach)
                        {
                            GameObject peach = mc.units_state[currentPos + position].gameObject;
                            mc.peach_pos = currentPos;

                            hasPeach = true;

                            mc.units_state[currentPos + position] = null;
                            Destroy(peach);
                            break;
                        }
                    }
                }

                walking = false;                                                         //set the walking flag to false (unit movement is end)
                mc.units_state[currentPos] = this.gameObject;
                acting = false;
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

    public override void Health_Change(float damage)
    {
        base.Health_Change(damage);
        mc.AI_units.Remove(this.gameObject);

        anim.Play("Attacked");

        if (hasPeach)
        {
            int peach_pos = mc.peach_pos;
            foreach (int i in pickRange)
            {
                if (mc.units_state[i + peach_pos] == null)
                {
                    GameObject peach = Instantiate(mc.PeachPrefab);
                    peach.gameObject.GetComponent<Peach>().currentPos = i + peach_pos;
                    break;
                }
            }
            hasPeach = false;
        }
        healthFillImage.fillAmount = health / 100f;
    }
}
