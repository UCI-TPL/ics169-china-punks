using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


// Scripts that have control on the team
public class BaseManager : MonoBehaviour {

	// The team that the player has chosen, each element MUST be the character prefab name(aka the character's name)
	private List<string> team;

	// The position that the chosen character need to stay
	private List<Vector2> team_position;

	private BaseUI base_UI;

	private void Awake()
	{
		team = new List<string>() { null, null, null, null };
		team_position = new List<Vector2>() { new Vector2(-5.84f, 1.44f), new Vector2(-2.38f, 1.44f), new Vector2(2.3f, 1.44f), new Vector2(5.71f, 1.44f) };
		base_UI = GameObject.Find("Canvas").GetComponent<BaseUI>();
	}

	// Use this for initialization
	void Start () {
        PlayerPrefs.DeleteAll();
	}
	
	// Update is called once per frame
	void Update () {
	}

    // Add a character to the current team if there is still position open
	public Vector2 AddMember(string character_name){
		for (int i = 0; i < 4; i++){
			if(team[i] == null){
				team[i] = character_name;
				return team_position[i];
			}
		}

		// There is no empty spot in the current team. Tell the player team is full in the UI. Return (0,0) to indicate the chosen character should not move.
		base_UI.errorEvent.Invoke("Team is already full. \n \"A greedy team will never success\"");
		return Vector2.zero;
	}

    // Remove a character from the current team
	public void RemoveMember(string character_name){
		for (int i = 0; i < 4; i++){
			if(team[i] == character_name){
				team[i] = null;
				return;
			}
		}
	}

    // The function for the confirm team button
	public void confirm_team(){
		for (int i = 0; i < 4; i++){
			// The team is not already full and the error message will display
			if(team[i] == null){
				base_UI.errorEvent.Invoke("Team is not full. \n \" An incomplete team is not acceptable\"");
				return;
			}
        }

        // Confirm Team successful
		for (int i = 0; i < 4; i++)
        {
            Team.add_member(team[i]);
            if (team[i] == "Monk")
            {
                PlayerPrefs.SetInt("Monk", 1);
            }
            if (team[i] == "Archer")
            {
                PlayerPrefs.SetInt("Archer", 1);
            }
            if (team[i] == "Makepingguo")
            {
                Debug.Log("makepinggo is here!");
                PlayerPrefs.SetInt("Makepinggo", 1);
            }
            if (team[i] == "Tauren")
            {
                PlayerPrefs.SetInt("Tauren", 1);
            }
            if (team[i] == "Swordman")
            {
                PlayerPrefs.SetInt("Swordman", 1);
            }
			if (team[i] == "Wuchang")
            {
                PlayerPrefs.SetInt("Wuchang", 1);
            }
        }


        Team.change_money(50 - Team.get_money());
		// Load level one
		SceneManager.LoadScene("level1");
	}
}
