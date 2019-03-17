using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NianShou : AIUnit {

    float _moveSpeed;

    private SpriteRenderer SR;

    Vector3 moveDestination = new Vector3();

    // Use this for initialization
    void Start () {
        attackRange = new List<int>() { 1, -1, mc.map_size, -mc.map_size };
        mc.units_state[currentPos] = gameObject;
        mc.AI_units.Add(gameObject);
        myIndex = mc.AI_units.Count;



        mapInfo = mc.map_tiles;                                                          //get map info from GameController
        Vector3 xyPosition = mapInfo[currentPos].transform.position;
        transform.position = new Vector3(xyPosition.x, xyPosition.y + 0.4f, xyPosition.z - 1.0f);      //initialize my current position on map
        _fire_cd = fire_cd;
        _moveSpeed = moveSpeed;
        current_health = health;

        SR = this.gameObject.GetComponent<SpriteRenderer>();
        face_right = true;
    }
	
	// Update is called once per frame
	void Update () {
        if (acting && turn_ctr.gameRound == "AI")
        {
            TurnUpdate();
            if (mc.character_moving)                                                                     //if the unit need to move then...
            {
                if ((this.transform.position.x - moveDestination.x) > 0)
                {
                    face_right = true;
                    SR.flipX = false;
                }
                else if ((this.transform.position.x - moveDestination.x) <= 0)
                {
                    face_right = false;
                    SR.flipX = true;
                }

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
        //if path got, starts walking
        if (mc.path.Count > 0)
        {
            moveDestination = mapInfo[mc.path[0]].transform.position;
            moveDestination = new Vector3(moveDestination.x, moveDestination.y + 0.4f, moveDestination.z - 1.0f);
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
                        moveSpeed += 3;

                    mc.units_state[currentPos] = null;
                    currentPos = mc.path[0];
                    mc.units_state[currentPos] = gameObject;
                    moveDestination = mapInfo[mc.path[0]].transform.position;
                    moveDestination = new Vector3(moveDestination.x, moveDestination.y + 0.7f, moveDestination.z - 1.0f);
                    mc.character_moving = true;
                }
                //AI finished moving, start attacking or picking the peach
                else
                {
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
                                    && mc.units_state[currentPos + position].gameObject.name.StartsWith("Peach"))
                                {
                                    anim.Play("Attack");
                                    AS.PlayOneShot(Attack_Clip, 1.0f);
                                    Destroy(mc.units_state[currentPos + position].gameObject);
                                    mc.peach_count--;
                                    turnComplete = true;
                                    break;
                                }

                            }
                        }
                    }
                    //if didn't attack, check if can pick the peach
                    if (!turnComplete)
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
                    moveSpeed = _moveSpeed;
                    slippery = false;

                }

            }
        }
    }


}
