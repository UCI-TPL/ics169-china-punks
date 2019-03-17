using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Turn_Control : MonoBehaviour
{


    public GameObject map;
    Map_Control map_ctr;
    public GameObject UI;
    InGameUI UI_ctr;
    //Variable used to keep track whose round the current one is.
    //Value: "Player", "AI"
    public string gameRound;
    public GameObject endTurnButton;

    public GameObject WinScene;
    public GameObject LoseScene;

    public WorldGenerator WG;


    public bool roundUI_showing;

    // Use this for initialization
    void Start()
    {
        gameRound = "Player";

        //debug
        Debug.Log("turn: " + gameRound);


        map_ctr = map.GetComponent<Map_Control>();
        UI_ctr = UI.GetComponent<InGameUI>();
    }

    // Update is called once per frame
    void Update()
    {
        //if (map_ctr.CheckIfWin())
        //{
        //    WinScene.SetActive(true);
        //    map_ctr.reset();
        //}

        //else if (map_ctr.CheckIfLose())
        //{
        //    LoseScene.SetActive(true);
        //    map_ctr.reset();
        //}

        //Player round
        if (gameRound == "Player")
        {
            map_ctr.Character_Click();
        }

        if (map_ctr.character_moving)
        {
            endTurnButton.GetComponent<Button>().interactable = false;
        }
        else
            endTurnButton.GetComponent<Button>().interactable = true;

        if (roundUI_showing)
        {
            Image round_UI;
            if (gameRound == "Player")
            {
                round_UI = UI.GetComponent<InGameUI>().Player_roundUI;
            }
            else if (gameRound == "AI")
            {
                round_UI = UI.GetComponent<InGameUI>().Enemy_roundUI;
            }
            else
            {
                round_UI = null;
                roundUI_showing = false;
            }


            if (round_UI != null)
            {
                round_UI.gameObject.SetActive(true);
                float screen_width = UI.GetComponent<RectTransform>().rect.width;
                float speed;
                float mid_1 = screen_width * 9 / 20;
                float mid_2 = screen_width * 11 / 20;
                if (round_UI.rectTransform.anchoredPosition.x > mid_1 &&
                    round_UI.rectTransform.anchoredPosition.x < mid_2)
                {
                    speed = 200;
                }
                else
                    speed = 2500;

                round_UI.rectTransform.anchoredPosition = new Vector2(round_UI.rectTransform.anchoredPosition.x
                                                                            + speed * Time.deltaTime, 0);

                if (round_UI.rectTransform.anchoredPosition.x > screen_width)
                {
                    roundUI_showing = false;
                    round_UI.rectTransform.anchoredPosition = new Vector2(0, 0);
                    round_UI.gameObject.SetActive(false);
                }
            }

        }

    }


    //Change round between player and AI, AI units from different group all act in "AI" round
    public void changeRound()
    {
        map_ctr.turn_count++;
        WG.rdsGeneEnemy--;

        if (gameRound == "Player")
        {

            for (int i = 0; i < map_ctr.map_size * map_ctr.map_size; i++)
            {
                //reduce tile fire cd
                if (map_ctr.map_tiles[i].GetComponent<Tile>().on_fire)
                {
                    map_ctr.map_tiles[i].GetComponent<Tile>().update_fire();
                }
                //traps disappear
                if (map_ctr.map_tiles[i].GetComponent<Tile>().trap != null)
                {
                    if(map_ctr.map_tiles[i].GetComponent<Tile>().trap.GetComponent<trap>().visible_time !=0)
                        map_ctr.map_tiles[i].GetComponent<Tile>().trap.GetComponent<trap>().visible_time--;
                }


                ////apply fire damage and reduce skill cd
                if (map_ctr.units_state[i] != null)
                {
                    if (map_ctr.units_state[i].tag == "PlayerUnit")
                    {
                        //apply fire damage to character if on fire
                        if(map_ctr.units_state[i].GetComponent<UserUnit>().on_fire)
                            map_ctr.units_state[i].GetComponent<UserUnit>().Health_Change(map_ctr.units_state[i].GetComponent<UserUnit>().fire_damage);
                        //apply poison damage to character if poisoned;
                        if (map_ctr.units_state[i].GetComponent<UserUnit>().poisoned)
                            map_ctr.units_state[i].GetComponent<UserUnit>().Health_Change(map_ctr.units_state[i].GetComponent<UserUnit>().poison_damage);
                        //reduce fire cd
                        if (map_ctr.units_state[i].GetComponent<UserUnit>().on_fire){
                            map_ctr.units_state[i].GetComponent<UserUnit>().fire_cd--;
                            if(map_ctr.units_state[i].GetComponent<UserUnit>().fire_cd == 0){
                                map_ctr.units_state[i].GetComponent<UserUnit>().on_fire = false;
                                map_ctr.units_state[i].GetComponent<UserUnit>().Reset_FireCD();
                            }
                        }
                        //reduce skill cd
                        if (map_ctr.units_state[i].GetComponent<UserUnit>().coolDown != 0)
                            map_ctr.units_state[i].GetComponent<UserUnit>().coolDown--;
                        if (map_ctr.units_state[i].GetComponent<UserUnit>().coolDown == 0)
                            map_ctr.units_state[i].GetComponent<UserUnit>().Reset_Skill();
                        //reduce poison cd
                        if (map_ctr.units_state[i].GetComponent<UserUnit>().poisoned)
                        {
                            map_ctr.units_state[i].GetComponent<UserUnit>().poison_cd--;
                            if (map_ctr.units_state[i].GetComponent<UserUnit>().poison_cd == 0)
                            {
                                map_ctr.units_state[i].GetComponent<UserUnit>().poisoned = false;
                                map_ctr.units_state[i].GetComponent<UserUnit>().Reset_PoisonCD();
                            }
                        }
                    }
                    else if (map_ctr.units_state[i].tag == "EnemyUnit")
                    {   //apply fire damage to enemy
                        if (map_ctr.units_state[i].GetComponent<AIUnit>().on_fire)
                            map_ctr.units_state[i].GetComponent<AIUnit>().Health_Change(map_ctr.units_state[i].GetComponent<AIUnit>().fire_damage);
                        //apply poison damage to character if poisoned;
                        if (map_ctr.units_state[i].GetComponent<AIUnit>().poisoned)
                            map_ctr.units_state[i].GetComponent<AIUnit>().Health_Change(map_ctr.units_state[i].GetComponent<AIUnit>().poison_damage);
                        //reduce fire cd
                        if (map_ctr.units_state[i].GetComponent<AIUnit>().on_fire)
                        {
                            map_ctr.units_state[i].GetComponent<AIUnit>().fire_cd--;
                            if (map_ctr.units_state[i].GetComponent<AIUnit>().fire_cd == 0)
                            {
                                map_ctr.units_state[i].GetComponent<AIUnit>().on_fire = false;
                                map_ctr.units_state[i].GetComponent<AIUnit>().Reset_FireCD();
                            }
                        }
                        //reduce poison cd
                        if (map_ctr.units_state[i].GetComponent<AIUnit>().poisoned)
                        {
                            map_ctr.units_state[i].GetComponent<AIUnit>().poison_cd--;
                            if (map_ctr.units_state[i].GetComponent<AIUnit>().poison_cd == 0)
                            {
                                map_ctr.units_state[i].GetComponent<AIUnit>().poisoned = false;
                                map_ctr.units_state[i].GetComponent<AIUnit>().Reset_PoisonCD();
                            }
                        }
                    }
                }

            }

            map_ctr.reset();
            endTurnButton.SetActive(false);

            //reset AI turncomplete
            if (map_ctr.AI_units.Count >= 1)
            {
                foreach (GameObject ob in map_ctr.AI_units)
                {
                    ob.GetComponent<AIUnit>().turnComplete = false;
                    ob.GetComponent<AIUnit>().moveComplete = false;

                }
            }

            bool enemyNone = true;
            foreach (GameObject ob in map_ctr.units_state)
            {
                if (ob != null && ob.tag == "EnemyUnit")
                    enemyNone = false;
            }

            if (WG.rdsGeneEnemy == 0 || enemyNone) {
                WG.generateEnemy();
            }


            StartCoroutine(map_routine());

        }
    }

    IEnumerator map_routine()
    {
        yield return new WaitUntil(() => !map_ctr.animation_is_playing);


        gameRound = "Map";
        Debug.Log("turn :" + gameRound);
        //Volcano
        foreach (GameObject ob in map_ctr.units_state)
        {
            if (ob != null && ob.name == "Volcano")
            {
                ob.GetComponent<Volcano>().Spray();
                yield return new WaitUntil(() => !ob.GetComponent<Volcano>().fire_moving);
                break;
            }
        }

        //start AI round
        gameRound = "AI";
        Debug.Log("turn :" + gameRound);

        //Excute AI turn
        StartCoroutine(AIBlocker());
    }


    IEnumerator AIBlocker()
    {
        roundUI_showing = true;
        yield return new WaitUntil(() => !roundUI_showing);


        List <GameObject> temp_AI_units = new List<GameObject>(map_ctr.AI_units);

        foreach (GameObject ob in temp_AI_units)
        {

            Debug.Log(ob.name);
            AIUnit enemy = ob.GetComponent<AIUnit>();

            //Find where the AI unit should go
            int move_range;
            //if enemy is on muddy tile, move_range becomes 1
            if (map_ctr.map_tiles[enemy.currentPos].GetComponent<Tile>().tile_type == "Muddy")
                move_range = 1;
            else
                move_range = enemy.moveRange;
            if (ob.name.StartsWith("NianShou")) {
                map_ctr.all_paths = map_ctr.Search_solution(enemy.currentPos, move_range, gameRound, ob.tag, checkPeach);
            }
            else {
                map_ctr.all_paths = map_ctr.Search_solution(enemy.currentPos, move_range, gameRound, ob.tag, checkEnemy);
            }

            int solution_key = -1;
            foreach (int i in map_ctr.all_paths.Keys)
                solution_key = i;
            if (solution_key != -1)
            {
                map_ctr.path = map_ctr.all_paths[solution_key];
            }
            map_ctr.all_paths.Clear();
            //AI unit move in the front end
            enemy.acting = true;
            yield return new WaitUntil(() => !enemy.acting);

        }


        gameRound = "Player";
        endTurnButton.SetActive(true);

        foreach (GameObject ob in map_ctr.units_state)
        {
            if (ob != null && ob.CompareTag("PlayerUnit"))
            {
                ob.GetComponent<UserUnit>().turnComplete = false;
                ob.GetComponent<UserUnit>().moveComplete = false;
            }
        }

        //debug for printing turn
        Debug.Log("turn :" + gameRound);

        roundUI_showing = true;
        yield return new WaitUntil(() => !roundUI_showing);

    }

    //---All other functions should be placed before here---
    //
    //
    //Condition functions that help the search algorithms decide whether a tile satisfies the condition
    //In terms of AI, these can be changed in to heuristic functions

    //Check if a tile has a unit, and that unit is not a unit of the same group. (Possiblely should exclude traps and buildings here)
    private bool checkEnemy(int pos, string unitTag)
    {
        foreach (int i in map_ctr.expansion_of_tiles[pos])
        {
            if (map_ctr.units_state[i] != null
                && !map_ctr.units_state[i].CompareTag(unitTag)
                && !map_ctr.units_state[i].CompareTag("Block")
                && !map_ctr.units_state[i].GetComponent<Unit>().hide)
            {
                if(!map_ctr.provocative)
                    return true;    
                else if(map_ctr.units_state[i].GetComponent<Unit>().provocative)
                {
                    return true;
                }
            }
                
        }
        return false;
    }

    //Check if a tile has a unit, and that unit is a peach
    private bool checkPeach(int pos, string unitTag)
    {
        foreach (int i in map_ctr.expansion_of_tiles[pos])
        {
            if (map_ctr.units_state[i] != null
                && map_ctr.units_state[i].gameObject.name.StartsWith("Peach"))
            {
                if (!map_ctr.provocative)
                    return true;
                else if (map_ctr.units_state[i].GetComponent<Unit>().provocative)
                {
                    return true;
                }
            }

        }
        return false;
    }


    IEnumerator Show_Round_UI(){
        roundUI_showing = true;
        yield return new WaitUntil(() => roundUI_showing = false);

    }
}