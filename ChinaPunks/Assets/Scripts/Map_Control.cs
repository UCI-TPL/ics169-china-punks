using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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
    
    public Dictionary<GameObject, int> map_tiles_pos = new Dictionary<GameObject,int>();
    public Dictionary<int, List<int>> expansion_of_tiles = new Dictionary<int, List<int>>();
    public List<int> occupied_tiles = new List<int>();
    public List<int> expanded_tiles = new List<int>();

    public Dictionary<int, List<int>> all_paths = new Dictionary<int, List<int>>();

    public bool first_click = true;

    public int picked_pos;
    public int current_picked_pos = -1;

    public bool playerHUD_showed = false;

    //bool ShowedClickedEffect;

    //0 for original state, 1 for movement, 2 for attack, 3...
    public int acting_state = 0;

	//Variable used to keep track whose round the current one is.
    //Value: "Player", "AI"

    private void Awake()
    {
        for (int i = 0; i < map_size * map_size; i++){
            units_state.Add(null);
        }
    }
    // Use this for initialization
    void Start(){
        Tile_Store();

    }

    // Update is called once per frame
    void Update(){

        //click right botton to reset
        if (Input.GetKeyDown("r")){
            Debug.Log("r click!!!");
            reset();
        }

    }



	//Function used for unselecting character
    public void reset(){
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
        if (pickTile)
        {
            foreach (int i in expansion_of_tiles[map_tiles_pos[pickTile]])
            {
                map_tiles[i].GetComponent<SpriteRenderer>().color = new Color(255, 255, 255);
            }
        }
        expanded_tiles.Clear();
        first_click = true;
        tile_picked = false;
        pickTile = null;
        pickEndTile = null;

        playerHUD_showed = false;
    }

    public void Character_Move()
    {
        acting_state = 1;
        units_state[picked_pos].GetComponent<UserUnit>().isClicked = true;
        int move_range = units_state[picked_pos].GetComponent<UserUnit>().moveRange;

        pickEndTile = pickTile;

        Search_accessible_tiles(picked_pos, move_range);
        foreach (int i in expanded_tiles)
        {
            map_tiles[i].GetComponent<SpriteRenderer>().color = new Color(0, 200, 0);
        }
        tile_picked = false;
        first_click = false;
        playerHUD_showed = false;
       

    }

    public void Character_Attack()
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

        playerHUD_showed = false;
    }


    public void Character_Click(){
        if (tile_picked)
        {
            if (first_click)
            {
                picked_pos = map_tiles_pos[pickTile];

                if(current_picked_pos == -1)
                {
                    current_picked_pos = picked_pos;
                }
                current_picked_pos = picked_pos;

                 
                

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
                    //move state
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
                    //attack state
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




}
