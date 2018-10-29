using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseTileDetection : MonoBehaviour {

    public GameObject map_tiles;
    Map_Control map_ctr;

    public GameObject turn_control;
    Turn_Control turn_ctr;


	// Use this for initialization
	void Start () {
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
            //it's player's round and players' hud isn't showed
            if (turn_ctr.gameRound == "Player" && 
                !(map_ctr.tile_picked && map_ctr.first_click && map_ctr.playerHUD_showed))
               {
                map_ctr.pickTile = gameObject;
                map_ctr.tile_picked = true;
                //current tile has player unit, so show playerHUD
                if (map_ctr.units_state[map_ctr.map_tiles_pos[gameObject]] != null &&
                    map_ctr.units_state[map_ctr.map_tiles_pos[gameObject]].tag == "PlayerUnit")
                //playerHUD_showed = true;
                map_ctr.playerHUD_showed = true;
            }

        }
    }
}
