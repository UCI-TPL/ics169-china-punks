﻿using System.Collections;
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
    public Button AttackBtn;
    public Button Pickbtn;


    bool ShowedClickedEffect;
    public Button exit;

    // Use this for initialization
    void Start () {
        map_ctr = map.GetComponent<Map_Control>();

        Button Attack_btn = AttackBtn.GetComponent<Button>();
        Attack_btn.onClick.AddListener(() => map_ctr.Character_Attack());

        Button Pick_btn = Pickbtn.GetComponent<Button>();
        Pick_btn.onClick.AddListener(() => map_ctr.Character_PickUp());

        ShowedClickedEffect = false;
        Button _exit = exit.GetComponent<Button>();
        _exit.onClick.AddListener(exit_on_click);

    }
	
	// Update is called once per frame
	void Update () {
        Character_Click();
    }

    void exit_on_click()
    {
        SceneManager.LoadScene("Player_Camp");
        Debug.Log("clicked");
    }

    void Character_Click(){

        if (map_ctr.playerHUD_showed)
        {
            if (map_ctr.current_picked_pos != -1 && map_ctr.picked_pos != map_ctr.current_picked_pos && ShowedClickedEffect)
            {
                //Disable effect if it existed
                if (map_ctr.units_state[map_ctr.current_picked_pos] != null)
                {
                    Destroy(_selectEffect);
                    ShowedClickedEffect = false;
                }
            }

            else if (map_ctr.units_state[map_ctr.picked_pos] != null && map_ctr.units_state[map_ctr.picked_pos].gameObject.tag == "PlayerUnit" && !ShowedClickedEffect)
            {
                //Show effect
                _selectEffect = Instantiate(selectEffect, map_ctr.units_state[map_ctr.picked_pos].gameObject.GetComponent<Transform>());
                _selectEffect.transform.Translate(new Vector3(0, -0.8f, 0));
                ShowedClickedEffect = true;

                //Show player HUD
                InGameHUD.SetActive(true);
            }

            // here to disable attack and pickup button (for temp debug use)
            if (map_ctr.units_state[map_ctr.picked_pos] != null && map_ctr.units_state[map_ctr.picked_pos].gameObject.tag == "PlayerUnit"){
                GameObject player = map_ctr.units_state[map_ctr.picked_pos];
                if (player.gameObject.GetComponent<UserUnit>().hasPeach){
                    AttackBtn.interactable = false;
                    Pickbtn.interactable = false;
                }
                else{
                    AttackBtn.interactable = true;
                    Pickbtn.interactable = true;
                }
            }
        }

        else
        {
            if (ShowedClickedEffect)
            {
                Destroy(_selectEffect);
                ShowedClickedEffect = false;
                InGameHUD.SetActive(false);
            }
        }


    }
}
