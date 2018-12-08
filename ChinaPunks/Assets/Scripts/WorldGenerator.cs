using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class WorldGenerator : MonoBehaviour
{

    public GameObject map;
    Map_Control map_ctr;
    public GameObject UI;
    InGameUI UI_ctr;
    public GameObject Turn;
    Turn_Control Turn_ctr;

    //level design
    public int level;
    [System.Serializable]
    public class Gameobject_Position_prefab
    {
        public GameObject prefab;
        public List<int> positions;
    }
    public List<Gameobject_Position_prefab> Tiles_prefabs = new List<Gameobject_Position_prefab>();
    public List<Gameobject_Position_prefab> characters_prefab = new List<Gameobject_Position_prefab>();
    public List<Gameobject_Position_prefab> AI_prefabs = new List<Gameobject_Position_prefab>();
    public List<Gameobject_Position_prefab> Block_prefabs = new List<Gameobject_Position_prefab>();

    public GameObject trap_prefab;
    public List<int> trap_positions;
    public GameObject peach_prefab;
    public int peach_pos;
    public GameObject exit_icon_prefab;
    public int exit_pos;



    //level objects
    public List<GameObject> blocks = new List<GameObject>();
    public List<GameObject> AIs = new List<GameObject>();
    public List<GameObject> characters = new List<GameObject>();
    public List<GameObject> traps = new List<GameObject>();
    public GameObject peach;




    // Use this for initialization

    void Awake()
    {
        map_ctr = map.GetComponent<Map_Control>();
        UI_ctr = UI.GetComponent<InGameUI>();
        Turn_ctr = Turn.GetComponent<Turn_Control>();
        LevelStart();
    }

    void Start () {


    }
	
	// Update is called once per frame
	void Update () {
		
	}

	void LevelStart(){

		StartCoroutine(StartPause());
        


        foreach (Gameobject_Position_prefab GP in Tiles_prefabs){
            GameObject tile;
            for (int i = 0; i < GP.positions.Count; ++i)
            {
                tile = Instantiate(GP.prefab);
                tile.GetComponent<Tile>().currentPos = GP.positions[i];
                tile.GetComponent<Tile>().map_tiles = map;
                tile.GetComponent<Tile>().turn_control = Turn;
                map_ctr.map_tiles[GP.positions[i]] = tile;
            }

        }

        foreach (Gameobject_Position_prefab GP in Block_prefabs)
        {
            GameObject block;
            for (int i = 0; i < GP.positions.Count; ++i)
            {
                block = Instantiate(GP.prefab);
                block.GetComponent<Unit>().currentPos = GP.positions[i];
            }

        }

        map_ctr.map_tiles[exit_pos].GetComponent<Tile>().exit = true;
        GameObject exit_icon = Instantiate(exit_icon_prefab);
        exit_icon.GetComponent<Exit_icon>().map = map;
        exit_icon.GetComponent<Exit_icon>().currentPos = exit_pos;



        int UI_yPos = -220;
        foreach (Gameobject_Position_prefab GP in characters_prefab){
            GameObject character;
            for (int i = 0; i < GP.positions.Count;++i){
                character = Instantiate(GP.prefab);
                character.GetComponent<UserUnit>().mc = map_ctr;
                character.GetComponent<UserUnit>().turn_ctr = Turn_ctr;
                character.GetComponent<UserUnit>().currentPos = GP.positions[i];

                string character_name = character.name.Remove(character.name.Length-7);
                GameObject characterUI = UI.transform.Find(character_name).gameObject;
                characterUI.GetComponent<RectTransform>().anchoredPosition = new Vector2(200,UI_yPos);
                characterUI.SetActive(true);
                character.GetComponent<UserUnit>().healthFillImage = characterUI.transform.Find("Image (1)").Find("Health_FILLImage").gameObject.GetComponent<Image>();
            }
            UI_yPos -= 140;
        }

        UI_yPos = -220;
        foreach (Gameobject_Position_prefab GP in AI_prefabs)
        {
            GameObject AI;
            Vector2 UI_pos = new Vector2(750, 300);
            for (int i = 0; i < GP.positions.Count; ++i)
            {
                AI = Instantiate(GP.prefab);
                AI.GetComponent<AIUnit>().mc = map_ctr;
                AI.GetComponent<AIUnit>().turn_ctr = Turn_ctr;
                AI.GetComponent<AIUnit>().currentPos = GP.positions[i];

                string AI_name = AI.name.Remove(AI.name.Length - 7);
                GameObject AIUI = UI.transform.Find(AI_name).gameObject;
                AIUI.GetComponent<RectTransform>().anchoredPosition = new Vector2(1700,UI_yPos);
                AIUI.SetActive(true);
                AI.GetComponent<AIUnit>().healthFillImage = AIUI.transform.Find("Image (1)").Find("Health_FILLImage").gameObject.GetComponent<Image>();
            }
            UI_yPos -= 140;
        }
            

        GameObject Peach = Instantiate(peach_prefab);
        Peach.GetComponent<Peach>().currentPos = peach_pos;


        foreach(int pos in trap_positions){
            GameObject trap = Instantiate(trap_prefab);
            trap.GetComponent<trap>().pos = pos;
        }


            ////AI unit won't move in level 1
            //map_ctr.AI_units = new List<GameObject>();


        //else if(level == 2){
            //GameObject block = Instantiate(blocks_prefab[0]);

            //block.GetComponent<Unit>().currentPos = 41;
            //blocks.Add(block);

            //block = Instantiate(blocks_prefab[0]);
            //block.GetComponent<Unit>().currentPos = 52;
            //blocks.Add(block);

            //block = Instantiate(blocks_prefab[0]);
            //block.GetComponent<Unit>().currentPos = 53;
            //blocks.Add(block);

            //block = Instantiate(blocks_prefab[0]);
            //block.GetComponent<Unit>().currentPos = 44;
            //blocks.Add(block);

            //block = Instantiate(blocks_prefab[0]);
            //block.GetComponent<Unit>().currentPos = 45;
            //blocks.Add(block);

            //block = Instantiate(blocks_prefab[0]);
            //block.GetComponent<Unit>().currentPos = 46;
            //blocks.Add(block);
            //block = Instantiate(blocks_prefab[0]);
            //block.GetComponent<Unit>().currentPos = 47;
            //blocks.Add(block);

            //block = Instantiate(blocks_prefab[0]);
            //block.GetComponent<Unit>().currentPos = 48;
            //blocks.Add(block);

            //block = Instantiate(blocks_prefab[0]);
            //block.GetComponent<Unit>().currentPos = 39;
            //blocks.Add(block);

            //foreach(int pos in snowTiles){
            //    map_ctr.map_tiles[pos].GetComponent<Tile>().tile_type = "Snow";
            //    map_ctr.map_tiles[pos].GetComponent<SpriteRenderer>().sprite = Resources.LoadAll<Sprite>("Snow")[0];

            //}

            //map_ctr.map_tiles[exit_pos].GetComponent<Tile>().exit = true;
            //Vector3 exit_tile_pos = map_ctr.map_tiles[exit_pos].transform.position;
            //GameObject exit_icon = Instantiate(exit_icon_prefab);
            //exit_icon.transform.position = new Vector3(exit_tile_pos.x, exit_tile_pos.y + 0.5f, exit_tile_pos.z - 1.0f);

            //GameObject AI = Instantiate(AI_prefab[0]);
            //string AI_name = AI.name.Remove(AI.name.Length - 7);
            //GameObject AIUI = UI.transform.Find(AI_name).gameObject;
            //AIUI.GetComponent<RectTransform>().anchoredPosition = new Vector2(750, 300);
            //AIUI.SetActive(true);
            //AI.GetComponent<AIUnit>().healthFillImage = AIUI.transform.Find("Image (1)").Find("Health_FILLImage").gameObject.GetComponent<Image>();
            //AI.GetComponent<AIUnit>().currentPos = 60;

            //AI = Instantiate(AI_prefab[2]);
            //AI_name = AI.name.Remove(AI.name.Length - 7);
            //AIUI = UI.transform.Find(AI_name).gameObject;
            //AIUI.GetComponent<RectTransform>().anchoredPosition = new Vector2(750, 100);
            //AIUI.SetActive(true);
            //AI.GetComponent<AIUnit>().healthFillImage = AIUI.transform.Find("Image (1)").Find("Health_FILLImage").gameObject.GetComponent<Image>();
            //AI.GetComponent<AIUnit>().currentPos = 61;

            //AI = Instantiate(AI_prefab[3]);
            //AI_name = AI.name.Remove(AI.name.Length - 7);
            //AIUI = UI.transform.Find(AI_name).gameObject;
            //AIUI.GetComponent<RectTransform>().anchoredPosition = new Vector2(750, -100);
            //AIUI.SetActive(true);
            //AI.GetComponent<AIUnit>().healthFillImage = AIUI.transform.Find("Image (1)").Find("Health_FILLImage").gameObject.GetComponent<Image>();
            //AI.GetComponent<AIUnit>().currentPos = 62;

            //AI = Instantiate(AI_prefab[1]);
            //AI_name = AI.name.Remove(AI.name.Length - 7);
            //AIUI = UI.transform.Find(AI_name).gameObject;
            //AIUI.GetComponent<RectTransform>().anchoredPosition = new Vector2(750, -300);
            //AIUI.SetActive(true);
            //AI.GetComponent<AIUnit>().healthFillImage = AIUI.transform.Find("Image (1)").Find("Health_FILLImage").gameObject.GetComponent<Image>();
            //AI.GetComponent<AIUnit>().currentPos = 37;





            //GameObject Peach = Instantiate(peach_prefab);
            //Peach.GetComponent<Peach>().currentPos = 50;

            //GameObject character = Instantiate(characters_prefab[0]);
            //string character_name = character.name.Remove(character.name.Length-7);
            //GameObject characterUI = UI.transform.Find(character_name).gameObject;
            //characterUI.GetComponent<RectTransform>().anchoredPosition = new Vector2(-750, 300);
            //characterUI.SetActive(true);
            //character.GetComponent<UserUnit>().healthFillImage = characterUI.transform.Find("Image (1)").Find("Health_FILLImage").gameObject.GetComponent<Image>();
            //character.GetComponent<UserUnit>().currentPos = 84;

            //character = Instantiate(characters_prefab[1]);
            //character_name = character.name.Remove(character.name.Length - 7);
            //characterUI = UI.transform.Find(character_name).gameObject;
            //characterUI.GetComponent<RectTransform>().anchoredPosition = new Vector2(-750, 100);
            //characterUI.SetActive(true);
            //character.GetComponent<UserUnit>().healthFillImage = characterUI.transform.Find("Image (1)").Find("Health_FILLImage").gameObject.GetComponent<Image>();
            //character.GetComponent<UserUnit>().currentPos = 85;


        //}
    }

	IEnumerator StartPause()
    {
        yield return new WaitForSeconds(1);
    }
}
