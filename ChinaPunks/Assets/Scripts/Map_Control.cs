using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Map_Control : MonoBehaviour
{
    public List<GameObject> map_tiles = new List<GameObject>();
    public List<GameObject> units_state = new List<GameObject>();
    public List<int> path = new List<int>();

    public GameObject pickTile;
    public GameObject pickEndTile;

    public bool tile_picked;

    public int pickTile_pos;
    public int map_size;
    
    private Dictionary<GameObject, int> map_tiles_pos = new Dictionary<GameObject,int>();
    private Dictionary<int, List<int>> expansion_of_tiles = new Dictionary<int, List<int>>();
    private List<int> occupied_tiles = new List<int>();
    private List<int> expanded_tiles = new List<int>();

    private Dictionary<int, List<int>> all_paths = new Dictionary<int, List<int>>();

    private bool first_click = true;

    public GameObject selectEffect;
    GameObject _selectEffect;
    public GameObject InGameHUD;
    GameObject _InGameHUD;

    //HUD buttons
    public Button AttackBtn;
    public Button Movebtn;
    public Button Resetbtn;

    public Button AttackBtn2;
    public Button Movebtn2;
    public Button Resetbtn2;

    public int picked_pos;
    int current_picked_pos = -1;
    bool ShowedClickedEffect;

    //0 for original state, 1 for movement, 2 for attack, 3...
    private int acting_state = 0;

	//Variable used to keep track whose round the current one is.
    //Value: "Player", "AI"
	public string gameRound;
	public GameObject endTurnButton;

    private void Awake()
    {
		gameRound = "Player";
        for (int i = 0; i < map_size * map_size; i++){
            units_state.Add(null);
        }
    }
    // Use this for initialization
    void Start(){
        Tile_Store();
        Button Move_btn = Movebtn.GetComponent<Button>();
        Move_btn.onClick.AddListener(Character_Move);

        Button Attack_btn = AttackBtn.GetComponent<Button>();
        Attack_btn.onClick.AddListener(() => Character_Attack());

        Button Reset_btn = Resetbtn.GetComponent<Button>();
        Reset_btn.onClick.AddListener(() => reset());

        Button Move_btn2 = Movebtn2.GetComponent<Button>();
        Move_btn2.onClick.AddListener(Character_Move);

        Button Attack_btn2 = AttackBtn2.GetComponent<Button>();
        Attack_btn2.onClick.AddListener(() => Character_Attack());

        Button Reset_btn2 = Resetbtn2.GetComponent<Button>();
        Reset_btn2.onClick.AddListener(() => reset());

        ShowedClickedEffect = false;
    }

    // Update is called once per frame
    void Update(){
        Character_Click();
        //click right botton to reset
        if (Input.GetKeyDown("r"))
        {
            Debug.Log("r click!!!");
            reset();
        }

        //Execute AI round
		if(gameRound == "AI"){
			Debug.Log("AI round");
			changeRound();
		}
    }

	IEnumerator pauseSimulator()
    {
        yield return new WaitForSeconds(5);
    }

	//Function used for unselecting character
    void reset(){
        acting_state = 0;
        //expanded tile empty?
        if (expanded_tiles.Count != 0)
        {
            //recover tile color
            foreach (int i in expanded_tiles)
            {
                map_tiles[i].GetComponent<SpriteRenderer>().color = new Color(255, 255, 255);
            }
        }
        //recover tile colors
        foreach (int i in expansion_of_tiles[map_tiles_pos[pickTile]])
        {
            map_tiles[i].GetComponent<SpriteRenderer>().color = new Color(255, 255, 255);
        }
        expanded_tiles.Clear();
        first_click = true;
        tile_picked = false;
        pickTile = null;
        pickEndTile = null;
    }

    void Character_Move()
    {
        acting_state = 1;
        units_state[picked_pos].GetComponent<UserUnit>().isClicked = true;
        int move_range = units_state[picked_pos].GetComponent<UserUnit>().moveRange;
        //First click to show all available tiles
        if (!expanded_tiles.Contains(picked_pos))
        {
            pickEndTile = pickTile;

            Search_accessible_tiles(picked_pos, move_range);
            foreach (int i in expanded_tiles)
            {
                map_tiles[i].GetComponent<SpriteRenderer>().color = new Color(0, 200, 0);
            }
            tile_picked = false;
            first_click = false;
        }
    }

    void Character_Attack()
    {
        acting_state = 2;
        pickEndTile = pickTile;

        List<int> attackRange = units_state[picked_pos].GetComponent<UserUnit>().attackRange;
        int debug = 0; // for debug
        foreach (int position in attackRange)
        {
            if (map_tiles_pos[pickTile] + position >= 0 && map_tiles_pos[pickTile] + position <= map_size * map_size - 1)
            {
                if (units_state[map_tiles_pos[pickTile] + position] != null && units_state[map_tiles_pos[pickTile] + position].gameObject.tag != "PlayerUnit")
                {
                    debug++;
                    map_tiles[map_tiles_pos[pickTile] + position].GetComponent<SpriteRenderer>().color = new Color(200, 0, 0);
                }
            }
        }

        tile_picked = false;
        first_click = false;

        if (debug == 0)
        {
            Debug.Log("No target for attacking!");
            first_click = true;
        }
    }

    void Character_Click(){
        if (tile_picked)
        {
            if (first_click)
            {
                picked_pos = map_tiles_pos[pickTile];

                if(current_picked_pos == -1)
                {
                    current_picked_pos = picked_pos;
                }
                else if (picked_pos != current_picked_pos && ShowedClickedEffect)
                {
                    //Disable effect if it existed
                    Destroy(_selectEffect);
                    units_state[current_picked_pos].gameObject.GetComponent<Transform>().GetChild(0).GetComponent<PlayerHUD>().HUDpanel.SetActive(false);
                    ShowedClickedEffect = false;
                }
                current_picked_pos = picked_pos;

                if (units_state[picked_pos] != null && units_state[picked_pos].gameObject.tag == "PlayerUnit" && !ShowedClickedEffect)
                {
                    //Show effect
                    _selectEffect = Instantiate(selectEffect, units_state[picked_pos].gameObject.GetComponent<Transform>());
                    _selectEffect.transform.Translate(new Vector3(0, -0.8f, 0));
                    ShowedClickedEffect = true;

                    //Show player HUD
                    InGameHUD = units_state[picked_pos].gameObject.GetComponent<Transform>().GetChild(0).GetComponent<PlayerHUD>().HUDpanel;
                    _InGameHUD = InGameHUD;
                    _InGameHUD.SetActive(true);
                }
            }
            //Second click to choose the end point of the path
            else
            {
                GameObject temp;
                temp = pickTile; //pickTile here is the end tile
                pickTile = pickEndTile; //pickEndTile is the start tile from first click
                pickEndTile = temp; //put temp(real end tile) into pickEndTile

                // For Debug
                //string ap = "[";
                //foreach (int i in all_paths.Keys)
                //{
                //    ap += i.ToString() + ",";
                //}
                //ap = ap.Remove(ap.Length - 1);
                //ap += "]";
                //Debug.Log(ap);
                //Debug.Log(map_tiles_pos[pickEndTile]);
                if (acting_state == 1)
                {
                    if (all_paths.ContainsKey(map_tiles_pos[pickEndTile]))
                    {
                        path = all_paths[map_tiles_pos[pickEndTile]];
                        path.Insert(0, map_tiles_pos[pickEndTile]);
                        path.Reverse();

                        // For Debug
                        //string result = "Path Found:[";
                        //foreach (int i in path)
                        //{
                        //    result += i.ToString() + ",";
                        //}
                        //result = result.Remove(result.Length-1);
                        //result += "]";
                        //Debug.Log(result);

                        foreach (int i in expanded_tiles)
                        {
                            map_tiles[i].GetComponent<SpriteRenderer>().color = new Color(255, 255, 255);
                        }
                        expanded_tiles.Clear();
                        all_paths.Clear();
                        first_click = true;
                    }
                }
                else if(acting_state == 2){
                    //check second-clicked tile has unit
                    if(units_state[map_tiles_pos[pickEndTile]] != null && units_state[map_tiles_pos[pickEndTile]].gameObject.tag == "EnemyUnit")
                    {
                        int attack_damage = units_state[map_tiles_pos[pickTile]].GetComponent<Unit>().attack_damge;
                        units_state[map_tiles_pos[pickEndTile]].GetComponent<Unit>().Health_Change(attack_damage);
                        Debug.Log(units_state[map_tiles_pos[pickTile]].gameObject.name + " attacked "
                                  + units_state[map_tiles_pos[pickEndTile]].gameObject.name);

                    }
                    //recover tiles color
                    foreach(int i in expansion_of_tiles[map_tiles_pos[pickTile]]){
                        map_tiles[i].GetComponent<SpriteRenderer>().color = new Color(255, 255, 255);
                    }
                    reset();
                }

                tile_picked = false;
                Destroy(_selectEffect);
                ShowedClickedEffect = false;
                _InGameHUD.SetActive(false);
            }
        }

    }

    void Tile_Store(){
        //store each tile and its position in a dict
        for (int i = 0; i < map_size * map_size; ++i)
        {
            map_tiles_pos.Add(map_tiles[i], i);

            //store expansion of each tile in a dict
            //not left boundary
            List<int> temp_tiles = new List<int>();
            if (i % map_size != 0)
            {
                temp_tiles.Add(i - 1);
            }
            //not right boundary
            if ((i + 1) % map_size != 0)
            {
                temp_tiles.Add(i + 1);
            }
            //not top boundary
            if ((i + map_size) <= (map_size * map_size - 1))
            {
                temp_tiles.Add(i + map_size);
            }
            //not bottom boundary
            if ((i - map_size) >= 0)
            {
                temp_tiles.Add(i - map_size);
            }
            expansion_of_tiles[i] = new List<int>(temp_tiles);
        }
    }

	//search for all accessible tiles after the player pick one
	void Search_accessible_tiles(int picked_pos, int range)
	{
		//search algorithm
		List<int> temp_tiles_to_explore = new List<int>() { picked_pos };
		expanded_tiles.Add(picked_pos);
		all_paths[picked_pos] = new List<int>();

		for (int i = 0; i < range; ++i)
		{
			if (occupied_tiles.Contains(pickTile_pos))
			{
				//there is a selectible unit on the picked tile
			}
			//store the expanded tiles of the currently picked tile
			foreach (int pos_to_explore in temp_tiles_to_explore)
			{
				foreach (int pos in expansion_of_tiles[pos_to_explore])
				{
					//add the explored tile to the list
					//check if pos already expanded in previous loop
					if (!expanded_tiles.Contains(pos) && units_state[pos] == null)
					{
						expanded_tiles.Add(pos);
						//add the explored tile to the path
						if (!all_paths.ContainsKey(pos))
						{
							all_paths[pos] = new List<int>() { pos_to_explore };

							foreach (int p in all_paths[pos_to_explore])
							{
								all_paths[pos].Add(p);
							}
						}
                        
					}
				}
			}
			//update tiles_to_explore to the list of newly expaneded tiles
			List<int> temp = new List<int>();
			foreach (int tile_pos in expanded_tiles)
			{
				if (!temp_tiles_to_explore.Contains(tile_pos))
				{
					temp.Add(tile_pos);
				}
			}

			temp_tiles_to_explore = new List<int>(temp);


		}

	}

	//Change round between player and AI, AI units from different group all act in "AI" round
    public void changeRound()
    {
		if (gameRound == "Player"){
			gameRound = "AI";
            endTurnButton.SetActive(false);
		}

		else{
			gameRound = "Player";
            endTurnButton.SetActive(true);
		}    
    }


}
