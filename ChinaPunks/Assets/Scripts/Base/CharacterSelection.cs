using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelection : MonoBehaviour {

	public GameObject char_info_ui;
    public GameObject char_skill_ui;

    Text char_description;
    Image char_skill_info;
    public Sprite Monk_Skill_info;
    public Sprite Archer_Skill_info;
    public Sprite Makepinggo_Skill_info;
    public Sprite Wuchang_Skill_info;
    public Sprite Tauren_Skill_info;
    public Sprite Swordman_Skill_info;

    private GameObject base_manager;

    // Boolean specify whether the character is in the team
	private bool in_team;

    // Cache the start point of the character
	private Vector2 start_position;

    // UI Sound Effect
    public AudioSource Audio;
    public AudioClip Mouse_Over;
    public AudioClip Mouse_Click;

	private void Awake()
	{
		base_manager = GameObject.Find("BaseManager");
		in_team = false;
		start_position = transform.position;
	}

	// Use this for initialization
	void Start () {
		char_description = char_info_ui.GetComponentInChildren<Text>();
        char_skill_info = char_skill_ui.GetComponentInChildren<Image>();

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    // Change characters size when mouse hover on them
	private void OnMouseEnter()
	{
		gameObject.transform.localScale = gameObject.transform.localScale * 1.2f;
	}

	private void OnMouseExit()
	{
        char_info_ui.SetActive(false);
        char_skill_ui.SetActive(false);

        gameObject.transform.localScale = gameObject.transform.localScale / 1.2f;
	}


	private void OnMouseDown()
	{
        Audio.PlayOneShot(Mouse_Click);
        // If character not in team and the team still have empty spot, move the character to the empty spot
        if (!in_team){
			Vector2 destination = base_manager.GetComponent<BaseManager>().AddMember(gameObject.name);
			Vector2 distance = destination - start_position;
			if (!destination.Equals(Vector2.zero))
            {
                transform.Translate(distance);
				in_team = true;
            }
		}
		else{
			Vector2 distance = start_position - (Vector2)gameObject.transform.position;
			transform.Translate(distance);
			in_team = false;
			base_manager.GetComponent<BaseManager>().RemoveMember(gameObject.name);
		}      
	}

	private void OnMouseOver()
	{
        if (!in_team)
		{
            char_info_ui.SetActive(true);
            char_skill_ui.SetActive(true);

            char_description.text = (this.gameObject.name);

            if(this.gameObject.name == "Monk")
            {
                char_description.text = (this.gameObject.name + "\n" + "Health: 40 " + "Attack: 20" + "\n" + "Skill: Drink Wine");
                char_skill_info.sprite = Monk_Skill_info;
            }
            if (this.gameObject.name == "Archer")
            {
                char_description.text = (this.gameObject.name + "\n" + "Health: 25 " + "Attack: 15" + "\n" + "Skill: Arrow Rain");
                char_skill_info.sprite = Archer_Skill_info;
            }
            if (this.gameObject.name == "Tauren")
            {
                char_description.text = (this.gameObject.name + "\n" + "Health: 50 " + "Attack: 20" + "\n" + "Skill: Tauren’s Punch");
                char_skill_info.sprite = Tauren_Skill_info;
            }
            if (this.gameObject.name == "Wuchang")
            {
                char_description.text = (this.gameObject.name + "\n" + "Health: 20 " + "Attack: 15" + "\n" + "Skill: Calls From Hell");
                char_skill_info.sprite = Wuchang_Skill_info;
            }
            if (this.gameObject.name == "Makepingguo")
            {
                char_description.text = (this.gameObject.name + "\n" + "Health: 30 " + "Attack: 15" + "\n" + "Skill: BOMB!");
                char_skill_info.sprite = Makepinggo_Skill_info;
            }
            if (this.gameObject.name == "Swordman")
            {
                char_description.text = (this.gameObject.name + "\n" + "Health: 35 " + "Attack: 20" + "\n" + "Skill: Critical Hit");
                char_skill_info.sprite = Swordman_Skill_info;
            }

        }
	}
}
