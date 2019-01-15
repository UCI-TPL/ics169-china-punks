using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class WinLoseCheck : MonoBehaviour {


    public int Level;

    public Map_Control mc;
    public Turn_Control turn;

    public int win_conditions_count;
    public bool lose;
    public List<GameObject> character_list = new List<GameObject>();
    public List<int> exits;
    public GameObject timeline;

    public bool level_finished;

    

    


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        //level? win/lose check
        if (Level == 0){
            foreach(GameObject character in character_list){
                if (exits.Contains(character.GetComponent<UserUnit>().currentPos))
                    win_conditions_count++;
            }


            if (win_conditions_count == 2)
            {
                if (!mc.character_moving)
                {
                    if (!level_finished)
                    {
                        //player win, hide HUD
                        for (int i = 0; i < turn.UI.GetComponent<InGameUI>().transform.childCount; ++i)
                            turn.UI.GetComponent<InGameUI>().transform.GetChild(i).gameObject.SetActive(false);

                        //start timeline
                        if (timeline.GetComponent<PlayableDirector>().state == PlayState.Paused)
                        {
                            timeline.GetComponent<PlayableDirector>().Play();
                            mc.reset();
                            level_finished = true;
                        }
                    }
                    //timeline ends, level finishes
                    else if (timeline.GetComponent<PlayableDirector>().state == PlayState.Paused)
                    {
                        turn.WinScene.SetActive(true);
                    }
                }
            }
            else
                win_conditions_count = 0;
        }


	}




}
