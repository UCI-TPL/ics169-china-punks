using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Tile : MonoBehaviour {

    public bool exit;
    public GameObject map_tiles;
    Map_Control map_ctr;

    public GameObject turn_control;
    Turn_Control turn_ctr;
    public int currentPos;

    public string tile_type;
    public bool on_fire;
    public int fire_cd;
    int _fire_cd;
    public GameObject trap;

    //UI
    InGameUI UI_ctr;

    // UI codes which should not be here
 //   public GameObject Enemy_info_HUD;
	//public GameObject Char_detailed_info_hud;
 //   Text _enemy_name;
	//Text _char_detail_info;

	// Use this for initialization
	void Start () {
        _fire_cd = fire_cd;
        map_ctr = map_tiles.GetComponent<Map_Control>();
        turn_ctr = turn_control.GetComponent<Turn_Control>();

        KeyValuePair<float, float> XYpos = new KeyValuePair<float, float>(currentPos % map_ctr.map_size,
                                                                         currentPos / map_ctr.map_size);
        transform.position = new Vector3(-5f+ XYpos.Key * 0.5f + XYpos.Value * 0.5f,
                                         0 + XYpos.Key * (0.25f) + XYpos.Value * (-0.25f),
                                         0 + XYpos.Key * (0.01f) + XYpos.Value * (-0.01f));

		//GameObject IngameUI = turn_ctr.UI.gameObject;

		//GameObject MoveOver_Char_info = IngameUI.gameObject.transform.FindChild


		//      GameObject selected_Enemy_name = Enemy_info_HUD.transform.Find("Enemy_avatar_back/Text").gameObject;
		//      _enemy_name = selected_Enemy_name.GetComponent<Text>();
		//      Enemy_info_HUD.SetActive(false);

		//GameObject Mouse_over_Char_name = Char_detailed_info_hud.transform.Find("Char_avatar_back/Text").gameObject;
		//_char_detail_info = Mouse_over_Char_name.GetComponent<Text>();
		//Char_detailed_info_hud.SetActive(false);

		UI_ctr = turn_control.GetComponent<Turn_Control>().UI.GetComponent<InGameUI>();
    }
	
	// Update is called once per frame
	void Update () {

    }

    private void OnMouseDown()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            //it's player's round
            if (turn_ctr.gameRound == "Player" && !map_ctr.character_moving)
               {
                map_ctr.pickTile = gameObject;
                map_ctr.tile_picked = true;
                //current tile has player unit, so show playerHUD
                if (map_ctr.units_state[map_ctr.map_tiles_pos[gameObject]] != null &&
                    map_ctr.units_state[map_ctr.map_tiles_pos[gameObject]].tag == "PlayerUnit" &&
                    !map_ctr.units_state[map_ctr.map_tiles_pos[gameObject]].GetComponent<UserUnit>().turnComplete)
                {
                    //playerHUD_showed = true;
                    map_ctr.playerHUD_showed = true;

                }
            }

        }
    }

    //for monk skill range showing
    private void OnMouseEnter()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            //show selection outline
            transform.GetChild(0).gameObject.SetActive(true);


            if (map_ctr.acting_state == 4 && map_ctr.skill_tiles.ContainsKey(map_ctr.map_tiles_pos[gameObject]))
            {
                map_ctr.BGCurve = Instantiate(map_ctr.BGCurve_prefab);
                map_ctr.color_skill_tiles(map_ctr.map_tiles_pos[gameObject]);
                Vector3 start_pos = map_ctr.units_state[map_ctr.picked_pos].transform.position;
                Vector3 end_pos = new Vector3(transform.position.x, transform.position.y + 0.4f, transform.position.z - 1.0f);


                BansheeGz.BGSpline.Curve.BGCurvePoint point1 = map_ctr.BGCurve.GetComponent<BansheeGz.BGSpline.Curve.BGCurve>().CreatePointFromWorldPosition(
                    start_pos, (BansheeGz.BGSpline.Curve.BGCurvePoint.ControlTypeEnum)1);
                BansheeGz.BGSpline.Curve.BGCurvePoint point2 = map_ctr.BGCurve.GetComponent<BansheeGz.BGSpline.Curve.BGCurve>().CreatePointFromWorldPosition(
                    end_pos, (BansheeGz.BGSpline.Curve.BGCurvePoint.ControlTypeEnum)1);
                point1.ControlFirstLocal = new Vector3(0f, 0f);
                point1.ControlSecondLocal = new Vector3(0f, 0f);
                if (start_pos.x > end_pos.x)
                {
                    point2.ControlFirstLocal = new Vector3(0.5f, 4f);
                    point2.ControlSecondLocal = new Vector3(-0.5f, -4f);
                }
                else
                {
                    point2.ControlFirstLocal = new Vector3(-0.5f, 4f);
                    point2.ControlSecondLocal = new Vector3(0.5f, -4f);
                }


                map_ctr.BGCurve.GetComponent<BansheeGz.BGSpline.Curve.BGCurve>().AddPoint(point1);
                map_ctr.BGCurve.GetComponent<BansheeGz.BGSpline.Curve.BGCurve>().AddPoint(point2);
                map_ctr.BGCurve.GetComponent<BansheeGz.BGSpline.Components.BGCcTrs>().Distance = 0;

                map_ctr.BGCurve.SetActive(true);

            }
        }
    }

    private void OnMouseExit()
    {
        //clear mouse over info
		UI_ctr.MoveOver_Map_Info.Clear();

		//Check mouse is on map
		UI_ctr.mouse_is_on_map = false;

        //hide selection outline
        transform.GetChild(0).gameObject.SetActive(false);

        if (map_ctr.acting_state == 4)
        {
            map_ctr.uncolor_skill_tiles(map_ctr.map_tiles_pos[gameObject]);
            if (map_ctr.BGCurve != null)
            {
                map_ctr.BGCurve.GetComponent<BansheeGz.BGSpline.Curve.BGCurve>().Clear();
                map_ctr.BGCurve.SetActive(false);
                Destroy(map_ctr.BGCurve);
            }
        }

    }

    private void OnMouseOver()
	{
		//Check mouse is on map
		UI_ctr.mouse_is_on_map = true;

		if (map_ctr.units_state[map_ctr.map_tiles_pos[gameObject]] != null)
        {
			if(UI_ctr.MoveOver_Map_Info.Count == 0)
			{
                UI_ctr.MoveOver_Map_Info.Add(map_ctr.units_state[map_ctr.map_tiles_pos[gameObject]]);
			}

			//Char_detailed_info_hud.SetActive(true);
			//_char_detail_info.text = map_ctr.units_state[map_ctr.map_tiles_pos[gameObject]].name;
        }
    }

    public void update_fire(){
        if(on_fire){
            if (fire_cd != 0)
                fire_cd--;
            else
            {
                on_fire = false;
                gameObject.GetComponent<SpriteRenderer>().sprite = Resources.LoadAll<Sprite>("LavaStone")[0];
                fire_cd = _fire_cd;
            }
        }
        
    }
}
