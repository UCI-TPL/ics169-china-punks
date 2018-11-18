using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour {


    public string charater_type;

    public int skill_damage;

	// Use this for initialization
	void Start () {
        gameObject.GetComponent<UserUnit>().charater_type = charater_type;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
