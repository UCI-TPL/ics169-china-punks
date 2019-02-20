using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameItem : MonoBehaviour {

	public string description;
	protected bool consumable;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    // Function called when the player acquire the item
	public void add_to_character(UserUnit userUnit){
		if(consumable){
			userUnit.add_Inventory(gameObject);
		}
		else{
			change_character(userUnit);
		}
	}

	/* Function called when the item is used. Equipments will be used when the player acquire them, consumables will be used when the player use them
     * Input is the character that use them
     */
	public virtual void change_character(Unit unit){}
}
