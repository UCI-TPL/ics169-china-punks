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
    public int player_character_count;
    public int peach_count;
    public List<int> exits;
    public GameObject timeline;

    public bool level_finished;

    

    


	// Use this for initialization
	void Start () {
        peach_count = 4;
        player_character_count = 4;
	}
	
	// Update is called once per frame
	void Update () {

        // Lose state
        if(mc.peach_count == 0 || mc.user_unit_count == 0) {
            turn.LoseScene.SetActive(true);
        }

        //level? win/lose check
        if (Level == 0){


            if (win_conditions_count == 1)
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

    public void decrease_peach_count() {
        peach_count--;
    }

    public void decrease_player_count() {
        player_character_count--;
    }


}
