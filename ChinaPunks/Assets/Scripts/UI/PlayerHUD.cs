using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour {

    public GameObject HUDpanel;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 HUDpos = Camera.main.WorldToScreenPoint(this.transform.position);
        HUDpanel.transform.position = HUDpos;
    }
}
