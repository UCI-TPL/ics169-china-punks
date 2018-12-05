using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserUnit : Unit
{

    public bool hasPeach = false;
    public bool isClicked = false;
    public List<int> attackRange = new List<int>() { 1, -1, 10, -10 };
    public List<int> pickRange = new List<int>() { 1, -1, 10, -10 };

    public int moveRange;
    public float attack_damage;
    public float skill_damage;

    public Image healthFillImage;
    public Animator anim;

    public GameObject selectEffect;
    GameObject _selectEffect;
    bool selectEffect_exist = false;

    public string charater_type;

    public int coolDown = 0;
    public float moveSpeed = 1;
    public bool slippery = false;

    public int fire_cd;
    int _fire_cd;

    public int poison_cd;
    int _poison_cd;

    public int poison_damage;
    public int fire_damage;
    public int trap_damage;

    public int skill_cd;




    Vector3 moveDestination = new Vector3();


    // Use this for initialization
    void Start()
    {
        //currentPos = 11;
        //moveRange = 2;
        mc.units_state[currentPos] = gameObject;

        mapInfo = mc.map_tiles;                                                          //get map info from GameController
        Vector3 xyPosition = mapInfo[currentPos].transform.position;
        transform.position = new Vector3(xyPosition.x, xyPosition.y + 0.7f, xyPosition.z - 1.0f);      //initialize my current position on map
        _fire_cd = fire_cd;
    }

    // Update is called once per frame
    void Update()
    {

        if (isClicked && turn_ctr.gameRound == "Player")
        {
            TurnUpdate();
            if (mc.character_moving)
            {
                float step = moveSpeed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, moveDestination, step);
            }


        }

        if (hide)
        {
            if (transform.position == moveDestination)
            {
                gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f);
            }
        }
        else
        {
            if (transform.position == moveDestination)
            {
                gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
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
                        Debug.Log("Slippery!!!!!!!!!!!!!!!");
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
                    if (mc.map_tiles[mc.path[0]].GetComponent<Tile>().tile_type == "Muddy")
                    {
                        Debug.Log("Muddy!!!!!!!!!!!!!!!");
                        int next_tile = mc.path[0];
                        mc.path = new List<int>();
                        mc.path.Add(next_tile);
                    }
                    //check if next tile is hide tile
                    if (mc.map_tiles[mc.path[0]].GetComponent<Tile>().tile_type == "Hide")
                    {
                        hide = true;
                    }
                    else
                    {
                        hide = false;
                    }
                    //check if next tile is on fire
                    //if (mc.map_tiles[mc.path[0]].GetComponent<Tile>().on_fire)
                    //on_fire = true;

                    //if next tile is snow, increase movespeed
                    if (slippery && mc.map_tiles[currentPos].GetComponent<Tile>().tile_type == "Snow")//&& mc.path.Count <= 1)
                        moveSpeed = 3;
                    //if walk through a fire tile, change health
                    //if (on_fire && mc.map_tiles[currentPos].GetComponent<Tile>().on_fire)
                    //Health_Change(fire_damage);

                    mc.units_state[currentPos] = null;
                    mc.picked_pos = mc.path[0];
                    mc.pickTile = mc.pickEndTile = mc.map_tiles[mc.path[0]];
                    currentPos = mc.path[0];
                    mc.units_state[currentPos] = gameObject;
                    moveDestination = mapInfo[mc.path[0]].transform.position;
                    moveDestination = new Vector3(moveDestination.x, moveDestination.y + 0.7f, moveDestination.z - 1.0f);
                    mc.character_moving = true;
                }
                //if no next tile, moveing ends
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

                    isClicked = false;
                    mc.character_moving = false;
                    moveSpeed = 1;
                    slippery = false;
                }
            }
        }

    }

    public override void Health_Change(float damage)
    {
        base.Health_Change(damage);
        StartCoroutine(waitforanim(anim));

        //anim.Play("Attacked");
        hide = false;

        if (hasPeach)
        {
            int peach_pos = mc.peach_pos;
            foreach (int i in pickRange)
            {
                if (mc.units_state[i + peach_pos] == null)
                {
                    if (i + peach_pos >= 0 && i + peach_pos <= mc.map_size * mc.map_size - 1)
                    {
                        if (peach_pos % 10 != 0)
                        {
                            if ((peach_pos + i) % 10 != 0)
                            {
                                GameObject peach = Instantiate(mc.PeachPrefab);
                                peach.gameObject.GetComponent<Peach>().currentPos = i + peach_pos;
                                break;
                            }
                        }
                    }
                }
            }
            hasPeach = false;
        }
        healthFillImage.fillAmount = health / 100f;

        //if (health <= 0)
        //{
        //    Debug.Log(this.gameObject.name + " is Dead!");
        //    mc.units_state[currentPos] = null;
        //    Destroy(this.gameObject);
        //}
    }

    public void show_clickEffect()
    {
        if (!selectEffect_exist)
        {
            _selectEffect = Instantiate(selectEffect, gameObject.GetComponent<Transform>());
            _selectEffect.transform.Translate(new Vector3(0, -0.8f, 0));
            selectEffect_exist = true;
        }
    }

    public void destory_clickEffect()
    {
        if (selectEffect_exist)
        {
            Destroy(_selectEffect);
            selectEffect_exist = false;
        }
    }

    public virtual void Skill()
    {

    }
    public virtual void Reset_Skill()
    {

    }
    public virtual void Reset_FireCD()
    {
        fire_cd = _fire_cd;
    }
    public virtual void Reset_PoisonCD()
    {
        poison_cd = _poison_cd;
    }

    IEnumerator waitforanim(Animator anim)
    {
        anim.Play("Attacked");
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length - 0.1f);
        if (health <= 0)
        {
            Debug.Log(this.gameObject.name + " is Dead!");
            mc.units_state[currentPos] = null;
            Destroy(this.gameObject);
        }

    }
}
