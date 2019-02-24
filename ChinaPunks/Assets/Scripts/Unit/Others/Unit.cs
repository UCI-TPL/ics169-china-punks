using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour {
    public Map_Control mc;
    public Turn_Control turn_ctr;
    public List<GameObject> mapInfo = new List<GameObject>();

    public int currentPos;
    public bool hide;
    public bool on_fire;
    public bool provocative;
    public bool poisoned;
    public bool In_Fog = true;

    public float health;
    public float current_health;

    //Check if a unit has complete its turn
    public bool moveComplete;
    public bool turnComplete;

    // Use this for initialization
    void Start () {
        In_Fog = true;

        mc.units_state[currentPos] = gameObject;

        mapInfo = mc.map_tiles;                                                          //get map info from GameController
        Vector3 xyPosition = mapInfo[currentPos].transform.position;
        transform.position = new Vector3(xyPosition.x, xyPosition.y + 0.5f, xyPosition.z - 1.0f);      //initialize my current position on map
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public virtual void TurnUpdate(){

    }

    public virtual void Health_Change(float damage){
        current_health -= damage;
        Debug.Log("Ahhhh, damage taken: " + damage.ToString());
    }
}
