﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour {
    public Map_Control mc;
    public List<GameObject> mapInfo = new List<GameObject>();

    public int currentPos;
    public int moveRange;

    public float health;
    public float attack_damge;

<<<<<<< HEAD
=======
    public Animator anim;

>>>>>>> 350ff241c047136fb1e29268cd32eb7b429084db
    //Check if a unit has complete its turn
    public bool moveComplete;
    public bool turnComplete;

    // Use this for initialization
    void Start () {
        GameObject controller = GameObject.Find("map_tiles");                            //get reference of GameController
        mc = controller.GetComponent<Map_Control>();                                     //same as above

        mc.units_state[currentPos] = this.gameObject;

        mapInfo = mc.map_tiles;                                                          //get map info from GameController
        Vector3 xyPosition = mapInfo[currentPos].transform.position;
        transform.position = new Vector3(xyPosition.x, xyPosition.y + 0.6f, xyPosition.z - 1.0f);      //initialize my current position on map
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public virtual void TurnUpdate(){

    }

    public virtual void Health_Change(float damage){
<<<<<<< HEAD
=======
        anim.Play("Attacked");
>>>>>>> 350ff241c047136fb1e29268cd32eb7b429084db
        health -= damage;
        Debug.Log("Ahhhh, damage taken: " + damage.ToString());

        if (health <= 0){
            Debug.Log(this.gameObject.name + " is Dead!");
            Destroy(this.gameObject);
        }
    }
}
