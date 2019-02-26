using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameItem{

    // Item name must be exactly same as the sprite name
	public string item_name;
	public string description;
	public int price;

    // Boolean used for distingush whether this item is a consumable, it can only be accessed inside the GameItem class
	protected bool consumable;
	protected List<int> use_range;
	protected int map_size = GameObject.Find("WorldGenerator").GetComponent<WorldGenerator>().map_ctr.map_size;

    // Function called when the player character acquire the item
	public void add_to_character(UserUnit userUnit){
		if(consumable){
			userUnit.add_Inventory(this);
		}
		else{
			change_character(userUnit);
		}
	}

	/* Function called when the item is used. Equipments will be used when the player acquire them, consumables will be used when the player use them
     * Input is the character that the item has effect on
     * The return boolean specify whether using the item is successful
     */
	public virtual bool change_character(Unit unit) { return false; }
}
