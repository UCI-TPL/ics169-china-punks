using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour {
    public Map_Control mc;
    public List<GameObject> mapInfo = new List<GameObject>();

    public int currentPos;

    // Use this for initialization
    void Start () {
        GameObject controller = GameObject.Find("map_tiles");                            //get reference of GameController
        mc = controller.GetComponent<Map_Control>();                                     //same as above

        mc.units_state[currentPos] = gameObject;
        mapInfo = mc.map_tiles;                                                          //get map info from GameController
        Vector3 xyPosition = mapInfo[currentPos].transform.position;
        transform.position = new Vector3(xyPosition.x, xyPosition.y + 0.6f, xyPosition.z - 1.0f);

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
