using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelection : MonoBehaviour {

	private GameObject base_manager;

    // Boolean specify whether the character is in the team
	private bool in_team;

    // Cache the start point of the character
	private Vector2 start_position;

	private void Awake()
	{
		base_manager = GameObject.Find("BaseManager");
		in_team = false;
		start_position = transform.position;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    // Change characters size when mouse hover on them
	private void OnMouseEnter()
	{
		gameObject.transform.localScale = gameObject.transform.localScale * 1.2f;
	}

	private void OnMouseExit()
	{
		gameObject.transform.localScale = gameObject.transform.localScale / 1.2f;
	}


	private void OnMouseDown()
	{
		// If character not in team and the team still have empty spot, move the character to the empty spot
		if(!in_team){
			Vector2 destination = base_manager.GetComponent<BaseManager>().AddMember(gameObject.name);
			Vector2 distance = destination - start_position;
			if (!destination.Equals(Vector2.zero))
            {
                transform.Translate(distance);
				in_team = true;
            }
		}
		else{
			Vector2 distance = start_position - (Vector2)gameObject.transform.position;
			transform.Translate(distance);
			in_team = false;
			base_manager.GetComponent<BaseManager>().RemoveMember(gameObject.name);
		}


	}
}
