using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuWind : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.Translate(Vector2.left * Time.deltaTime);
        if(transform.position.x < -20)
        {
            transform.position = new Vector2(20f, transform.position.y);
        }
    }
}
