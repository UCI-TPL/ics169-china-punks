using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InGameUI : MonoBehaviour {

    public GameObject map;
    private Map_Control map_ctr;


    public GameObject selectEffect;
    GameObject _selectEffect;
    public GameObject InGameHUD;

    //HUD buttons
    public Button Attackbtn;
    public Button Pickbtn;
    public Button Skillbtn;


    bool ShowedClickedEffect;
    public Button exit;

    // Use this for initialization
    void Start () {
        map_ctr = map.GetComponent<Map_Control>();

        Button Attack_btn = Attackbtn.GetComponent<Button>();
        Attack_btn.onClick.AddListener(() => map_ctr.Character_Attack());

        Button Pick_btn = Pickbtn.GetComponent<Button>();
        Pick_btn.onClick.AddListener(() => map_ctr.Character_PickUp());

        Button Skill_btn = Skillbtn.GetComponent<Button>();
        Skillbtn.onClick.AddListener(() => map_ctr.Character_Skill());

        ShowedClickedEffect = false;
        Button _exit = exit.GetComponent<Button>();
        _exit.onClick.AddListener(exit_on_click);

    }
	
	// Update is called once per frame
	void Update () {
        Character_Click();
    }

    public void exit_on_click()
    {
        SceneManager.LoadScene("Player_Camp");
        Debug.Log("clicked");
    }

    void Character_Click(){

        if (map_ctr.playerHUD_showed)
        {
            //pick tile has player unit
            if (map_ctr.units_state[map_ctr.picked_pos] != null 
                && map_ctr.units_state[map_ctr.picked_pos].gameObject.tag == "PlayerUnit")
            {
                //Show player HUD
                InGameHUD.SetActive(true);
            }

            // here to disable attack and pickup button (for temp debug use)
            if (map_ctr.units_state[map_ctr.picked_pos] != null 
                && map_ctr.units_state[map_ctr.picked_pos].gameObject.tag == "PlayerUnit"){
                GameObject player = map_ctr.units_state[map_ctr.picked_pos];
                if (player.gameObject.GetComponent<UserUnit>().hasPeach
                   || map_ctr.units_state[map_ctr.picked_pos].gameObject.GetComponent<UserUnit>().turnComplete)
                {
                    Attackbtn.interactable = false;
                    Pickbtn.interactable = false;
                    Skillbtn.interactable = false;
                }
                else {
                    Attackbtn.interactable = true;
                    Pickbtn.interactable = true;
                    Skillbtn.interactable = true;
                }
            }
        }

        else
        {
            //Destroy(_selectEffect);
            InGameHUD.SetActive(false);
        }


    }
}
