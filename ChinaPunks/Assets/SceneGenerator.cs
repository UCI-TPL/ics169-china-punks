using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneGenerator : MonoBehaviour {



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
    public List<int> trap_positions = new List<int>();
    public GameObject peach_prefab;
    public int peach_pos;
    public GameObject exit_icon_prefab;
    public List<int> exit_pos = new List<int>();

    void Awake(){

    }

    void Start () {
		
	}
	
	
	void Update () {
		
	}
}
