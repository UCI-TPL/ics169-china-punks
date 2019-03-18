using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;

public class InGameUI : MonoBehaviour {

    public static int Score;

    public GameObject wave_info;
    Text wave_num;

    public GameObject score_info;
    Text score_num;

    public GameObject UI_WAVE;
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
    public GameObject MouseOver_Char_Skill_info;
    public GameObject MouseOver_Skill_CD_Info;

    public Sprite Makepinggo;
    public Sprite Tauren;
    public Sprite SwordMan;
	public Sprite Archer;
    public Sprite Monk;
	public Sprite Wuchang;
    public Sprite Skeleton;
	public Sprite Skeleton_Leader;
    public Sprite NianShou;
    public Sprite Table;
    public Sprite Bucket;
	public Sprite Tile_1;

    public Sprite Makepinggo_skill;
    public Sprite Tauren_skill;
    public Sprite SwordMan_skill;
    public Sprite Archer_skill;
    public Sprite Monk_skill;
    public Sprite Wuchang_skill;

    Image Char_avatar;
    Image healthfill;
    Text Health_number;
    Text Char_Move;
    Text Char_attack;
	Text Enemy_Num;
	Text Next_WAVE_Round;
    Text Current_Char_Skill_CD;

	public Image Move_Over_Avatar;
    public Image Move_Over_Char_Skill;
    Text  Move_Over_Text;
	Image Move_Over_Tile;
    Text Move_Over_Tile_Name;

    public int Level = 1;

    //Tile and quest Info
	public GameObject MouseOver_Tile_Info;
	public GameObject Enemy_Info;
	public GameObject Round_Info;

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

        GameObject Move_Over_Char_Skill_Info = MouseOver_Char_Skill_info.transform.Find("Char_avatar_back").gameObject;
        Move_Over_Char_Skill = Move_Over_Char_Skill_Info.GetComponent<Image>();

        GameObject MouseOver_Tile_Info_1 = MouseOver_Tile_Info.transform.Find("Image").gameObject;
		Move_Over_Tile = MouseOver_Tile_Info_1.GetComponent<Image>();

        GameObject MouseOver_Tile_Info_2 = MouseOver_Tile_Info.transform.Find("Tile_name").gameObject;
        Move_Over_Tile_Name = MouseOver_Tile_Info_2.GetComponent<Text>();

		GameObject Enemy_Information = Enemy_Info.transform.Find("Enemy_num").gameObject;
		Enemy_Num = Enemy_Information.GetComponent<Text>();

		GameObject Round_Information = Round_Info.transform.Find("round_remain").gameObject;
		Next_WAVE_Round = Round_Information.GetComponent<Text>();

        wave_num = wave_info.transform.Find("wave").gameObject.GetComponent<Text>();
        score_num = score_info.transform.Find("score").gameObject.GetComponent<Text>();

        UI_WAVE.SetActive(false);

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

                if (Characters_clone[i].name == "Monk(Clone)")
                {
                    Char_infos[i].SetActive(true);
                    Health_number.text = ((Characters_clone[i].GetComponent<Monk>().current_health).ToString() + "/" +
                                          (Characters_clone[i].GetComponent<Monk>().health).ToString());
                    healthfill.fillAmount = ((Characters_clone[i].GetComponent<Monk>().current_health) /
                                             (Characters_clone[i].GetComponent<Monk>().health));
                    Char_attack.text = (Characters_clone[i].GetComponent<Monk>().attack_damage).ToString();
                    Char_Move.text = (Characters_clone[i].GetComponent<Monk>().dmgUp_by_skill).ToString();
                    Char_avatar.sprite = Monk;
                    char_info_slots_dict["Monk"] = i;
                }

                if (Characters_clone[i].name == "Makepinggo(Clone)")
				{
                    Char_infos[i].SetActive(true);               
					Health_number.text = ((Characters_clone[i].GetComponent<Makepinggo>().current_health).ToString() + "/" +
					                      (Characters_clone[i].GetComponent<Makepinggo>().health).ToString());
					healthfill.fillAmount = ((Characters_clone[i].GetComponent<Makepinggo>().current_health) /
					                         (Characters_clone[i].GetComponent<Makepinggo>().health));
					Char_attack.text = (Characters_clone[i].GetComponent<Makepinggo>().attack_damage).ToString();
					Char_Move.text = (Characters_clone[i].GetComponent<Makepinggo>().skill_damage).ToString();
					Char_avatar.sprite = Makepinggo;
					char_info_slots_dict["Makepinggo"] = i;
				}
				else if (Characters_clone[i].name == "Tauren(Clone)")
                {
					Char_infos[i].SetActive(true);               
					Health_number.text = ((Characters_clone[i].GetComponent<Tauren>().current_health).ToString() + "/" +
					                      (Characters_clone[i].GetComponent<Tauren>().health).ToString());
					healthfill.fillAmount = ((Characters_clone[i].GetComponent<Tauren>().current_health) /
					                         (Characters_clone[i].GetComponent<Tauren>().health));
					Char_attack.text = (Characters_clone[i].GetComponent<Tauren>().attack_damage).ToString();
					Char_Move.text = (Characters_clone[i].GetComponent<Tauren>().skill_damage).ToString();
					Char_avatar.sprite = Tauren;
					char_info_slots_dict["Tauren"] = i;
                }
				else if (Characters_clone[i].name == "Archer(Clone)")
                {
					Char_infos[i].SetActive(true);               
					Health_number.text = ((Characters_clone[i].GetComponent<Archer>().current_health).ToString() + "/" +
					                      (Characters_clone[i].GetComponent<Archer>().health).ToString());
					healthfill.fillAmount = ((Characters_clone[i].GetComponent<Archer>().current_health) /
					                         (Characters_clone[i].GetComponent<Archer>().health));
					Char_attack.text = (Characters_clone[i].GetComponent<Archer>().attack_damage).ToString();
					Char_Move.text = (Characters_clone[i].GetComponent<Archer>().skill_damage).ToString();
					Char_avatar.sprite = Archer;  
					char_info_slots_dict["Archer"] = i;
                }
				else if (Characters_clone[i].name == "Wuchang(Clone)")
                {
                    Char_infos[i].SetActive(true);
					Health_number.text = ((Characters_clone[i].GetComponent<Wuchang>().current_health).ToString() + "/" +
					                      (Characters_clone[i].GetComponent<Wuchang>().health).ToString());
					healthfill.fillAmount = ((Characters_clone[i].GetComponent<Wuchang>().current_health) /
					                         (Characters_clone[i].GetComponent<Wuchang>().health));
					Char_attack.text = (Characters_clone[i].GetComponent<Wuchang>().attack_damage).ToString();
					Char_Move.text = (Characters_clone[i].GetComponent<Wuchang>().skill_damage).ToString();
					Char_avatar.sprite = Wuchang;
					char_info_slots_dict["Wuchang"] = i;
                }
				else if (Characters_clone[i].name == "SwordMan(Clone)")
                {
					Char_infos[i].SetActive(true);               
					Health_number.text = ((Characters_clone[i].GetComponent<SwordMan>().current_health).ToString() + "/" +
					                      (Characters_clone[i].GetComponent<SwordMan>().health).ToString());
					healthfill.fillAmount = ((Characters_clone[i].GetComponent<SwordMan>().current_health) /
					                         (Characters_clone[i].GetComponent<SwordMan>().health));
					Char_attack.text = (Characters_clone[i].GetComponent<SwordMan>().attack_damage).ToString();
					Char_Move.text = (Characters_clone[i].GetComponent<SwordMan>().skill_damage).ToString();
					Char_avatar.sprite = SwordMan;   
					char_info_slots_dict["SwordMan"] = i;
                }            
				Available_Char_info_slot_num -= 1;
            }
        }

    }
	
	// Update is called once per frame
	void Update () {

        wave_num.text = ("Wave: " + Turn_Control.Wave_Num.ToString());
        score_num.text = ("Score: " + Score);
        Debug.Log("score: " + Score);

        if (map_ctr.units_state[map_ctr.picked_pos] != null)
        {
            if (!map_ctr.units_state[map_ctr.picked_pos].gameObject.GetComponent<UserUnit>().turnComplete)
            {
                UI_WAVE.SetActive(false);
            }
        }

        Character_Click();

        // Change the money number to the number player team has in the battle scene
		gameObject.transform.Find("Gold").GetChild(0).GetComponent<Text>().text = Team.get_money().ToString();

        foreach (var item in GameObject.FindGameObjectsWithTag("EnemyUnit"))
        {
            GameObject _Fill = item.transform.Find("health_canvas/Small_Health_bar/fill").gameObject;
            Image _fill_image = _Fill.GetComponent<Image>();
            _fill_image.fillAmount = ((item.transform.GetComponent<AIUnit>().current_health) /
                                      (item.transform.GetComponent<AIUnit>().health));
        }

		Enemy_Num.text = "Enemy Number: " + (GameObject.FindGameObjectsWithTag("EnemyUnit").Length).ToString();
		Next_WAVE_Round.text = ("Next Wave in " + WorldGenerator.GetComponent<WorldGenerator>().rdsGeneEnemy + " rounds");

        if (mouse_is_on_map == true)
		{
			if (MoveOver_Map_Info.Count != 0)
            {
                MouseOver_Char_info.SetActive(true);
                MouseOver_Char_Skill_info.SetActive(true);
                MouseOver_Tile_Info.SetActive(false);

                //Debug.Log("over name: "+MoveOver_Map_Info[0].name);


                if (map_ctr.acting_state != 1)
				{
					if (MoveOver_Map_Info[0].name == "Makepinggo(Clone)")
                    {
                        Move_Over_Avatar.sprite = Makepinggo;
						Move_Over_Text.text = "Makepinggo";
                        Move_Over_Char_Skill.sprite = Makepinggo_skill;

                    }
                    if (MoveOver_Map_Info[0].name == "Monk(Clone)")
                    {
                        Move_Over_Avatar.sprite = Monk;
                        Move_Over_Text.text = "Monk";
                        Move_Over_Char_Skill.sprite = Monk_skill;
                    }
                    if (MoveOver_Map_Info[0].name == "Tauren(Clone)")
                    {
                        Move_Over_Avatar.sprite = Tauren;
						Move_Over_Text.text = "Tauren";
                        Move_Over_Char_Skill.sprite = Tauren_skill;
                    }
                    if (MoveOver_Map_Info[0].name == "SwordMan(Clone)")
                    {
                        Move_Over_Avatar.sprite = SwordMan;
						Move_Over_Text.text = "SwordMan";
                        Move_Over_Char_Skill.sprite = SwordMan_skill;
                    }
                    if (MoveOver_Map_Info[0].name == "Archer(Clone)")
                    {
						Move_Over_Avatar.sprite = Archer;
						Move_Over_Text.text = "Archer";
                        Move_Over_Char_Skill.sprite = Archer_skill;
                    }
					if (MoveOver_Map_Info[0].name == "Wuchang(Clone)")
                    {
						Move_Over_Avatar.sprite = Wuchang;
						Move_Over_Text.text = "Wuchang";
                        Move_Over_Char_Skill.sprite = Wuchang_skill;
                    }
					if (MoveOver_Map_Info[0].name == "Skeleton(Clone)" && MoveOver_Map_Info[0].gameObject.transform.GetChild(0).gameObject.activeSelf == true)
                    {
                        Move_Over_Avatar.sprite = Skeleton;
                        Move_Over_Text.text = "Skeleton";
                        MouseOver_Char_Skill_info.SetActive(false);
                    }
					if (MoveOver_Map_Info[0].name == "SkeletonLeader(Clone)" && MoveOver_Map_Info[0].gameObject.transform.GetChild(0).gameObject.activeSelf == true)
                    {
						Move_Over_Avatar.sprite = Skeleton_Leader;
						Move_Over_Text.text = "SkeletonLeader";
                        MouseOver_Char_Skill_info.SetActive(false);
                    }

                    if (MoveOver_Map_Info[0].name == "Table(Clone)")
                    {
                        Move_Over_Avatar.sprite = Table;
                        Move_Over_Text.text = "Table";
                        MouseOver_Char_Skill_info.SetActive(false);
                    }

                    if (MoveOver_Map_Info[0].name == "Bucket(Clone)")
                    {
                        Move_Over_Avatar.sprite = Bucket;
                        Move_Over_Text.text = "Bucket";
                        MouseOver_Char_Skill_info.SetActive(false);
                    }

                    if (MoveOver_Map_Info[0].name == "NianShou(Clone)")
                    {
                        //Move_Over_Avatar.sprite = Bucket;
                        //Move_Over_Text.text = "Bucket";
                        MouseOver_Char_Skill_info.SetActive(false);
                    }

                    if (MoveOver_Map_Info[0].name == "Peach(Clone)")
                    {
                        Move_Over_Avatar.sprite = NianShou;
                        Move_Over_Text.text = "NianShou";
                        MouseOver_Char_Skill_info.SetActive(false);
                    }
                }            
            }
            else if (MoveOver_Map_Info.Count == 0)
            {
				if(map_ctr.acting_state != 1)
				{
					MouseOver_Char_info.SetActive(false);
                    MouseOver_Char_Skill_info.SetActive(false);
				}
                MouseOver_Tile_Info.SetActive(true);
                if(Level == 1)
                {
                    Move_Over_Tile.sprite = Tile_1;
                    Move_Over_Tile_Name.text = "Grass";
                }
            }
		}
		else
		{
			if (map_ctr.acting_state != 1)
            {
                MouseOver_Char_info.SetActive(false);
                MouseOver_Char_Skill_info.SetActive(false);
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

            if (Characters_clone[i] == null)
                return;

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
							((Characters_clone[i].GetComponent<Makepinggo>().current_health) / (Characters_clone[i].GetComponent<Makepinggo>().health));

                        if ((Characters_clone[i].GetComponent<Makepinggo>().current_health) <= 0)
                        {
                            Char_infos[char_info_slots_dict["Makepinggo"]].transform.Find("Char_Health_bar").Find("Health_number").gameObject.GetComponent<Text>().text =
                                                                          "0" + "/" + (Characters_clone[i].GetComponent<Makepinggo>().health).ToString();
                            return;
                        }
                        Char_infos[char_info_slots_dict["Makepinggo"]].transform.Find("Char_Health_bar").Find("Health_number").gameObject.GetComponent<Text>().text =
							                                              ((Characters_clone[i].GetComponent<Makepinggo>().current_health).ToString() + "/" + (Characters_clone[i].GetComponent<Makepinggo>().health).ToString());                                      
					}
				}
			}

            if (Characters_clone[i].name == "Monk(Clone)")
            {
                GameObject _Fill = Characters_clone[i].transform.Find("health_canvas/Small_Health_bar/fill").gameObject;
                Image _fill_image = _Fill.GetComponent<Image>();
                _fill_image.fillAmount = ((Characters_clone[i].GetComponent<Monk>().current_health) /
                                          (Characters_clone[i].GetComponent<Monk>().health));
                foreach (var item in Char_infos)
                {
                    if (item.active == true)
                    {
                        Char_infos[char_info_slots_dict["Monk"]].transform.Find("Char_Health_bar").Find("Health_FILLImage").gameObject.GetComponent<Image>().fillAmount =
                            ((Characters_clone[i].GetComponent<Monk>().current_health) / (Characters_clone[i].GetComponent<Monk>().health));

                        if ((Characters_clone[i].GetComponent<Monk>().current_health) <= 0)
                        {
                            Char_infos[char_info_slots_dict["Monk"]].transform.Find("Char_Health_bar").Find("Health_number").gameObject.GetComponent<Text>().text =
                                                                          "0" + "/" + (Characters_clone[i].GetComponent<Monk>().health).ToString();
                            return;
                        }
                        Char_infos[char_info_slots_dict["Monk"]].transform.Find("Char_Health_bar").Find("Health_number").gameObject.GetComponent<Text>().text =
                                                                          ((Characters_clone[i].GetComponent<Monk>().current_health).ToString() + "/" + (Characters_clone[i].GetComponent<Monk>().health).ToString());
                        Char_infos[char_info_slots_dict["Monk"]].transform.Find("Char_Attack").Find("Attack_Damage").gameObject.GetComponent<Text>().text = (Characters_clone[i].GetComponent<Monk>().attack_damage).ToString();
                    }
                }
            }

            if (Characters_clone[i].name == "Tauren(Clone)")
            {
                GameObject _Fill = Characters_clone[i].transform.Find("health_canvas/Small_Health_bar/fill").gameObject;
                Image _fill_image = _Fill.GetComponent<Image>();
				_fill_image.fillAmount = ((Characters_clone[i].GetComponent<Tauren>().current_health) /
				                          (Characters_clone[i].GetComponent<Tauren>().health));
				foreach (var item in Char_infos)
                {
                    if (item.active == true)
                    {
                        Char_infos[char_info_slots_dict["Tauren"]].transform.Find("Char_Health_bar").Find("Health_FILLImage").gameObject.GetComponent<Image>().fillAmount =
							                                        ((Characters_clone[i].GetComponent<Tauren>().current_health) / (Characters_clone[i].GetComponent<Tauren>().health));
                        if ((Characters_clone[i].GetComponent<Tauren>().current_health) <= 0)
                        {
                            Char_infos[char_info_slots_dict["Tauren"]].transform.Find("Char_Health_bar").Find("Health_number").gameObject.GetComponent<Text>().text =
                                                                          "0" + "/" + (Characters_clone[i].GetComponent<Tauren>().health).ToString();
                            return;
                        }
                        Char_infos[char_info_slots_dict["Tauren"]].transform.Find("Char_Health_bar").Find("Health_number").gameObject.GetComponent<Text>().text =
							                                        ((Characters_clone[i].GetComponent<Tauren>().current_health).ToString() + "/" + (Characters_clone[i].GetComponent<Tauren>().health).ToString());
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
                {
                    if (item.active == true)
                    {
                        Char_infos[char_info_slots_dict["SwordMan"]].transform.Find("Char_Health_bar").Find("Health_FILLImage").gameObject.GetComponent<Image>().fillAmount =
                                                                      ((Characters_clone[i].GetComponent<SwordMan>().current_health) / (Characters_clone[i].GetComponent<SwordMan>().health));
                        if ((Characters_clone[i].GetComponent<SwordMan>().current_health) <= 0)
                        {
                            Char_infos[char_info_slots_dict["SwordMan"]].transform.Find("Char_Health_bar").Find("Health_number").gameObject.GetComponent<Text>().text =
                                                                          "0" + "/" + (Characters_clone[i].GetComponent<SwordMan>().health).ToString();
                            return;
                        }
                        Char_infos[char_info_slots_dict["SwordMan"]].transform.Find("Char_Health_bar").Find("Health_number").gameObject.GetComponent<Text>().text =
                                                                          ((Characters_clone[i].GetComponent<SwordMan>().current_health).ToString() + "/" + (Characters_clone[i].GetComponent<SwordMan>().health).ToString());
                    }
                }
            }

			if (Characters_clone[i].name == "Archer(Clone)")
            {
                GameObject _Fill = Characters_clone[i].transform.Find("health_canvas/Small_Health_bar/fill").gameObject;
                Image _fill_image = _Fill.GetComponent<Image>();
				_fill_image.fillAmount = ((Characters_clone[i].GetComponent<Archer>().current_health) /
				                          (Characters_clone[i].GetComponent<Archer>().health));
				foreach (var item in Char_infos)
                {
                    if (item.active == true)
                    {
                        Char_infos[char_info_slots_dict["Archer"]].transform.Find("Char_Health_bar").Find("Health_FILLImage").gameObject.GetComponent<Image>().fillAmount =
                                                                  ((Characters_clone[i].GetComponent<Archer>().current_health) / (Characters_clone[i].GetComponent<Archer>().health));
                        if ((Characters_clone[i].GetComponent<Archer>().current_health) <= 0)
                        {
                            Char_infos[char_info_slots_dict["Archer"]].transform.Find("Char_Health_bar").Find("Health_number").gameObject.GetComponent<Text>().text =
                                                                          "0" + "/" + (Characters_clone[i].GetComponent<Archer>().health).ToString();
                            return;
                        }
                        Char_infos[char_info_slots_dict["Archer"]].transform.Find("Char_Health_bar").Find("Health_number").gameObject.GetComponent<Text>().text =
                                                                      ((Characters_clone[i].GetComponent<Archer>().current_health).ToString() + "/" + (Characters_clone[i].GetComponent<Archer>().health).ToString());
                    }
                }
            }
			if (Characters_clone[i].name == "Wuchang(Clone)")
            {
                GameObject _Fill = Characters_clone[i].transform.Find("health_canvas/Small_Health_bar/fill").gameObject;
                Image _fill_image = _Fill.GetComponent<Image>();
				_fill_image.fillAmount = ((Characters_clone[i].GetComponent<Wuchang>().current_health) /
				                          (Characters_clone[i].GetComponent<Wuchang>().health));
                foreach (var item in Char_infos)
                {
                    if (item.active == true)
                    {
						Char_infos[char_info_slots_dict["Wuchang"]].transform.Find("Char_Health_bar").Find("Health_FILLImage").gameObject.GetComponent<Image>().fillAmount =
							                                           ((Characters_clone[i].GetComponent<Wuchang>().current_health) / (Characters_clone[i].GetComponent<Wuchang>().health));
						if ((Characters_clone[i].GetComponent<Wuchang>().current_health) <= 0)
                        {
							Char_infos[char_info_slots_dict["Wuchang"]].transform.Find("Char_Health_bar").Find("Health_number").gameObject.GetComponent<Text>().text =
								                                           "0" + "/" + (Characters_clone[i].GetComponent<Wuchang>().health).ToString();
                            return;
                        }
						Char_infos[char_info_slots_dict["Wuchang"]].transform.Find("Char_Health_bar").Find("Health_number").gameObject.GetComponent<Text>().text =
							                                           ((Characters_clone[i].GetComponent<Wuchang>().current_health).ToString() + "/" + (Characters_clone[i].GetComponent<Wuchang>().health).ToString());
                    }
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

                MouseOver_Skill_CD_Info.transform.Find("Text").GetComponent<Text>().text = ("CD: " + (map_ctr.units_state[map_ctr.picked_pos].gameObject.GetComponent<UserUnit>().coolDown).ToString() + " Turn");

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
