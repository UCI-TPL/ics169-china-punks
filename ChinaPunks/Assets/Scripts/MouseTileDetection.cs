using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseTileDetection : MonoBehaviour {

    public GameObject map_tiles;
    Map_Control map_control;


	// Use this for initialization
	void Start () {
        map_control = map_tiles.GetComponent<Map_Control>();
    }
	
	// Update is called once per frame
	void Update () {
        

    }

    private void OnMouseDown()
    {
        if(!EventSystem.current.IsPointerOverGameObject())
		if(map_control.gameRound == "Player"){
			map_control.pickTile = gameObject;
            map_control.tile_picked = true;
		}
    }
}
