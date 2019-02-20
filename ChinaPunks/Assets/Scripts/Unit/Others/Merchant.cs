using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Merchant : Unit {

	// Boolean for checking whether the player can trade with the merchant
	private bool can_trade;

	// Reference to the map 
	private List<int> adjacent_tiles;

	// Reference to the trade button
	public GameObject trade_button;
	public GameObject shop_panel;

	void Start()
	{
		can_trade = false;
        
		adjacent_tiles = mc.expansion_of_tiles[currentPos];

		mc.units_state[currentPos] = gameObject;
        mapInfo = mc.map_tiles;                                                          //get map info from GameController
        Vector3 xyPosition = mapInfo[currentPos].transform.position;
        transform.position = new Vector3(xyPosition.x, xyPosition.y + 0.5f, xyPosition.z - 1.0f);      //initialize my current position on map

		// Set shop panel invisible
		shop_close();

        // Bind generated buttons to the call back function
		trade_button.GetComponent<Button>().onClick.AddListener(shop_open);
		shop_panel.transform.Find("Back").GetComponent<Button>().onClick.AddListener(shop_close);
	}

	// Update is called once per frame
	void Update () {
		// set trade button
		can_trade = check_near_player();
		if (can_trade)
			trade_button.SetActive(true);
		else{
			trade_button.SetActive(false);
		}
			

	}
    
    // Check if there is a PlayerUnit in the adjacent 4 tiles
	bool check_near_player(){

		foreach (int i in adjacent_tiles)
			if (mc.units_state[i] != null && mc.units_state[i].CompareTag("PlayerUnit"))
				return true;
		return false;
	}

    // Function called by the trade button
	void shop_open(){
		shop_panel.SetActive(true);
	}

    // Function called by the shop back button
	void shop_close(){
		shop_panel.SetActive(false);
	}
}
