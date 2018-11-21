using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserUnit : Unit {

    public bool hasPeach = false;
    public bool isClicked = false;
    public List<int> attackRange = new List<int>() { 1, -1, 10, -10 };
    public List<int> pickRange = new List<int>() { 1, -1, 10, -10 };
    

    private float cd = 0.2f;
    private float next;
    private bool walking = false;

    public int moveRange;

    public Image healthFillImage;
    public Animator anim;

    public GameObject selectEffect;
    GameObject _selectEffect;
    bool selectEffect_exist = false;

    public string charater_type;

    public float skill_damage;
    public float attack_damage;


    // Use this for initialization
    void Start () {
        //currentPos = 11;
        //moveRange = 2;
        mc.units_state[currentPos] = gameObject;

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
                        mc.units_state[currentPos] = null;
                        currentPos = mc.path[0];                                         //set my current position to the grid#
                        mc.units_state[currentPos] = this.gameObject;
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
        }
    }

    public override void Health_Change(float damage)
    {
        base.Health_Change(damage);

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

    public void show_clickEffect(){
        if (!selectEffect_exist)
        {
            _selectEffect = Instantiate(selectEffect, gameObject.GetComponent<Transform>());
            _selectEffect.transform.Translate(new Vector3(0, -0.8f, 0));
            selectEffect_exist = true;
        }
    }

    public void destory_clickEffect(){
        if (selectEffect_exist)
        {
            Destroy(_selectEffect);
            selectEffect_exist = false;
        }
    }

    public virtual void Skill(){

    }
}
