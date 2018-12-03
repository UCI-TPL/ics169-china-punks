using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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

    public int peach_pos;

    public bool provocative;

    public GameObject BGCurve;

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

    public bool character_moving = false;

    private void Awake()
    {
        for (int i = 0; i < map_size * map_size; i++)
        {
            units_state.Add(null);
        }
    }
    // Use this for initialization
    void Start()
    {
        turn_count = 1;
        Tile_Store();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            reset();
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
            units_state[picked_pos].GetComponent<UserUnit>().isClicked = false;
        }


        //expanded tile empty?
        if (expanded_tiles.Count != 0)
        {
            //recover tile color from moving state
            foreach (int i in expanded_tiles)
            {
                map_tiles[i].GetComponent<SpriteRenderer>().color = new Color(255, 255, 255);
            }
        }
        //recover tile colors from attack state and pick state
        if (picked_pos != -1)
        {
            foreach (int i in expansion_of_tiles[picked_pos])
            {
                map_tiles[i].GetComponent<SpriteRenderer>().color = new Color(255, 255, 255);
            }

            //Archer
            if(units_state[picked_pos] != null && units_state[picked_pos].name == "Archer"){
                foreach (int i in units_state[picked_pos].GetComponent<Archer>().Attack_range())
                {
                    map_tiles[i].GetComponent<SpriteRenderer>().color = new Color(255, 255, 255);
                }
            }

        }
        //recover skill tiles
        if(skill_tiles.Count!=0){
            foreach(KeyValuePair<int,List<int>> pair in skill_tiles){
                foreach(int pos in pair.Value){
                    map_tiles[pos].GetComponent<SpriteRenderer>().color = new Color(255, 255, 255);
                }
            }

            
        }
        //disable bgcurve
        BGCurve.SetActive(false);

        expanded_tiles.Clear();
        skill_tiles.Clear();
        first_click = true;
        tile_picked = false;
        pickTile = null;
        pickEndTile = null;
        picked_pos = -1;

        playerHUD_showed = false;
    }

    void reset_color(){
        if (units_state[picked_pos] != null && units_state[picked_pos].name == "Archer")
        {
            foreach (int i in units_state[picked_pos].GetComponent<Archer>().Attack_range())
            {
                map_tiles[i].GetComponent<SpriteRenderer>().color = new Color(255, 255, 255);
            }
        }

        if (expanded_tiles.Count != 0)
        {
            foreach (int i in expanded_tiles)
            {
                map_tiles[i].GetComponent<SpriteRenderer>().color = new Color(255, 255, 255);
            }
        }

        //recover tile colors from attack state and pick state
        if (picked_pos != -1)
        {
            foreach (int i in expansion_of_tiles[picked_pos])
            {
                map_tiles[i].GetComponent<SpriteRenderer>().color = new Color(255, 255, 255);
            }

            //Archer
            if (units_state[picked_pos] != null && units_state[picked_pos].name == "Archer")
            {
                foreach (int i in units_state[picked_pos].GetComponent<Archer>().Attack_range())
                {
                    map_tiles[i].GetComponent<SpriteRenderer>().color = new Color(255, 255, 255);
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
                map_tiles[picked_pos + position].GetComponent<SpriteRenderer>().color = new Color(0, 0, 250);
                if (picked_pos % 10 == 0 && (picked_pos + position + 1) % 10 == 0)
                {
                    map_tiles[picked_pos + position].GetComponent<SpriteRenderer>().color = new Color(255, 255, 255);
                }
                else if ((picked_pos + 1) % 10 == 0 && (picked_pos + position) % 10 == 0)
                {
                    map_tiles[picked_pos + position].GetComponent<SpriteRenderer>().color = new Color(255, 255, 255);
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
            map_tiles[i].GetComponent<SpriteRenderer>().color = new Color(0, 200, 0);
        }
        tile_picked = false;
        first_click = false;
        playerHUD_showed = true;


    }

    public void Character_Attack()
    {
        // reset color
        reset_color();

        acting_state = 2;

        List<int> attackRange = new List<int>();
        //check if it's archer's round
        if (units_state[picked_pos].name == "Archer")
            attackRange = units_state[picked_pos].GetComponent<Archer>().Attack_range();
        else
            attackRange = expansion_of_tiles[picked_pos];

        foreach (int position in attackRange)
        {
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
                    first_click = false;
                    acting_state = 1;
                    playerHUD_showed = true;

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
                        path = all_paths[map_tiles_pos[pickEndTile]];
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
                    if (units_state[map_tiles_pos[pickEndTile]] != null && units_state[map_tiles_pos[pickEndTile]].gameObject.tag == "EnemyUnit")
                    {
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
                    if (skill_tiles.ContainsKey(map_tiles_pos[pickEndTile])){
                        //get skill damage
                        float skill_damage = units_state[picked_pos].GetComponent<UserUnit>().skill_damage;
                        foreach (int pos in skill_tiles[map_tiles_pos[pickEndTile]]){
                            //there is unit on this tile
                            if(units_state[pos]!= null){
                                //showing attack message
                                Debug.Log(units_state[picked_pos].gameObject.name + " attacked "
                                  + units_state[pos].gameObject.name);
                                units_state[pos].GetComponent<Unit>().Health_Change(skill_damage);
                                //apply fire effect if attacker is archer
                                if (units_state[picked_pos].gameObject.name == "Archer")
                                    units_state[pos].GetComponent<Unit>().on_fire = true;
                            }
                        }
                        //recover the tile colors after using skill
                        foreach(int k in skill_tiles.Keys){
                            foreach(int pos in skill_tiles[k]){
                                map_tiles[pos].GetComponent<SpriteRenderer>().color = new Color(255, 255, 255);
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

    //search for all tiles that are considered solution for AI
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
                if (!searched_tiles.Contains(tile_pos))
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

                            if (userMode == "AI" && myCondition(pos, unitTag))
                                solution[pos] = all_paths[pos];
                        }
                        if (userMode == "AI" && solution.Count != 0)
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
                    && unit.transform.position == exit_position )
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
                map_tiles[p].GetComponent<SpriteRenderer>().color = new Color(200, 0, 0);
            }
        }
    }
    public void uncolor_skill_tiles(int pos){
        if (skill_tiles.ContainsKey(pos)){
            foreach (int p in skill_tiles[pos]){
                if(!skill_tiles.ContainsKey(p))
                    map_tiles[p].GetComponent<SpriteRenderer>().color = new Color(255, 255, 255);
                else{
                    map_tiles[p].GetComponent<SpriteRenderer>().color = new Color(0, 0, 200);
                }
            }
        }
    }

}
