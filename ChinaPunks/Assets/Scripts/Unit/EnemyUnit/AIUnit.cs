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

    public Animator anim;
    public Image healthFillImage;

    public float attack_damage;

    public float moveSpeed = 1;
    public bool slippery = false;

    public int fire_cd;
    int _fire_cd;
    public int poison_cd;
    int _poison_cd;
    public int poison_damage;
    public int fire_damage;
    public int trap_damage;

    public bool anim_is_playing;

    Vector3 moveDestination = new Vector3();

    private int myIndex;

    // Use this for initialization
    void Start()
    {
        GameObject controller = GameObject.Find("map_tiles");                            //get reference of GameController
        mc = controller.GetComponent<Map_Control>();                                     //same as above

        mc.units_state[currentPos] = gameObject;
        mc.AI_units.Add(gameObject);
        myIndex = mc.AI_units.Count;



        mapInfo = mc.map_tiles;                                                          //get map info from GameController
        Vector3 xyPosition = mapInfo[currentPos].transform.position;
        transform.position = new Vector3(xyPosition.x, xyPosition.y + 0.7f, xyPosition.z - 1.0f);      //initialize my current position on map
        _fire_cd = fire_cd;
    }

    // Update is called once per frame
    void Update()
    {

        if (acting && turn_ctr.gameRound == "AI")
        {
            TurnUpdate();
            if (mc.character_moving)                                                                     //if the unit need to move then...
            {
                if (hasPeach)
                {
                    mc.peach_pos = currentPos;
                }

                float step = moveSpeed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, moveDestination, step);

            }
        }


    }

    public override void TurnUpdate()
    {
        base.TurnUpdate();
        //if path got, starts walking
        if (mc.path.Count > 0)
        {
            moveDestination = mapInfo[mc.path[0]].transform.position;
            moveDestination = new Vector3(moveDestination.x, moveDestination.y + 0.7f, moveDestination.z - 1.0f);
            //already move to the destination(next tile)
            if (transform.position == moveDestination && currentPos == mc.path[0])
            {
                mc.path.RemoveAt(0);
                //check if there is next tile to go to
                if (mc.path.Count > 0)
                {
                    //check if next tile is snow tile
                    if (mc.map_tiles[mc.path[0]].GetComponent<Tile>().tile_type == "Snow")
                    {
                        Debug.Log("Splippery!!!!!!!!!!!!!!!");
                        int next_tile = mc.path[0];
                        int offset = mc.path[0] - currentPos;
                        int slippery_tile = next_tile + offset;
                        mc.path = new List<int>();
                        mc.path.Add(next_tile);
                        //no unit on slippery tile
                        if (mc.units_state[slippery_tile] == null)
                        {
                            int next_tile_x = next_tile % mc.map_size;
                            int next_tile_y = next_tile / mc.map_size;
                            int slippery_tile_x = slippery_tile % mc.map_size;
                            int slippery_tile_y = slippery_tile / mc.map_size;
                            if (Mathf.Abs(next_tile_x - slippery_tile_x) <= 1
                               && Mathf.Abs(next_tile_y - slippery_tile_y) <= 1)
                            {
                                slippery = true;
                                mc.path.Add(slippery_tile);
                            }
                        }
                    }
                    //check if next tile is muddy tile
                    else if (mc.map_tiles[mc.path[0]].GetComponent<Tile>().tile_type == "Muddy")
                    {
                        Debug.Log("Muddy!!!!!!!!!!!!!!!");
                        int next_tile = mc.path[0];
                        mc.path = new List<int>();
                        mc.path.Add(next_tile);
                    }
                    //check if next tile is on fire
                    //if (mc.map_tiles[mc.path[0]].GetComponent<Tile>().on_fire)
                        //on_fire = true;

                    //if next tile is snow, increase movespeed
                    if (slippery && mc.path.Count <= 1)
                        moveSpeed = 3;

                    mc.units_state[currentPos] = null;
                    currentPos = mc.path[0];
                    mc.units_state[currentPos] = gameObject;
                    moveDestination = mapInfo[mc.path[0]].transform.position;
                    moveDestination = new Vector3(moveDestination.x, moveDestination.y + 0.7f, moveDestination.z - 1.0f);
                    mc.character_moving = true;
                }
                //AI finished moving, start attacking or picking the peach
                else{
                    //if moves to fire tile, change health
                    if (mc.map_tiles[currentPos].GetComponent<Tile>().on_fire)
                    {
                        Health_Change(fire_damage);
                        on_fire = true;
                    }

                    //if moves to trap tile, change health
                    if (mc.map_tiles[currentPos].GetComponent<Tile>().trap)
                    {
                        Health_Change(trap_damage);
                        mc.map_tiles[currentPos].GetComponent<Tile>().trap.GetComponent<trap>().triggered = true;
                    }

                    //attack first
                    if (!turnComplete)
                    {
                        foreach (int position in attackRange)
                        {
                            if (currentPos + position >= 0 && currentPos + position <= mc.map_size * mc.map_size - 1)
                            {
                                if (mc.units_state[currentPos + position] != null
                                    && mc.units_state[currentPos + position].gameObject.tag == "PlayerUnit"
                                    && !hasPeach
                                    && !mc.units_state[currentPos + position].GetComponent<Unit>().hide)
                                {
                                    if (!mc.provocative)
                                    {
                                        mc.units_state[currentPos + position].GetComponent<UserUnit>().Health_Change(attack_damage);
                                        turnComplete = true;
                                        break;
                                    }
                                    else if (mc.units_state[currentPos + position].GetComponent<UserUnit>().provocative)
                                    {
                                        mc.units_state[currentPos + position].GetComponent<UserUnit>().Health_Change(attack_damage);
                                        turnComplete = true;
                                        break;
                                    }
                                }

                            }
                        }
                    }
                    //if didn't attack, check if can pick the peach
                    if(!turnComplete)
                    {
                        foreach (int position in pickRange)
                        {
                            if (currentPos + position >= 0 && currentPos + position <= mc.map_size * mc.map_size - 1)
                            {
                                if (mc.units_state[currentPos + position] != null && mc.units_state[currentPos + position].gameObject.CompareTag("Peach")
                                         && !hasPeach)
                                {
                                    GameObject peach = mc.units_state[currentPos + position].gameObject;
                                    mc.peach_pos = currentPos;

                                    hasPeach = true;
                                    turnComplete = true;
                                    mc.units_state[currentPos + position] = null;
                                    Destroy(peach);
                                    break;
                                }
                            }
                        }
                    }
                    acting = false;
                    mc.character_moving = false;
                    moveSpeed = 1;
                    slippery = false;

                }

            }
        }
    }

    public override void Health_Change(float damage)
    {
        //if AI dies, destory it after the anim finishes
        StartCoroutine(waitforanim(anim));
        //anim.Play("Attacked");
        base.Health_Change(damage);
        if (health <= 0){
            moveComplete = true;
            turnComplete = true;
            acting = false;
        }

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

    public virtual void Reset_FireCD()
    {
        fire_cd = _fire_cd;
    }
    public virtual void Reset_PoisonCD()
    {
        poison_cd = _poison_cd;
    }

    IEnumerator waitforanim(Animator anim){
        anim.Play("Attacked");
        mc.animation_is_playing = true;
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length-0.1f);
        mc.animation_is_playing = false;
        if (health <= 0)
        {
            mc.AI_units.Remove(gameObject);
            Debug.Log(this.gameObject.name + " is Dead!");
            mc.units_state[currentPos] = null;
            Destroy(this.gameObject);
        }

        //while (
        //    true
        ////Time.time < t + anim.GetCurrentAnimatorStateInfo(0).length);
        //;
    }
}