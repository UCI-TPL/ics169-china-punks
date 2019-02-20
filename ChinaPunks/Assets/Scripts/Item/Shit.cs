using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shit : GameItem {

	// Use this for initialization
	void Start () {
		description = "This is a strange poo. It looks like it has no effect...";
		consumable = true;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public override void change_character(Unit unit) { }
}
