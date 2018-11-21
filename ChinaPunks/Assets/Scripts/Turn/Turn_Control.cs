﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turn_Control : MonoBehaviour
{


    public GameObject map;
    private Map_Control map_ctr;

    //Variable used to keep track whose round the current one is.
    //Value: "Player", "AI"
    public string gameRound;
    public GameObject endTurnButton;

    public GameObject WinScene;
    public GameObject LoseScene;

    // Use this for initialization
    void Start()
    {
        gameRound = "Player";

        //debug
        Debug.Log("turn: " + gameRound);


        map_ctr = map.GetComponent<Map_Control>();
    }

    // Update is called once per frame
    void Update()
    {
        if (map_ctr.CheckIfWin())
        {
            WinScene.SetActive(true);
            map_ctr.reset();
            Time.timeScale = 0;
        }

        else if (map_ctr.CheckIfLose())
        {
            LoseScene.SetActive(true);
            map_ctr.reset();
            Time.timeScale = 0;
        }

        //Player round
        if (gameRound == "Player")
        {
            map_ctr.Character_Click();
        }

    }


    //Change round between player and AI, AI units from different group all act in "AI" round
    public void changeRound()
    {
        if (gameRound == "Player")
        {
            //reduce skill cd of each character by 1
            foreach(GameObject character in map_ctr.units_state){
                //this gameobject is a character
                if(character != null && character.tag == "PlayerUnit"){
                    //skill cd is not 0
                    if (character.GetComponent<UserUnit>().coolDown != 0)
                        character.GetComponent<UserUnit>().coolDown--;
                    if (character.GetComponent<UserUnit>().coolDown == 0)
                        character.GetComponent<UserUnit>().Reset_Skill();
                }
            }
            map_ctr.reset();
            gameRound = "AI";
            endTurnButton.SetActive(false);

            //debug for printing turn
            Debug.Log("turn :" + gameRound);

            //Excute AI turn
            StartCoroutine(AIBlocker());

        }
    }

    IEnumerator AIBlocker()
    {

        foreach (GameObject ob in map_ctr.AI_units)
        {
            AIUnit enemy = ob.GetComponent<AIUnit>();

            //Find where the AI unit should go
            int move_range = enemy.moveRange;
            map_ctr.all_paths = map_ctr.Search_solution(enemy.currentPos, move_range, gameRound, ob.tag, checkEnemy);
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
            if (map_ctr.units_state[i] != null && !map_ctr.units_state[i].CompareTag(unitTag))
                return true;
        }
        return false;
    }

}