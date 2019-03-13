using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Merchant : Unit {

    public AudioSource _AudioSource;

	// Reference to the trade button
	public GameObject trade_button;
	public GameObject shop_panel;

    // Reference to all player characters
    public List<GameObject> players;

	// Merchant PowerUp list
	private List<PowerUp> power_ups;
    // Potential PowerUps
    private List<PowerUp> prepared_powerUps = new List<PowerUp>() { new HealthPowerUp(), new AttDmgPowerUp(), new ViewPowerUp(), new MovRangePowerUp() };


    void Start()
	{
	    power_ups = new List<PowerUp>();
        
		//adjacent_tiles = mc.expansion_of_tiles[currentPos];

		//mc.units_state[currentPos] = gameObject;
        //mapInfo = mc.map_tiles;                                                          //get map info from GameController
        //Vector3 xyPosition = mapInfo[currentPos].transform.position;
        //transform.position = new Vector3(xyPosition.x, xyPosition.y + 0.5f, xyPosition.z - 1.0f);      //initialize my current position on map

		// Shop preparation, including generating items for selling
		set_up_shop();

	}

	// Update is called once per frame
	void Update () {

		// Update player team money
		shop_panel.transform.Find("Gold").GetChild(0).GetComponent<Text>().text = Team.get_money().ToString();
	}

    // Function used to set shop panel visible
	public void shop_open(){
		shop_panel.SetActive(true);
		_AudioSource.Play();
	}

    // Function used to set shop panel invisible
	void shop_close(){
		shop_panel.SetActive(false);
        trade_button.SetActive(false);
        Destroy(gameObject);
	}

    // Function used to generate shop items and set up the shop at the beginning of each level
	void set_up_shop(){
        //power_ups.Add();
        int skip_index = Random.Range(0,4);

        for (int i = 0; i < 4; i++) {
            if(i != skip_index) {
                power_ups.Add(prepared_powerUps[i]);
            }
        }

        for (int i = 0; i < 3; i++){
			Sprite image = Resources.Load<Sprite>("PowerUp/" + power_ups[i].attribute);
			shop_panel.transform.Find("Item" + (i + 1).ToString()).GetComponent<Image>().sprite = image;

			// Bind generated buttons to the call back function
			int index = i;
            trade_button.GetComponent<Button>().onClick.AddListener(shop_open);
            shop_panel.transform.Find("Back").GetComponent<Button>().onClick.AddListener(shop_close);
            shop_panel.transform.Find("Gold").GetChild(0).GetComponent<Text>().text = Team.get_money().ToString();
			shop_panel.transform.Find("Price" + (i + 1).ToString()).GetChild(0).GetComponent<Text>().text = power_ups[i].price.ToString();
			shop_panel.transform.Find("Message").GetComponent<Text>().text = "Welcome to the cheapest shop you can find in the world!";
			// Use delegate to add callback functions for buy buttons
			shop_panel.transform.Find("Buy" + (i + 1).ToString()).GetComponent<Button>().onClick.AddListener(() => { buy_items(index); });

		}
        
	}

    // Function for buy buttons
	void buy_items(int index){
		if(power_ups[index] == null){
			shop_panel.transform.Find("Message").GetComponent<Text>().text = "Although I don't mind you donate to my shop, there is nothing for you to buy.";
			return;
		}
		if(Team.get_money() < power_ups[index].price){
			shop_panel.transform.Find("Message").GetComponent<Text>().text = "You must be kidding! You cannot afford my precious treasure.";
			return;
		}
		else{
			//purchanse successful

            foreach(GameObject player in players) {
                if(player != null)
                {
                    power_ups[index].powerUp(player.GetComponent<UserUnit>());
                }
            }
            Team.change_money(-power_ups[index].price);
			power_ups[index] = null;
			shop_panel.transform.Find("Item" + (index + 1).ToString()).gameObject.SetActive(false);
			shop_panel.transform.Find("Message").GetComponent<Text>().text = "You are very brilliant in choosing that best quality item. There will be more coming, with higher price of course, he he he.";
		}
	}
}
