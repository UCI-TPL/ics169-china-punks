using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit_icon : MonoBehaviour {

    public int currentPos;
    public GameObject map;
    Map_Control map_ctr;
	void Start () {
        map_ctr = map.GetComponent<Map_Control>();
        Vector3 exit_tile_pos = map_ctr.map_tiles[currentPos].transform.position;
        transform.position = new Vector3(exit_tile_pos.x, exit_tile_pos.y + 0.5f, exit_tile_pos.z - 1.0f);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
