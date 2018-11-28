using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tile : MonoBehaviour {

    public bool exit;
    public GameObject map_tiles;
    Map_Control map_ctr;

    public GameObject turn_control;
    Turn_Control turn_ctr;

    public string tile_type;
    public bool on_fire;
    public int fire_cd;
    public int fire_damage;
    int _fire_cd;


	// Use this for initialization
	void Start () {
        _fire_cd = fire_cd;
        map_ctr = map_tiles.GetComponent<Map_Control>();
        turn_ctr = turn_control.GetComponent<Turn_Control>();
    }
	
	// Update is called once per frame
	void Update () {

    }

    private void OnMouseDown()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            //it's player's round
            if (turn_ctr.gameRound == "Player" && !map_ctr.character_moving)
               {
                map_ctr.pickTile = gameObject;
                map_ctr.tile_picked = true;
                //current tile has player unit, so show playerHUD
                if (map_ctr.units_state[map_ctr.map_tiles_pos[gameObject]] != null &&
                    map_ctr.units_state[map_ctr.map_tiles_pos[gameObject]].tag == "PlayerUnit" &&
                    !map_ctr.units_state[map_ctr.map_tiles_pos[gameObject]].GetComponent<UserUnit>().turnComplete)
                {
                    //playerHUD_showed = true;
                    map_ctr.playerHUD_showed = true;

                }
            }

        }
    }

    //for monk skill range showing
    private void OnMouseEnter()
    {
        if(map_ctr.acting_state == 4){
            map_ctr.color_skill_tiles(map_ctr.map_tiles_pos[gameObject]);
        }
    }

    private void OnMouseExit()
    {
        if (map_ctr.acting_state == 4)
        {
            map_ctr.uncolor_skill_tiles(map_ctr.map_tiles_pos[gameObject]);
        }
    }

    public void update_fire(){
        if(on_fire){
            if (fire_cd != 0)
                fire_cd--;
            else
            {
                on_fire = false;
                fire_cd = _fire_cd;
            }
        }
        
    }
}
