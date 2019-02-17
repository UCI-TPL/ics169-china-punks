using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class WorldGenerator : MonoBehaviour
{

    public GameObject map_prefab;
    public Map_Control map_ctr;
    public GameObject UI_prefab;
    InGameUI UI_ctr;
    public GameObject Turn_prefab;
    Turn_Control Turn_ctr;
    public GameObject timeline;
    PlayableDirector director;
    public GameObject WLcheck_prefab;
    WinLoseCheck WLcheck_ctr;

    //public GameObject LevelManager_prefab;
    //LevelManager level_ctr;
    //public GameObject Dialogue_prefab;
    //DialogueManager dialogue_ctr;

    //level design
    public int level;
    [System.Serializable]
    public class Gameobject_Position_prefab
    {
        public GameObject prefab;
        public List<int> positions;
    }

	[System.Serializable]
    public class AI_Position_prefab
    {
        public List<GameObject> AIPrefabs;
        public int generatedNum;
        public List<int> randomBlockPositions;

    }

    public List<Gameobject_Position_prefab> Tiles_prefabs = new List<Gameobject_Position_prefab>();
    public List<Gameobject_Position_prefab> characters_prefab = new List<Gameobject_Position_prefab>();
	public List<AI_Position_prefab> AI_prefabs = new List<AI_Position_prefab>();
    public List<Gameobject_Position_prefab> Block_prefabs = new List<Gameobject_Position_prefab>();

    public GameObject trap_prefab;
    public List<int> trap_positions = new List<int>();
    public GameObject peach_prefab;
    public int peach_pos;
    public GameObject exit_icon_prefab;
    public List<int> exit_pos = new List<int>();




    //level objects
    public GameObject map;
    public GameObject UI;
    public GameObject Turn;
    public GameObject WLcheck;
    public GameObject LevelManager;
    public GameObject Dialogue;
    public List<GameObject> blocks = new List<GameObject>();
    public List<GameObject> AIs = new List<GameObject>();
    public List<GameObject> characters = new List<GameObject>();
    public List<GameObject> traps = new List<GameObject>();
    public GameObject peach;





    // Use this for initialization

    void Awake()
    {
        map = Instantiate(map_prefab);
        UI = Instantiate(UI_prefab);
        Turn = Instantiate(Turn_prefab);
        WLcheck = Instantiate(WLcheck_prefab);



        //LevelManager = Instantiate(LevelManager_prefab);
        //Dialogue = Instantiate(Dialogue_prefab);
        //Dialogue.transform.SetParent(UI.transform);
        //Dialogue.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
        //Debug.Log(UI.GetComponent<RectTransform>().rect);
        //Dialogue.transform.GetChild(0).gameObject.GetComponent<RectTransform>().rect.Set(0,0,UI.GetComponent<Rect>().width,
        //UI.GetComponent<Rect>().height);
        //for (int i = 0; i < Dialogue.transform.childCount;++i)
        //Dialogue.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(UI.GetComponent<RectTransform>().rect.width,UI.GetComponent<RectTransform>().rect.height);
        //Dialogue.transform.localScale = new Vector3(1, 1, 1);

        map_ctr = map.GetComponent<Map_Control>();
        UI_ctr = UI.GetComponent<InGameUI>();
        Turn_ctr = Turn.GetComponent<Turn_Control>();
        WLcheck_ctr = WLcheck.GetComponent<WinLoseCheck>();
        //level_ctr = LevelManager.GetComponent<LevelManager>();
        //dialogue_ctr = Dialogue.GetComponent<DialogueManager>();

        director = timeline.GetComponent<PlayableDirector>();
        PlayableAsset playable = director.playableAsset;
        TimelineAsset timelineAsset = (TimelineAsset)playable;
        TrackAsset UItrack = timelineAsset.GetOutputTrack(6);
        director.SetGenericBinding(UItrack, UI);
        director.Play();

        LevelStart();



        UI.SetActive(false);

    }

    void Start () {


    }
	
	// Update is called once per frame
	void Update () {


	}

	void LevelStart(){


        UI_ctr.map = map;
        UI_ctr.transform.GetChild(0).gameObject.GetComponent<Button>().onClick.AddListener(Turn_ctr.changeRound);
   
        Turn_ctr.map = map;
        Turn_ctr.UI = UI;
        Turn_ctr.endTurnButton = UI.transform.GetChild(0).gameObject;
        Turn_ctr.WinScene = UI.transform.GetChild(1).gameObject;
        Turn_ctr.LoseScene = UI.transform.GetChild(2).gameObject;

        WLcheck_ctr.mc = map_ctr;
        WLcheck_ctr.turn = Turn_ctr;
        WLcheck_ctr.Level = level;
        WLcheck_ctr.timeline = timeline;
        
        //level_ctr.DialogueManager = Dialogue;
        //level_ctr.MapController = map;
        //level_ctr.TurnController = Turn;
        //level_ctr.InGameUI = UI;
        //level_ctr.Level = level;
        

		//StartCoroutine(StartPause());
        

        foreach (Gameobject_Position_prefab GP in Tiles_prefabs){
            GameObject tile;
            for (int i = 0; i < GP.positions.Count; ++i)
            {
                tile = Instantiate(GP.prefab);
				tile.GetComponent<Tile>().currentPos = i;
                tile.GetComponent<Tile>().map_tiles = map;
                tile.GetComponent<Tile>().turn_control = Turn;
				map_ctr.map_tiles[i] = tile;
            }

        }

        foreach (Gameobject_Position_prefab GP in Block_prefabs)
        {
            GameObject block;
            for (int i = 0; i < GP.positions.Count; ++i)
            {
                block = Instantiate(GP.prefab);
                block.GetComponent<Unit>().mc = map_ctr;
                block.GetComponent<Unit>().turn_ctr = Turn_ctr;
                block.GetComponent<Unit>().currentPos = GP.positions[i];
            }

        }

        foreach (int pos in exit_pos)
        {
            map_ctr.map_tiles[pos].GetComponent<Tile>().exit = true;
            GameObject exit_icon = Instantiate(exit_icon_prefab);
            exit_icon.GetComponent<Exit_icon>().map = map;
            exit_icon.GetComponent<Exit_icon>().currentPos = pos;

            WLcheck_ctr.exits.Add(pos);
        }



        //int UI_yPos = -220;
        //foreach (Gameobject_Position_prefab GP in characters_prefab){
        //    GameObject character;
        //    for (int i = 0; i < GP.positions.Count;++i){
        //        character = Instantiate(GP.prefab);
        //        character.GetComponent<UserUnit>().mc = map_ctr;
        //        character.GetComponent<UserUnit>().turn_ctr = Turn_ctr;
        //        character.GetComponent<UserUnit>().currentPos = GP.positions[i];

        //        string character_name = character.name.Remove(character.name.Length-7);
        //        GameObject characterUI = UI.transform.Find(character_name).gameObject;
        //        characterUI.GetComponent<RectTransform>().anchoredPosition = new Vector2(200,UI_yPos);
        //        characterUI.SetActive(true);
        //        character.GetComponent<UserUnit>().healthFillImage = characterUI.transform.Find("Image (1)").Find("Health_FILLImage").gameObject.GetComponent<Image>();
        //    }
        //    UI_yPos -= 140;
        //}

		//UI_yPos = -220;
		//foreach (Gameobject_Position_prefab GP in AI_prefabs)
		//{
		//    GameObject AI;
		//    Vector2 UI_pos = new Vector2(750, 300);
		//    for (int i = 0; i < GP.positions.Count; ++i)
		//    {
		//        AI = Instantiate(GP.prefab);
		//        AI.GetComponent<AIUnit>().mc = map_ctr;
		//        AI.GetComponent<AIUnit>().turn_ctr = Turn_ctr;
		//        AI.GetComponent<AIUnit>().currentPos = GP.positions[i];

		//        string AI_name = AI.name.Remove(AI.name.Length - 7);
		//        GameObject AIUI = UI.transform.Find(AI_name).gameObject;
		//        AIUI.GetComponent<RectTransform>().anchoredPosition = new Vector2(1700,UI_yPos);
		//        AIUI.SetActive(true);
		//        AI.GetComponent<AIUnit>().healthFillImage = AIUI.transform.Find("Image (1)").Find("Health_FILLImage").gameObject.GetComponent<Image>();
		//    }
		//    UI_yPos -= 140;
		//}

		foreach (AI_Position_prefab GP in AI_prefabs)
        {
            GameObject AI;
            List<int> randomPos = new List<int>(GP.randomBlockPositions);
            for (int i = 0; i < GP.generatedNum; ++i)
            {
                AI = Instantiate(GP.AIPrefabs[Random.Range(0, GP.AIPrefabs.Count)]);
                int pos = randomPos[Random.Range(0, randomPos.Count)];
                AI.GetComponent<AIUnit>().mc = map_ctr;
                AI.GetComponent<AIUnit>().turn_ctr = Turn_ctr;
                AI.GetComponent<AIUnit>().currentPos = pos;
                randomPos.Remove(pos);
            }
        }


		//GameObject Peach = Instantiate(peach_prefab);
		//Peach.GetComponent<Peach>().mc = map_ctr;
		//Peach.GetComponent<Peach>().turn_ctr = Turn_ctr;
		//Peach.GetComponent<Peach>().currentPos = peach_pos;


		//foreach(int pos in trap_positions){
		//    GameObject trap = Instantiate(trap_prefab);
		//    trap.GetComponent<trap>().mc = map_ctr;
		//    trap.GetComponent<trap>().pos = pos;
		//    map_ctr.map_tiles[pos].GetComponent<Tile>().trap = trap;
		//}



		foreach (Gameobject_Position_prefab GP in characters_prefab)
        {
            GameObject character;
            for (int i = 0; i < GP.positions.Count; ++i)
            {
                character = Instantiate(GP.prefab);
                character.GetComponent<UserUnit>().mc = map_ctr;
                character.GetComponent<UserUnit>().turn_ctr = Turn_ctr;
                character.GetComponent<UserUnit>().currentPos = GP.positions[i];

                UI_ctr.Characters_clone.Add(character);
                WLcheck_ctr.character_list.Add(character);
            }
        }

    }

	IEnumerator StartPause()
    {
        yield return new WaitForSeconds(1);
    }
}
