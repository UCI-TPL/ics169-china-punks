using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;

public class InGameUI : MonoBehaviour {

    public GameObject map;
    private Map_Control map_ctr;

	public GameObject WorldGenerator;

    public GameObject selectEffect;
    GameObject _selectEffect;
    public GameObject InGameHUD;
    public Image Player_roundUI;
    public Image Enemy_roundUI;

    //HUD buttons
    public Button Attackbtn;
    public Button Pickbtn;
    public Button Skillbtn;
    public Button exit;

	//Char Info
	public List<GameObject> Char_infos;
	public int Available_Char_info_slot_num = 4;
	public GameObject MouseOver_Char_info;

    public Sprite Makepinggo;
    public Sprite Monster;
    public Sprite SwordMan;
	public Sprite Archor;
	public Sprite Farmer;
	public Sprite Farmer_Leader;
	public Sprite Tile_1;

	Image Char_avatar;
    Image healthfill;
    Text Health_number;
    Text Char_Move;
    Text Char_attack;

	Image Move_Over_Avatar;
	Text  Move_Over_Text;
	Image Move_Over_Tile;
    //Tile and quest Info
	public GameObject MouseOver_Tile_Info;
	public GameObject Quest_Objectives;

	public List<GameObject> Characters_clone = new List<GameObject>();
	public List<GameObject> MoveOver_Map_Info = new List<GameObject>();

	public bool mouse_is_on_map;
	public bool Lock_on_character = false;

	public Dictionary<string, int> char_info_slots_dict = new Dictionary<string, int>();

    // Use this for initialization
    void Start () {
       
        map_ctr = map.GetComponent<Map_Control>();
		//List<Gameobject_Position_prefab> Character_Generate_Info = WorldGenerator.GetComponent<WorldGenerator>().characters_prefab

        Button Attack_btn = Attackbtn.GetComponent<Button>();
        Attack_btn.onClick.AddListener(() => map_ctr.Character_Attack());

        Button Pick_btn = Pickbtn.GetComponent<Button>();
        Pick_btn.onClick.AddListener(() => map_ctr.Character_PickUp());

        Button Skill_btn = Skillbtn.GetComponent<Button>();
        Skillbtn.onClick.AddListener(() => map_ctr.Character_Skill());

        Button _exit = exit.GetComponent<Button>();
		_exit.onClick.AddListener(exit_on_click);

		GameObject Move_Over_Avatar_Info = MouseOver_Char_info.transform.Find("Char_avatar_back").Find("Char_avatar").gameObject;
		Move_Over_Avatar = Move_Over_Avatar_Info.GetComponent<Image>();
		GameObject Move_Over_Avatar_Name_Info = MouseOver_Char_info.transform.Find("Char_avatar_back").Find("name").gameObject;
		Move_Over_Text = Move_Over_Avatar_Name_Info.GetComponent<Text>();

		GameObject MouseOver_Tile_Info_ = MouseOver_Tile_Info.transform.Find("Image").gameObject;
		Move_Over_Tile = MouseOver_Tile_Info_.GetComponent<Image>();
       

        for (int i = 0; i < 4; i++)
		{
			Char_infos[i].SetActive(false);
		}

		if (Available_Char_info_slot_num != 0)
        {
            for (int i = 0; i < 4; i++)
            {
				if (i + 1 > Characters_clone.Count)
                {
                    break;
                }

				GameObject Char_Avatar = Char_infos[i].transform.Find("Char_avatar_back").Find("Char_avatar").gameObject;
				GameObject Char_Health = Char_infos[i].transform.Find("Char_Health_bar").Find("Health_number").gameObject;
				GameObject Char_Health_bar = Char_infos[i].transform.Find("Char_Health_bar").Find("Health_FILLImage").gameObject;
				GameObject Char_Move_num = Char_infos[i].transform.Find("Char_Move").Find("Move_range").gameObject;
				GameObject Char_Attack = Char_infos[i].transform.Find("Char_Attack").Find("Attack_Damage").gameObject;

                Char_avatar = Char_Avatar.GetComponent<Image>();
                Health_number = Char_Health.GetComponent<Text>();
                Char_Move = Char_Move_num.GetComponent<Text>();
                Char_attack = Char_Attack.GetComponent<Text>();
                healthfill = Char_Health_bar.GetComponent<Image>();

				if(Characters_clone[i].name == "Makepinggo(Clone)")
				{
                    Char_infos[i].SetActive(true);               
					Health_number.text = ((Characters_clone[i].GetComponent<Makepinggo>().current_health).ToString() + "/" +
					                      (Characters_clone[i].GetComponent<Makepinggo>().health).ToString());
					healthfill.fillAmount = ((Characters_clone[i].GetComponent<Makepinggo>().current_health) /
					                         (Characters_clone[i].GetComponent<Makepinggo>().health));
					Char_attack.text = (Characters_clone[i].GetComponent<Makepinggo>().attack_damage).ToString();
					Char_Move.text = (Characters_clone[i].GetComponent<Makepinggo>().moveRange).ToString();
					Char_avatar.sprite = Makepinggo;
					char_info_slots_dict["Makepinggo"] = i;
				}
				else if (Characters_clone[i].name == "Monster(Clone)")
                {
					Char_infos[i].SetActive(true);               
					Health_number.text = ((Characters_clone[i].GetComponent<Monster>().current_health).ToString() + "/" +
					                      (Characters_clone[i].GetComponent<Monster>().health).ToString());
					healthfill.fillAmount = ((Characters_clone[i].GetComponent<Monster>().current_health) /
					                         (Characters_clone[i].GetComponent<Monster>().health));
					Char_attack.text = (Characters_clone[i].GetComponent<Monster>().attack_damage).ToString();
					Char_Move.text = (Characters_clone[i].GetComponent<Monster>().moveRange).ToString();
					Char_avatar.sprite = Monster;
					char_info_slots_dict["Trapmaster"] = i;
                }
				else if (Characters_clone[i].name == "Archer(Clone)")
                {
					Char_infos[i].SetActive(true);               
					Health_number.text = ((Characters_clone[i].GetComponent<Archer>().current_health).ToString() + "/" +
					                      (Characters_clone[i].GetComponent<Archer>().health).ToString());
					healthfill.fillAmount = ((Characters_clone[i].GetComponent<Archer>().current_health) /
					                         (Characters_clone[i].GetComponent<Archer>().health));
					Char_attack.text = (Characters_clone[i].GetComponent<Archer>().attack_damage).ToString();
					Char_Move.text = (Characters_clone[i].GetComponent<Archer>().moveRange).ToString();
					Char_avatar.sprite = Archor;  
					char_info_slots_dict["Archor"] = i;
                }
				else if (Characters_clone[i].name == "SwordMan(Clone)")
                {
					Char_infos[i].SetActive(true);               
					Health_number.text = ((Characters_clone[i].GetComponent<SwordMan>().current_health).ToString() + "/" +
					                      (Characters_clone[i].GetComponent<SwordMan>().health).ToString());
					healthfill.fillAmount = ((Characters_clone[i].GetComponent<SwordMan>().current_health) /
					                         (Characters_clone[i].GetComponent<SwordMan>().health));
					Char_attack.text = (Characters_clone[i].GetComponent<SwordMan>().attack_damage).ToString();
					Char_Move.text = (Characters_clone[i].GetComponent<SwordMan>().moveRange).ToString();
					Char_avatar.sprite = SwordMan;   
					char_info_slots_dict["SwordMan"] = i;
                }            
				Available_Char_info_slot_num -= 1;
            }
        }

    }
	
	// Update is called once per frame
	void Update () {
        Character_Click();


        if (mouse_is_on_map == true)
		{
			if (MoveOver_Map_Info.Count != 0)
            {
                MouseOver_Char_info.SetActive(true);
                MouseOver_Tile_Info.SetActive(false);

				if (map_ctr.acting_state != 1)
				{
					if (MoveOver_Map_Info[0].name == "Makepinggo(Clone)")
                    {
                        Move_Over_Avatar.sprite = Makepinggo;
						Move_Over_Text.text = "Makepinggo";
                    }
					if (MoveOver_Map_Info[0].name == "Monster(Clone)")
                    {
                        Move_Over_Avatar.sprite = Monster;
						Move_Over_Text.text = "Monster";
                    }
                    if (MoveOver_Map_Info[0].name == "SwordMan(Clone)")
                    {
                        Move_Over_Avatar.sprite = SwordMan;
						Move_Over_Text.text = "SwordMan";
                    }
                    if (MoveOver_Map_Info[0].name == "Archer(Clone)")
                    {
                        Move_Over_Avatar.sprite = Archor;
						Move_Over_Text.text = "Archer";
                    }
					if (MoveOver_Map_Info[0].name == "Farmer(Clone)")
                    {
                        Move_Over_Avatar.sprite = Farmer;
                        Move_Over_Text.text = "Farmer";
                    }
					if (MoveOver_Map_Info[0].name == "FarmerLeader(Clone)")
                    {
						Move_Over_Avatar.sprite = Farmer_Leader;
						Move_Over_Text.text = "FarmerLeader";
                    }
				}            
            }
            else if (MoveOver_Map_Info.Count == 0)
            {
				if(map_ctr.acting_state != 1)
				{
					MouseOver_Char_info.SetActive(false);
				}
                MouseOver_Tile_Info.SetActive(true);
                Move_Over_Tile.sprite = Tile_1;
            }
		}
		else
		{
			if (map_ctr.acting_state != 1)
            {
                MouseOver_Char_info.SetActive(false);
            }
			MouseOver_Tile_Info.SetActive(false);
		}

        //update character info in the left
     
        
        for (int i = 0; i < 4; i++)
		{
			if (i + 1 > Characters_clone.Count)
            {
                break;
            }

			if(Characters_clone[i].name == "Makepinggo(Clone)")
			{
				GameObject _Fill = Characters_clone[i].transform.Find("health_canvas/Small_Health_bar/fill").gameObject;
				Image _fill_image = _Fill.GetComponent<Image>();
				_fill_image.fillAmount = ((Characters_clone[i].GetComponent<Makepinggo>().current_health) / 
				                          (Characters_clone[i].GetComponent<Makepinggo>().health));
				foreach (var item in Char_infos)
				{
					if (item.active == true)
					{
						Char_infos[char_info_slots_dict["Makepinggo"]].transform.Find("Char_Health_bar").Find("Health_FILLImage").gameObject.GetComponent<Image>().fillAmount = 
							((Characters_clone[i].GetComponent<Makepinggo>().current_health) / (Characters_clone[i].GetComponent<Makepinggo>().health));;
						Char_infos[char_info_slots_dict["Makepinggo"]].transform.Find("Char_Health_bar").Find("Health_number").gameObject.GetComponent<Text>().text =
							                                              ((Characters_clone[i].GetComponent<Makepinggo>().current_health).ToString() + "/" + (Characters_clone[i].GetComponent<Makepinggo>().health).ToString());                                      
					}
				}
			}

			if (Characters_clone[i].name == "Monster(Clone)")
            {
                GameObject _Fill = Characters_clone[i].transform.Find("health_canvas/Small_Health_bar/fill").gameObject;
                Image _fill_image = _Fill.GetComponent<Image>();
				_fill_image.fillAmount = ((Characters_clone[i].GetComponent<Monster>().current_health) /
				                          (Characters_clone[i].GetComponent<Monster>().health));

//				Debug.Log("monk current health: " + (Characters_clone[i].GetComponent<Monk>().current_health).ToString());
				foreach (var item in Char_infos)
                {
                    if (item.active == true)
                    {
						Char_infos[char_info_slots_dict["Monk"]].transform.Find("Char_Health_bar").Find("Health_FILLImage").gameObject.GetComponent<Image>().fillAmount =
							                                        ((Characters_clone[i].GetComponent<Monster>().current_health) / (Characters_clone[i].GetComponent<Monster>().health)); ;
						Char_infos[char_info_slots_dict["Monk"]].transform.Find("Char_Health_bar").Find("Health_number").gameObject.GetComponent<Text>().text =
							                                        ((Characters_clone[i].GetComponent<Monster>().current_health).ToString() + "/" + (Characters_clone[i].GetComponent<Monster>().health).ToString());
                    }
                }
            }

			if (Characters_clone[i].name == "SwordMan(Clone)")
            {
                GameObject _Fill = Characters_clone[i].transform.Find("health_canvas/Small_Health_bar/fill").gameObject;
                Image _fill_image = _Fill.GetComponent<Image>();
				_fill_image.fillAmount = ((Characters_clone[i].GetComponent<SwordMan>().current_health) /
				                          (Characters_clone[i].GetComponent<SwordMan>().health));
				foreach (var item in Char_infos)
				if (item.active == true)
                {
					Char_infos[char_info_slots_dict["SwordMan"]].transform.Find("Char_Health_bar").Find("Health_FILLImage").gameObject.GetComponent<Image>().fillAmount =
						                                              ((Characters_clone[i].GetComponent<SwordMan>().current_health) / (Characters_clone[i].GetComponent<SwordMan>().health)); ;
					Char_infos[char_info_slots_dict["SwordMan"]].transform.Find("Char_Health_bar").Find("Health_number").gameObject.GetComponent<Text>().text =
						                                              ((Characters_clone[i].GetComponent<SwordMan>().current_health).ToString() + "/" + (Characters_clone[i].GetComponent<SwordMan>().health).ToString());
                }
            }

			if (Characters_clone[i].name == "Archer(Clone)")
            {
                GameObject _Fill = Characters_clone[i].transform.Find("health_canvas/Small_Health_bar/fill").gameObject;
                Image _fill_image = _Fill.GetComponent<Image>();
				_fill_image.fillAmount = ((Characters_clone[i].GetComponent<Archer>().current_health) /
				                          (Characters_clone[i].GetComponent<Archer>().health));
				foreach (var item in Char_infos)
				if (item.active == true)
                {
					Char_infos[char_info_slots_dict["Archer"]].transform.Find("Char_Health_bar").Find("Health_FILLImage").gameObject.GetComponent<Image>().fillAmount =
						                                          ((Characters_clone[i].GetComponent<Archer>().current_health) / (Characters_clone[i].GetComponent<Archer>().health)); ;
					Char_infos[char_info_slots_dict["Archer"]].transform.Find("Char_Health_bar").Find("Health_number").gameObject.GetComponent<Text>().text =
						                                          ((Characters_clone[i].GetComponent<Archer>().current_health).ToString() + "/" + (Characters_clone[i].GetComponent<Archer>().health).ToString());
                }
            }
		}

    }

    public void exit_on_click()
    {
        SceneManager.LoadSceneAsync("TowerBase");
        Debug.Log("clicked");
    }

    void Character_Click(){

        if (map_ctr.playerHUD_showed)
        {
			//pick tile has player unit
            if (map_ctr.picked_pos != -1 && map_ctr.units_state[map_ctr.picked_pos] != null 
                && map_ctr.units_state[map_ctr.picked_pos].gameObject.tag == "PlayerUnit")
            {
                //Show player HUD
                InGameHUD.SetActive(true);
            }

            // disable buttons if character's turn ends
            if (map_ctr.units_state[map_ctr.picked_pos] != null 
                && map_ctr.units_state[map_ctr.picked_pos].gameObject.tag == "PlayerUnit"){
                GameObject player = map_ctr.units_state[map_ctr.picked_pos];
                //character has peach or is moving
                if (player.gameObject.GetComponent<UserUnit>().hasPeach
                    || player.gameObject.GetComponent<UserUnit>().hide
                    || map_ctr.character_moving)
                {
                    Attackbtn.interactable = false;
                    Pickbtn.interactable = false;
                    Skillbtn.interactable = false;
                }
                //character's turn ends
                else if (map_ctr.units_state[map_ctr.picked_pos].gameObject.GetComponent<UserUnit>().turnComplete){
                    Attackbtn.interactable = false;
                    Pickbtn.interactable = false;
                    Skillbtn.interactable = false;
                }
                //character's skill in CD
                else if (map_ctr.units_state[map_ctr.picked_pos].gameObject.GetComponent<UserUnit>().coolDown != 0){
                    Attackbtn.interactable = true;
                    Pickbtn.interactable = true;
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
