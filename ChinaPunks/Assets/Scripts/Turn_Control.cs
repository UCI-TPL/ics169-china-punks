using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turn_Control : MonoBehaviour {


    public GameObject map;

    public string gameRound;
    public GameObject endTurnButton;


    private Map_Control map_ctr;


    // Use this for initialization
    void Start () {
        gameRound = "Player";

        //debug
        Debug.Log("turn: " + gameRound);


        map_ctr = map.GetComponent<Map_Control>();
	}
	
	// Update is called once per frame
	void Update () {
        //Player round
        if(gameRound == "Player"){
            map_ctr.Character_Click();
        }


        //Execute AI round
        if (gameRound == "AI")
        {
            Debug.Log("AI round");

            changeRound();
        }

    }


    //Change round between player and AI, AI units from different group all act in "AI" round
    public void changeRound()
    {
        if (gameRound == "Player")
        {
            gameRound = "AI";
            endTurnButton.SetActive(false);

            //debug for printing turn
            Debug.Log("turn :" + gameRound);

        }

        else
        {
            gameRound = "Player";
            endTurnButton.SetActive(true);

            //debug for printing turn
            Debug.Log("turn :" + gameRound);
        }
    }

    IEnumerator pauseSimulator()
    {
        yield return new WaitForSeconds(5);
    }


}
