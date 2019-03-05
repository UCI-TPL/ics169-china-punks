using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Map_Control : MonoBehaviour
{
    public List<GameObject> map_tiles = new List<GameObject>();
    public List<GameObject> units_state = new List<GameObject>();
    public List<GameObject> blocks_state = new List<GameObject>();
    public List<int> path = new List<int>();

    public GameObject pickTile;
    public GameObject pickEndTile;

    public bool tile_picked;

    public int pickTile_pos = -1;
    public int map_size;

    public Dictionary<GameObject, int> map_tiles_pos = new Dictionary<GameObject, int>();
    public Dictionary<int, List<int>> expansion_of_tiles = new Dictionary<int, List<int>>();
    public List<int> occupied_tiles = new List<int>();
    public List<int> expanded_tiles = new List<int>();

    public Dictionary<int, List<int>> all_paths = new Dictionary<int, List<int>>();

    public bool first_click = true;

    public int picked_pos;
    public int current_picked_pos = -1;

    public bool playerHUD_showed = false;

    public GameObject InGameUI;
    private InGameUI InGameUI_prefab;

    public int peach_pos;

    public bool provocative;

    public bool animation_is_playing;

    public GameObject Arrow_Effect_Prefab;
    public GameObject Explosion_Effect_Prefab;

    //bool ShowedClickedEffect;

    //0 for original state, 1 for movement, 2 for attack, 3 for PickUp...
    public int acting_state = 0;

    //Variables used for passing functions to search algorithm
    //Condition function input:
    //  pos: the tile that is being checked
    //  unitTag: the tag the function is compared with
    public delegate bool Condition(int pos, string unitTag);
    public Condition solutionCondition;

    //Variable that stores the reference of every AIUnit gameObject
    public List<GameObject> AI_units = new List<GameObject>();

    public GameObject PeachPrefab;

    public int turn_count;

    //tiles for showing skill range
    public Dictionary<int, List<int>> skill_tiles = new Dictionary<int, List<int>>();
    public List<int> attackRange = new List<int>();

    public bool character_moving = false;

	// Dictionary used for tracking who has visibility on each tile;
	public Dictionary<int, HashSet<string>> tile_visibility = new Dictionary<int, HashSet<string>>();

    // List used for checking whether a tile should be handled by visibility update
	public List<bool> mark_tile = new List<bool>();

    // Map Audio
    public AudioSource Audio;
    public AudioClip Grass_sound;

    public AudioSource Audio_Skill;
    public AudioClip Arrow_sound;
    public AudioClip Explosion_sound;
    public AudioClip Punch_sound;

    private void Awake()
    {
        //for (int i = 0; i < map_size * map_size; i++)
        //{
        //    units_state.Add(null);
        //}
    }
    // Use this for initialization
    void Start()
    {
        turn_count = 1;
        Tile_Store();

        InGameUI_prefab = InGameUI.GetComponent<InGameUI>();

        //Start with each tile has no character who has visual on them. Initial visibility set by player characters Start()
        for (int i = 0; i < map_tiles_pos.Count; i++){
			tile_visibility.Add(i, new HashSet<string>());
			mark_tile.Add(false);
		}      
    }

    // Update is called once per frame
    void Update()
    {
		change_visibility();

        if (Input.GetMouseButtonDown(1))
        {
            reset();
        }

        if (!character_moving)
        {
            Audio.Pause();
        }
        else if(character_moving)
        {
            Audio.UnPause();
        }
    }


    //Function used for unselecting character
    public void reset()
    {
        acting_state = 0;


        //detroy selectEffect if picked_pos has playerUnit, and deselect the character
        if (picked_pos != -1
           && units_state[picked_pos] != null
            && units_state[picked_pos].CompareTag("PlayerUnit"))
        {
            units_state[picked_pos].GetComponent<UserUnit>().destory_clickEffect();
//            Selected_Char_info_HUD.SetActive(false);
            units_state[picked_pos].GetComponent<UserUnit>().isClicked = false;
        }


        //expanded tile empty?
        if (expanded_tiles.Count != 0)
        {
            //recover tile color from moving state
            foreach (int i in expanded_tiles)
            {
                map_tiles[i].GetComponent<SpriteRenderer>().color = new Color(255, 255, 255);
				mark_tile[i] = false;
            }
        }
        //recover tile colors from attack state and pick state
        if (picked_pos != -1)
        {
            foreach (int i in expansion_of_tiles[picked_pos])
            {
                map_tiles[i].GetComponent<SpriteRenderer>().color = new Color(255, 255, 255);
				mark_tile[i] = false;
            }

            //Archer
            if(units_state[picked_pos] != null && units_state[picked_pos].name.StartsWith("Archer")){
                foreach (int i in units_state[picked_pos].GetComponent<Archer>().Attack_range())
                {
                    map_tiles[i].GetComponent<SpriteRenderer>().color = new Color(255, 255, 255);
					mark_tile[i] = false;
                }
            }

        }
        //recover attack range tile colors;
        foreach(int pos in attackRange){
            map_tiles[pos].GetComponent<SpriteRenderer>().color = new Color(255, 255, 255);
			mark_tile[pos] = false;
        }
        attackRange.Clear();


        //recover skill tiles
        if(skill_tiles.Count!=0){
            foreach(KeyValuePair<int,List<int>> pair in skill_tiles){
                foreach(int pos in pair.Value){
                    map_tiles[pos].GetComponent<SpriteRenderer>().color = new Color(255, 255, 255);
					mark_tile[pos] = false;
                }
            }

            
        }

        expanded_tiles.Clear();
        skill_tiles.Clear();
        first_click = true;
        tile_picked = false;
        pickTile = null;
        pickEndTile = null;

        playerHUD_showed = false;
    }

    void reset_color(){
        if (units_state[picked_pos] != null && units_state[picked_pos].name.StartsWith("Archer"))
        {
            foreach (int i in units_state[picked_pos].GetComponent<Archer>().Attack_range())
            {
                map_tiles[i].GetComponent<SpriteRenderer>().color = new Color(255, 255, 255);
				mark_tile[i] = false;
            }
        }

        if (expanded_tiles.Count != 0)
        {
            foreach (int i in expanded_tiles)
            {
                map_tiles[i].GetComponent<SpriteRenderer>().color = new Color(255, 255, 255);
				mark_tile[i] = false;
            }
        }

        //recover tile colors from attack state and pick state
        if (picked_pos != -1)
        {
            foreach (int i in expansion_of_tiles[picked_pos])
            {
                map_tiles[i].GetComponent<SpriteRenderer>().color = new Color(255, 255, 255);
				mark_tile[i] = false;
            }

            //Archer
            if (units_state[picked_pos] != null && units_state[picked_pos].name.StartsWith("Archer"))
            {
                foreach (int i in units_state[picked_pos].GetComponent<Archer>().Attack_range())
                {
                    map_tiles[i].GetComponent<SpriteRenderer>().color = new Color(255, 255, 255);
					mark_tile[i] = false;
                }
            }

        }
    }

    public void Character_PickUp()
    {
        // reset color
        reset_color();

        acting_state = 3;

        List<int> pickRange = units_state[picked_pos].GetComponent<UserUnit>().pickRange;

        foreach (int position in pickRange)
        {
            if (picked_pos + position >= 0 && picked_pos + position <= map_size * map_size - 1)
            {
				mark_tile[picked_pos + position] = true;
				map_tiles[picked_pos + position].GetComponent<SpriteRenderer>().color = new Color(0, 0, 250);
                if (picked_pos % 10 == 0 && (picked_pos + position + 1) % 10 == 0)
                {
                    map_tiles[picked_pos + position].GetComponent<SpriteRenderer>().color = new Color(255, 255, 255);
					mark_tile[picked_pos + position] = false;
                }
                else if ((picked_pos + 1) % 10 == 0 && (picked_pos + position) % 10 == 0)
                {
                    map_tiles[picked_pos + position].GetComponent<SpriteRenderer>().color = new Color(255, 255, 255);
					mark_tile[picked_pos + position] = false;
                }
            }
        }

        tile_picked = false;
        first_click = false;
    }

    public void Character_Move()
    {
        acting_state = 1;
        int move_range;
        units_state[picked_pos].GetComponent<UserUnit>().isClicked = true;
        //check if character is on Muddy tile
        if (map_tiles[picked_pos].GetComponent<Tile>().tile_type == "Muddy")
            move_range = 1;
        else 
            move_range = units_state[picked_pos].GetComponent<UserUnit>().moveRange;

        //pickEndTile = pickTile;

        all_paths = Search_solution(picked_pos, move_range, "Player", null, null);
        foreach (int i in expanded_tiles)
        {
			mark_tile[i] = true;
			map_tiles[i].GetComponent<SpriteRenderer>().color = new Color(0, 255, 114);
        }
        tile_picked = false;
        first_click = false;
        //playerHUD_showed = true;
    }

    public void Character_Attack()
    {

        Debug.Log("atk!!!!!!!!");
        // reset color
        reset_color();

        acting_state = 2;

        //check if it's archer's round
        if (units_state[picked_pos].name.StartsWith("Archer"))
            attackRange = units_state[picked_pos].GetComponent<Archer>().Attack_range();
        else
        {
            Debug.Log("atkRage!!!!!!!!!!!!!!!");
            Debug.Log(expansion_of_tiles.Count);
            attackRange = new List<int>( expansion_of_tiles[picked_pos]);
            foreach (int pos in attackRange)
                Debug.Log(pos);
        }

        foreach (int position in attackRange)
        {
			mark_tile[position] = true;
            map_tiles[position].GetComponent<SpriteRenderer>().color = new Color(200, 0, 0);
        }

        tile_picked = false;
        first_click = false;
    }

    public void Character_Skill()
    {
        // reset color
        reset_color();

        //clear skill_tiles, just in case
        skill_tiles.Clear();

        acting_state = 4;

        if (units_state[picked_pos].gameObject.name.StartsWith("Monk"))
        {
            units_state[picked_pos].GetComponent<Animator>().Play("Monk_skill");
        }

        //skill
        units_state[picked_pos].GetComponent<UserUnit>().Skill();
       

    }


    public void Character_Click()
    {
        if (tile_picked)
        {
            if (first_click)
            {
                picked_pos = -1;                      // set it to default value
                picked_pos = map_tiles_pos[pickTile];

                pickEndTile = pickTile;

                if (current_picked_pos == -1)
                {
                    current_picked_pos = picked_pos;
                }
                current_picked_pos = picked_pos;

                if (units_state[picked_pos] != null
                    && units_state[map_tiles_pos[pickEndTile]].tag == "PlayerUnit")
                {
                    //if this tile has playerUnit, show its clickEffect
                    units_state[picked_pos].GetComponent<UserUnit>().show_clickEffect();

                    //lock the Mouse Over info to character

                    first_click = false;
                    acting_state = 1;
                    //playerHUD_showed = true;

                    //if this playerUnit can still move, then call Character_Move()
                    if (!units_state[picked_pos].GetComponent<UserUnit>().moveComplete
                        && !units_state[picked_pos].GetComponent<UserUnit>().turnComplete)
                        Character_Move();
                }
            }
            //Second click to choose the end point of the path
            else
            {
                GameObject temp;
                temp = pickTile; //pickTile here is the end tile
                pickTile = pickEndTile; //pickEndTile is the start tile from first click
                pickEndTile = temp; //put temp(real end tile) into pickEndTile

                current_picked_pos = picked_pos;

                // if character has not moved and acting state is 1
                if (acting_state == 1)
                //move state
                {
                    //move character in the move range
                    if (!units_state[picked_pos].GetComponent<UserUnit>().moveComplete
                        && all_paths.ContainsKey(map_tiles_pos[pickEndTile]))
                    {
                        path = new List<int>(all_paths[map_tiles_pos[pickEndTile]]);
                        path.Add(map_tiles_pos[pickEndTile]);

                        units_state[picked_pos].GetComponent<UserUnit>().moveComplete = true;
                        if (units_state[picked_pos].GetComponent<UserUnit>().hasPeach)
                        {
                            peach_pos = map_tiles_pos[pickEndTile];
                        }

                        picked_pos = map_tiles_pos[pickEndTile];  // save changed pos

                        foreach (int i in expanded_tiles)
                        {
                            map_tiles[i].GetComponent<SpriteRenderer>().color = new Color(255, 255, 255);
                            mark_tile[i] = false;
                        }

                        //acting_state = 0;
                        //finised moving, clear all_path, expanded tiles
                        all_paths.Clear();
                        expanded_tiles.Clear();
                    }
                    // switch character
                    else if (units_state[map_tiles_pos[pickEndTile]] != null &&
                             units_state[map_tiles_pos[pickEndTile]].tag == "PlayerUnit")
                    {
                        //change character avatar
                        if (units_state[map_tiles_pos[pickEndTile]].name == "Monk(Clone)")
                        {
                            InGameUI_prefab.Move_Over_Avatar.sprite = InGameUI_prefab.Monk;
                        }
                        else if (units_state[map_tiles_pos[pickEndTile]].name == "Makepinggo(Clone)")
                        {
                            InGameUI_prefab.Move_Over_Avatar.sprite = InGameUI_prefab.Makepinggo;
                        }
                        else if (units_state[map_tiles_pos[pickEndTile]].name == "SwordMan(Clone)")
                        {
                            InGameUI_prefab.Move_Over_Avatar.sprite = InGameUI_prefab.SwordMan;
                        }
                        else if (units_state[map_tiles_pos[pickEndTile]].name == "Archer(Clone)")
                        {
                            InGameUI_prefab.Move_Over_Avatar.sprite = InGameUI_prefab.Archer;
                        }
                        else if (units_state[map_tiles_pos[pickEndTile]].name == "Tauren(Clone)")
                        {
                            InGameUI_prefab.Move_Over_Avatar.sprite = InGameUI_prefab.Tauren;
                        }
						else if (units_state[map_tiles_pos[pickEndTile]].name == "Wuchang(Clone)")
                        {
							InGameUI_prefab.Move_Over_Avatar.sprite = InGameUI_prefab.Wuchang;
                        }

                        //previous character is not being clicked now
                        units_state[picked_pos].GetComponent<UserUnit>().isClicked = false;

                        picked_pos = map_tiles_pos[pickEndTile];
                        current_picked_pos = picked_pos;

                        //detroy selectEffect of current character
                        if (units_state[map_tiles_pos[pickTile]] != null
                           && units_state[map_tiles_pos[pickTile]].tag == "PlayerUnit")
                            units_state[map_tiles_pos[pickTile]].GetComponent<UserUnit>().destory_clickEffect();

                        //create selectEffect of switched character
                        units_state[picked_pos].GetComponent<UserUnit>().show_clickEffect();

                        pickTile = pickEndTile;


                        //reset color and some other attributes
                        foreach (int i in expanded_tiles)
                        {
                            map_tiles[i].GetComponent<SpriteRenderer>().color = new Color(255, 255, 255);
                            mark_tile[i] = false;
                        }
                        all_paths.Clear();
                        expanded_tiles.Clear();

                        if (units_state[picked_pos] != null
                            && (!units_state[picked_pos].GetComponent<UserUnit>().moveComplete
                                && !units_state[picked_pos].GetComponent<UserUnit>().turnComplete))
                        {

                            Character_Move();
                        }

                    }
                    //click out of move range
                    else if (!all_paths.ContainsKey(map_tiles_pos[pickEndTile]))
                    {
                        reset();
                    }


                }
                //attack state
                else if (acting_state == 2)
                {
                    //check second-clicked tile has unit
                    if (units_state[map_tiles_pos[pickEndTile]] != null
                        & (units_state[map_tiles_pos[pickEndTile]].gameObject.tag == "EnemyUnit" ||
					       units_state[map_tiles_pos[pickEndTile]].gameObject.tag == "Block")
                        && attackRange.Contains(map_tiles_pos[pickEndTile]))
                    {
                        units_state[picked_pos].GetComponent<UserUnit>().anim.Play("Attack");
                        units_state[picked_pos].GetComponent<UserUnit>().AS.clip = units_state[picked_pos].GetComponent<UserUnit>().Attack_Clip;
                        units_state[picked_pos].GetComponent<UserUnit>().AS.Play();
                        float attack_damage = units_state[picked_pos].GetComponent<UserUnit>().attack_damage;
                        Debug.Log(units_state[picked_pos].gameObject.name + " attacked "
                                  + units_state[map_tiles_pos[pickEndTile]].gameObject.name);

                        // Player Unit finshed attack movement
                        units_state[picked_pos].GetComponent<UserUnit>().turnComplete = true;

                        if (picked_pos != -1)
                        {
                            foreach (int i in expansion_of_tiles[picked_pos])
                            {
                                map_tiles[i].GetComponent<SpriteRenderer>().color = new Color(255, 255, 255);
                                mark_tile[i] = false;
                            }
                        }
                        units_state[map_tiles_pos[pickEndTile]].GetComponent<Unit>().Health_Change(attack_damage);
                        reset();
                    }

                    else
                    {
                        Debug.Log("There is no target in attack range!");
                    }
                    //reset();
                }

                //picking state
                else if (acting_state == 3)
                {
                    if (units_state[map_tiles_pos[pickEndTile]] != null && units_state[map_tiles_pos[pickEndTile]].gameObject.tag == "Peach")
                    {
                        Debug.Log("Found Peach!");
                        //here write code to pick up peach
                        GameObject peach = units_state[map_tiles_pos[pickEndTile]].gameObject;
                        peach_pos = picked_pos;

                        units_state[picked_pos].GetComponent<UserUnit>().hasPeach = true;

                        units_state[map_tiles_pos[pickEndTile]] = null;
                        Destroy(peach);

                        //recover tile colors after attack
                        if (picked_pos != -1)
                        {
                            foreach (int i in expansion_of_tiles[picked_pos])
                            {
                                map_tiles[i].GetComponent<SpriteRenderer>().color = new Color(255, 255, 255);
                                mark_tile[i] = false;
                            }
                        }

                        // Player Unit finshed attack movement
                        units_state[picked_pos].GetComponent<UserUnit>().turnComplete = true;
                        reset();

                    }
                    else
                    {
                        Debug.Log("There is no peach in PickUp range!");
                    }
                    //reset();
                }
                //skill state
                else if (acting_state == 4)
                {
                    //second click is on the available tile
                    if (skill_tiles.ContainsKey(map_tiles_pos[pickEndTile]))
                    {
                        //get skill damage
                        float skill_damage = units_state[picked_pos].GetComponent<UserUnit>().skill_damage;
                        foreach (int pos in skill_tiles[map_tiles_pos[pickEndTile]])
                        {
                            //there is unit on this tile
                            if (units_state[pos] != null)
                            {
								//showing attack message
                                Debug.Log(units_state[picked_pos].gameObject.name + " attacked "
                                + units_state[pos].gameObject.name);
                                units_state[pos].GetComponent<Unit>().Health_Change(skill_damage);
                               
                                if (units_state[picked_pos].gameObject.name.StartsWith("Tauren"))
                                {
                                    if (units_state[pos].tag == "EnemyUnit")
                                    {
                                        Audio_Skill.PlayOneShot(Punch_sound);
										int destination = pos;
										int dashDesPos = picked_pos + (pos - picked_pos) * (units_state[picked_pos].gameObject.GetComponent<Tauren>().dash_range+1);  // here defined how many tiles enemy was dashed
										for (int i = picked_pos + (pos - picked_pos) * 2; (i != dashDesPos) && (i >= 0) && (i < map_size * map_size); i += pos - picked_pos)
                                        {
                                            if (units_state[i] == null)
                                            {
                                                destination = i;
                                            }
                                            else
                                            {
                                                break;
                                            }
                                        }

										dashDesPos = destination;


										if (dashDesPos >= 0
                                                && dashDesPos < map_size * map_size
                                                && units_state[dashDesPos] == null)
                                        {

                                            Debug.Log("dash");
                                            if (units_state[pos].GetComponent<AIUnit>().current_health > 0)
                                            {
                                                StartCoroutine(units_state[pos].GetComponent<AIUnit>().Dashed(dashDesPos, 5f));
                                            }
                                        }

                                    }
                                    
								}


                            }

                            if (units_state[picked_pos].gameObject.name.StartsWith("Archer"))
                            {
                                GameObject Arrow_Effect = Instantiate(Arrow_Effect_Prefab, map_tiles[pos].transform);
                                Audio_Skill.PlayOneShot(Arrow_sound, 0.5f);
                                Destroy(Arrow_Effect, 0.6f);
                                //map_tiles[pos].GetComponent<Animator>().Play("ArrowEffectOnTile");
                            }
                            else if (units_state[picked_pos].gameObject.name.StartsWith("Makepinggo"))
                            {
                                GameObject Explosion_Effect = Instantiate(Explosion_Effect_Prefab, map_tiles[pos].transform);
                                Audio_Skill.PlayOneShot(Explosion_sound, 0.2f);
                                Destroy(Explosion_Effect, 1f);
                                //map_tiles[pos].GetComponent<Animator>().Play("ExplosionEffect");
                            }

                        }
                        if (units_state[picked_pos].gameObject.name.StartsWith("Tauren"))
                        {
                            units_state[picked_pos].GetComponent<Animator>().Play("Tauren_skill");
                        }
                        else if (units_state[picked_pos].gameObject.name.StartsWith("Archer"))
                        {
                            units_state[picked_pos].GetComponent<Animator>().Play("Archer_skill");
                        }
                        else if (units_state[picked_pos].gameObject.name.StartsWith("Makepinggo"))
                        {
                            units_state[picked_pos].GetComponent<Animator>().Play("Makepinggo_skill");
                        }

                        //recover the tile colors after using skill
                        foreach (int k in skill_tiles.Keys)
                        {
                            foreach (int pos in skill_tiles[k])
                            {
                                map_tiles[pos].GetComponent<SpriteRenderer>().color = new Color(255, 255, 255);
                                mark_tile[pos] = false;
                            }
                        }
                        //apply skill cd
                        units_state[picked_pos].GetComponent<UserUnit>().coolDown +=
                            units_state[picked_pos].GetComponent<UserUnit>().skill_cd;
                        // Player turn ends after using skill
                        units_state[picked_pos].GetComponent<UserUnit>().turnComplete = true;
                        reset();
                    }
                }
                tile_picked = false;
                

            }

        }
    }

    void Tile_Store()
    {
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

    /* search for all tiles that are considered solution for the input Condition
     * Different user modes:
     *      "Player": used for finding the tiles that a player character can move to
     *      "AI": used for AI desicion making
     */
    public Dictionary<int, List<int>> Search_solution(int start_tile, int range, string userMode, string unitTag, Condition myCondition)
    {
        //search algorithm
        List<int> temp_tiles_to_explore = new List<int>() { start_tile };
        HashSet<int> searched_tiles = new HashSet<int>();
        Dictionary<int, List<int>> solution = new Dictionary<int, List<int>>();

        all_paths.Clear();
        all_paths[start_tile] = new List<int>();

        // For AI, if there is a non-AI Unit nearby(in attack range already), don't move(stay on this postion and attack).
        if (userMode == "AI")
        {
            if (myCondition(start_tile, unitTag))
            {
                solution[start_tile] = new List<int>() { start_tile };
                return solution;
            }
        }

        for (int i = 0; i < range; ++i)
        {
            //store the expanded tiles of the currently picked tile
            foreach (int pos_to_explore in temp_tiles_to_explore)
            {
                foreach (int pos in expansion_of_tiles[pos_to_explore])
                {
                    //check if the tile satisfied the condition
                    if (units_state[pos] == null)
                    {
                        //add the explored tile to the list
                        //check if pos already expanded in previous loop
                        if (userMode == "Player" && !expanded_tiles.Contains(pos))
                            expanded_tiles.Add(pos);

                        if (!all_paths.ContainsKey(pos))
                        {
                            all_paths[pos] = new List<int>(all_paths[pos_to_explore]);
                            all_paths[pos].Add(pos_to_explore);
                            if (userMode == "AI" && i == range - 1)
                                all_paths[pos].Add(pos);
                        }

                    }
                    if (userMode == "AI" && myCondition(pos, unitTag) && all_paths.ContainsKey(pos))            //For AI, there is an attackable Unit in move range.
                    {
                        solution[pos] = all_paths[pos];                           //save the path to this attackable Unit
                        solution[pos].Add(pos);
                    }
                    if (userMode == "AI" && solution.Count != 0)                  //For AI, if found a path to attack some Unit, return this path
                        return solution;
                }

                searched_tiles.Add(pos_to_explore);
            }
            //update tiles_to_explore to the list of newly expaneded tiles
            List<int> temp = new List<int>();
            foreach (int tile_pos in all_paths.Keys)
                if (!searched_tiles.Contains(tile_pos) && map_tiles[tile_pos].GetComponent<Tile>().tile_type != "Muddy")
                    temp.Add(tile_pos);

            temp_tiles_to_explore = new List<int>(temp);
        }

        //If AI hasn't find the solution within the range, expand the range endlessly until it finds a solution
        if (userMode == "AI")
        {
            foreach (int pos in all_paths.Keys)
                all_paths[pos].Add(pos);
            for (int i = range; i < map_size; ++i)
            {
                foreach (int pos_to_explore in temp_tiles_to_explore)
                {
                    foreach (int pos in expansion_of_tiles[pos_to_explore])
                    {
                        if (units_state[pos] == null)
                        {
                            if (!all_paths.ContainsKey(pos))
                                all_paths[pos] = new List<int>(all_paths[pos_to_explore]);

                            if (myCondition(pos, unitTag))
                                solution[pos] = all_paths[pos];
                        }
                        if (solution.Count != 0)
                            return solution;

                    }
                    searched_tiles.Add(pos_to_explore);
                }
                List<int> temp = new List<int>();
                foreach (int tile_pos in all_paths.Keys)
                    if (!searched_tiles.Contains(tile_pos))
                        temp.Add(tile_pos);

                temp_tiles_to_explore = new List<int>(temp);
            }
        }
        all_paths.Remove(start_tile);
        return all_paths;
    }

    public bool CheckIfWin()
    {
        int exit_pos = -1;
        foreach (GameObject tile in map_tiles)
        {
            if (tile.GetComponent<Tile>().exit)
            {
                exit_pos = map_tiles_pos[tile];
                break;
            }
        }

        foreach (GameObject unit in units_state)
        {
            if (unit != null && unit.CompareTag("PlayerUnit"))
            {
                Vector3 xyPosition = map_tiles[exit_pos].transform.position;
                Vector3 exit_position = new Vector3(xyPosition.x, xyPosition.y + 0.7f, xyPosition.z - 1.0f);
                if (unit.GetComponent<UserUnit>().hasPeach
                    && unit.GetComponent<UserUnit>().currentPos == exit_pos
                    && !character_moving)
                {
                    return true;
                }
            }
        }

        return false;
    }

    public bool CheckIfLose()
    {

        foreach (GameObject unit in units_state)
        {
            if (unit != null && unit.CompareTag("PlayerUnit"))
            {
                return false;
            }
        }

        return true;


    }

    public void color_skill_tiles(int pos) {
        if(skill_tiles.ContainsKey(pos)){
            foreach(int p in skill_tiles[pos]){
				mark_tile[p] = true;
                map_tiles[p].GetComponent<SpriteRenderer>().color = new Color(200, 0, 0);
            }
        }
    }
    public void uncolor_skill_tiles(int pos){
        if (skill_tiles.ContainsKey(pos)){
            foreach (int p in skill_tiles[pos]){
				if(!skill_tiles.ContainsKey(p)){
					map_tiles[p].GetComponent<SpriteRenderer>().color = new Color(255, 255, 255);
					mark_tile[p] = false;
				} 
                else{
					mark_tile[p] = true;
					map_tiles[p].GetComponent<SpriteRenderer>().color = new Color(0, 0, 200);
                }
            }
        }
    }

    /* Function used for changing the visibility of a group of tiles around a center
     * input:
     *      tile: center of the group
     *      view_range: view range of the character
     *      visual: boolean of whether a tile is visiable or not
     *      character_name: name of the characters
     */
	public void set_tile_group_visibility(int tile, int view_range, bool visual, string character_name){
		// Variables for the search algorithm
		HashSet<int> visited = new HashSet<int>(){tile};
		List<int> frontier = new List<int>(){tile};
		List<int> to_search = new List<int>();

		// Change center visibility
		set_tile_visibility(tile, visual, character_name);

		// Change other tiles visibility
		for (int i = 0; i < view_range; i++){
			foreach(int t in frontier){
				foreach(int new_tile in expansion_of_tiles[t]){
					if(!visited.Contains(new_tile)){
						set_tile_visibility(new_tile, visual, character_name);
						to_search.Add(new_tile);
						visited.Add(new_tile);
					}
				}
			}
			frontier = to_search;
			to_search = new List<int>();
		}
	}


	/* Function used for changing the visibility of a single tile
     * input is similar to the previous function
     */
	private void set_tile_visibility(int tile, bool visual, string character_name){
		if (visual)
			tile_visibility[tile].Add(character_name);
        else
			tile_visibility[tile].Remove(character_name);
	}

    /* Function used for updating the visibility of all tiles
     * 
     */
	private void change_visibility(){
		for (int tile = 0; tile < map_tiles.Count; tile++){
			if(tile_visibility[tile].Count >= 1){
				if(units_state[tile] != null)
                {
                    units_state[tile].GetComponent<SpriteRenderer>().enabled = true;
                    if (units_state[tile].gameObject.tag == "EnemyUnit")
                    {
                        units_state[tile].gameObject.transform.GetChild(0).gameObject.SetActive(true);
                    }
                }

                if (!mark_tile[tile])
				    map_tiles[tile].GetComponent<SpriteRenderer>().color = new Color(255, 255, 255);
			}
			else{
				if(units_state[tile] != null)
                {
                    units_state[tile].GetComponent<SpriteRenderer>().enabled = false;
                    if (units_state[tile].gameObject.tag == "EnemyUnit")
                    {
                        units_state[tile].gameObject.transform.GetChild(0).gameObject.SetActive(false);
                    }
                }

				if(!mark_tile[tile])
					map_tiles[tile].GetComponent<SpriteRenderer>().color = Color.gray;
			}
		}
	}



}
