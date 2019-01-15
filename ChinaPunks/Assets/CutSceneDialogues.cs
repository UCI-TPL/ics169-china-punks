using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;

public class CutSceneDialogues : MonoBehaviour {

    public List<string> dialogues = new List<string>();
    public string current_dialogue;
    public GameObject timeline;
    public int level;
    int idx;
    

	void Start () {
		
	}
	
	void Update () {
		
	}

    void OnEnable()
    {
        if (idx < dialogues.Count)
        {
            current_dialogue = dialogues[idx];
            idx++;
            gameObject.GetComponent<Text>().text = current_dialogue;
        }
    }

    void OnDisable()
    {
        if ( (idx == 18 && level == 0))
        {
            timeline.GetComponent<PlayableDirector>().Pause();
        }


        

    }

    //void OnDisable()
    //{
    //    if (idx < dialogues.Count - 1)
    //    {
    //        current_dialogue = dialogues[idx];
    //        idx++;
    //    }
    //}


}
