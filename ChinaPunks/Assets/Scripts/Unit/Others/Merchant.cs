using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Merchant : Unit {

	// Boolean for checking whether the player can trade with the merchant
	private bool can_trade;
	public AudioSource _AudioSource;

	// Reference to the adjacent tiles in the map 
	private List<int> adjacent_tiles;

	// Reference to the trade button
	public GameObject trade_button;
	public GameObject shop_panel;

	// Merchant item list
	private List<GameItem> game_items;

	void Start()
	{
		Team.change_money(100);
		can_trade = false;
		game_items = new List<GameItem>();
        
		adjacent_tiles = mc.expansion_of_tiles[currentPos];

		mc.units_state[currentPos] = gameObject;
        mapInfo = mc.map_tiles;                                                          //get map info from GameController
        Vector3 xyPosition = mapInfo[currentPos].transform.position;
        transform.position = new Vector3(xyPosition.x, xyPosition.y + 0.5f, xyPosition.z - 1.0f);      //initialize my current position on map

		// Shop preparation, including generating items for selling
		set_up_shop();
		shop_close();

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

		// Update player team money
		shop_panel.transform.Find("Gold").GetChild(0).GetComponent<Text>().text = Team.get_money().ToString();
	}
    
    // Check if there is a PlayerUnit in the adjacent 4 tiles
	bool check_near_player(){

		foreach (int i in adjacent_tiles)
			if (mc.units_state[i] != null && mc.units_state[i].CompareTag("PlayerUnit"))
				return true;
		return false;
	}

    // Function used to set shop panel visible
	void shop_open(){
		shop_panel.SetActive(true);
		_AudioSource.Play();
	}

    // Function used to set shop panel invisible
	void shop_close(){
		shop_panel.SetActive(false);
	}

    // Function used to generate shop items and set up the shop at the beginning of each level
	void set_up_shop(){
		for (int i = 0; i < 3; i++){
			game_items.Add(new Shit());
			Sprite image = Resources.Load<Sprite>("Items/" + game_items[i].item_name);
			shop_panel.transform.Find("Item" + (i + 1).ToString()).GetComponent<Image>().sprite = image;

			// Bind generated buttons to the call back function
			int index = i;
            trade_button.GetComponent<Button>().onClick.AddListener(shop_open);
            shop_panel.transform.Find("Back").GetComponent<Button>().onClick.AddListener(shop_close);
            shop_panel.transform.Find("Gold").GetChild(0).GetComponent<Text>().text = Team.get_money().ToString();
			shop_panel.transform.Find("Price" + (i + 1).ToString()).GetChild(0).GetComponent<Text>().text = game_items[i].price.ToString();
			shop_panel.transform.Find("Message").GetComponent<Text>().text = "Welcome to the cheapest shop you can find in the world!";
			// Use delegate to add callback functions for buy buttons
			shop_panel.transform.Find("Buy" + (i + 1).ToString()).GetComponent<Button>().onClick.AddListener(() => { buy_items(index); });

		}
        
	}

    // Function for buy buttons
	void buy_items(int index){
		if(game_items[index] == null){
			shop_panel.transform.Find("Message").GetComponent<Text>().text = "Although I don't mind you donate to my shop, there is nothing for you to buy.";
			return;
		}
		if(Team.get_money() < game_items[index].price){
			shop_panel.transform.Find("Message").GetComponent<Text>().text = "You must be kidding! You cannot afford my precious treasure.";
			return;
		}
		else{
			//purchanse successful
			foreach (int i in adjacent_tiles)
				if (mc.units_state[i] != null && mc.units_state[i].CompareTag("PlayerUnit")){
					game_items[index].add_to_character(mc.units_state[i].GetComponent<UserUnit>());
					break;
				}
			Team.change_money(-game_items[index].price);
			game_items[index] = null;
			shop_panel.transform.Find("Item" + (index + 1).ToString()).GetComponent<Image>().sprite = null;
			shop_panel.transform.Find("Message").GetComponent<Text>().text = "You are very brilliant in choosing that best quality item. There will be more coming, with higher price of course, he he he.";
		}
	}
}
