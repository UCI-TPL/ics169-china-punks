using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour {
    public Map_Control mc;
    public List<GameObject> mapInfo = new List<GameObject>();

    public int currentPos;
    public int moveRange;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public virtual void TurnUpdate(){

    }
}
