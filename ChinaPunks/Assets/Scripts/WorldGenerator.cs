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
    //public GameObject timeline;
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
        public bool squareBlock;
        public int bottomLeftPos;
        public int topRightPos;
        public List<int> randomBlockPositions;

    }

	[System.Serializable]
    public class Player_Position_prefab
    {
        public List<GameObject> PlayerPrefabs;
        public int bottomLeftPos;
        public int topRightPos;

    }

    public List<Gameobject_Position_prefab> Tiles_prefabs = new List<Gameobject_Position_prefab>();
	public List<Player_Position_prefab> characters_prefab = new List<Player_Position_prefab>();
	public List<AI_Position_prefab> AI_prefabs = new List<AI_Position_prefab>();
    public List<Gameobject_Position_prefab> Block_prefabs = new List<Gameobject_Position_prefab>();
	public Gameobject_Position_prefab Merchant_prefab;

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


	//used to store Units positions, to avoid generating Units on the same tile
        List<int> unitsPos = new List<int>();
        List<int> enemyBlockPos = new List<int>(); //for exit
        



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

        //director = timeline.GetComponent<PlayableDirector>();
        //PlayableAsset playable = director.playableAsset;
        //TimelineAsset timelineAsset = (TimelineAsset)playable;
        //TrackAsset UItrack = timelineAsset.GetOutputTrack(6);
        //director.SetGenericBinding(UItrack, UI);
        //director.Play();
          
        LevelStart();



        UI.SetActive(false);

    }

    void Start () {
		UI.SetActive(true);

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
        //WLcheck_ctr.timeline = timeline;
        
        //level_ctr.DialogueManager = Dialogue;
        //level_ctr.MapController = map;
        //level_ctr.TurnController = Turn;
        //level_ctr.InGameUI = UI;
        //level_ctr.Level = level;
        

		//StartCoroutine(StartPause());
        

		//generate tiles
        foreach (Gameobject_Position_prefab GP in Tiles_prefabs)
        {
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


		//generate block units
        foreach (Gameobject_Position_prefab GP in Block_prefabs)
        {
            GameObject block;
            for (int i = 0; i < GP.positions.Count; ++i)
            {
                block = Instantiate(GP.prefab);
                block.GetComponent<Unit>().mc = map_ctr;
                block.GetComponent<Unit>().turn_ctr = Turn_ctr;
                block.GetComponent<Unit>().currentPos = GP.positions[i];
                unitsPos.Add(GP.positions[i]);
          
            }

        }

		//generate AI/enemy
		foreach (AI_Position_prefab GP in AI_prefabs)
        {
            GameObject AI;
            List<int> randomPos = new List<int>();
            if (GP.squareBlock)
            {
                KeyValuePair<int, int> startPos = new KeyValuePair<int, int>(Mathf.FloorToInt(GP.bottomLeftPos / map_ctr.map_size), Mathf.FloorToInt(GP.bottomLeftPos % map_ctr.map_size));
                KeyValuePair<int, int> endPos = new KeyValuePair<int, int>(Mathf.FloorToInt(GP.topRightPos / map_ctr.map_size), Mathf.FloorToInt(GP.topRightPos % map_ctr.map_size));
                int height = endPos.Value - startPos.Value + 1;
                int width = endPos.Key - startPos.Key + 1;

                for (int i = 0; i < width; ++i)
                {
                    for (int j = 0; j < height; ++j)
                    {
                        randomPos.Add((startPos.Key + i) * map_ctr.map_size + startPos.Value + j);

                    }
                }

                Debug.Log("height" + height);
                Debug.Log("width" + width);
                for (int i = 0; i < randomPos.Count; ++i)
                    Debug.Log(randomPos[i]);

            }
            else
            {
                randomPos = new List<int>(GP.randomBlockPositions);
            }

            enemyBlockPos.AddRange(randomPos);
            for (int i = 0; i < GP.generatedNum; ++i)
            {
                AI = Instantiate(GP.AIPrefabs[Random.Range(0, GP.AIPrefabs.Count)]);
                int pos = randomPos[Random.Range(0, randomPos.Count)]; ;
                while (unitsPos.Contains(pos))
                    pos = randomPos[Random.Range(0, randomPos.Count)];

                AI.GetComponent<AIUnit>().mc = map_ctr;
                AI.GetComponent<AIUnit>().turn_ctr = Turn_ctr;
                AI.GetComponent<AIUnit>().currentPos = pos;
                randomPos.Remove(pos);
                unitsPos.Add(pos);
            }
        }

		//generate exit
        int exitPos = enemyBlockPos[Random.Range(0, enemyBlockPos.Count)];
		while(unitsPos.Contains(exitPos))
			exitPos = enemyBlockPos[Random.Range(0, enemyBlockPos.Count)];
        map_ctr.map_tiles[exitPos].GetComponent<Tile>().exit = true;
        GameObject exit_icon = Instantiate(exit_icon_prefab);
        exit_icon.GetComponent<Exit_icon>().map = map;
        exit_icon.GetComponent<Exit_icon>().currentPos = exitPos;
        unitsPos.Add(exitPos);
        WLcheck_ctr.exits.Add(exitPos);




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



		foreach (Player_Position_prefab GP in characters_prefab)
        {
            GameObject character;
            List<int> randomPos = new List<int>();
            KeyValuePair<int, int> startPos = new KeyValuePair<int, int>(Mathf.FloorToInt(GP.bottomLeftPos / map_ctr.map_size), Mathf.FloorToInt(GP.bottomLeftPos % map_ctr.map_size));
            KeyValuePair<int, int> endPos = new KeyValuePair<int, int>(Mathf.FloorToInt(GP.topRightPos / map_ctr.map_size), Mathf.FloorToInt(GP.topRightPos % map_ctr.map_size));
            int height = endPos.Value - startPos.Value + 1;
            int width = endPos.Key - startPos.Key + 1;

            for (int i = 0; i < width; ++i)
            {
                for (int j = 0; j < height; ++j)
                {
                    randomPos.Add((startPos.Key + i) * map_ctr.map_size + startPos.Value + j);

                }
            }


            for (int i = 0; i < GP.PlayerPrefabs.Count; ++i)
            {
                character = Instantiate(GP.PlayerPrefabs[i]);
                int pos = randomPos[Random.Range(0, randomPos.Count)];
                while (unitsPos.Contains(pos))
                    pos = randomPos[Random.Range(0, randomPos.Count)];
                character.GetComponent<UserUnit>().mc = map_ctr;
                character.GetComponent<UserUnit>().turn_ctr = Turn_ctr;
                character.GetComponent<UserUnit>().currentPos = pos;
                randomPos.Remove(pos);
                unitsPos.Add(pos);

                UI_ctr.Characters_clone.Add(character);
                WLcheck_ctr.character_list.Add(character);
            }
        }

		// Merchant generating
		GameObject merchant = Instantiate(Merchant_prefab.prefab);
		merchant.GetComponent<Merchant>().mc = map_ctr;
		merchant.GetComponent<Merchant>().turn_ctr = Turn_ctr;
		merchant.GetComponent<Merchant>().currentPos = Merchant_prefab.positions[0];
		merchant.GetComponent<Merchant>().trade_button = UI.transform.Find("TradeButton").gameObject;
		merchant.GetComponent<Merchant>().shop_panel = UI.transform.Find("ShopPanel").gameObject;
        
    }

	IEnumerator StartPause()
    {
        yield return new WaitForSeconds(1);
    }
}
