using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shit : GameItem {

	public Shit(){
		item_name = "shit";
		description = "This is a strange poo. It looks like it has no effect...";
		price = 50;
        consumable = true;
		use_range = new List<int>() { 1, -1, map_size, -map_size };
	}

	public override bool change_character(Unit unit) {
		if(!unit.CompareTag("EnemyUnit")){
			return false;
		}
		unit.Health_Change(1);
		return true;
	}
}
