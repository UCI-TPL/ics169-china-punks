using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Select_Effect : MonoBehaviour {

	public float period_time;
  
    
	// Use this for initialization
	void Start () {
		period_time = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		
		Vector3 new_position = transform.position;
		new_position.y = new_position.y + Mathf.Sin(Time.time * 10f) / 100f;
		transform.position = new_position;
		//this.gameObject.transform.Translate(new Vector3(0, Mathf.Sin(Time.time - period_time), 0));

		
	}
}
