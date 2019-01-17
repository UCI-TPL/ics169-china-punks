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
	bool skipped;
    

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

	public void Skip()
	{
		if (level == 0)
		{
			if (idx < 18)
			{
				idx = 18;
				timeline.GetComponent<PlayableDirector>().time = 56.90;
				skipped = true;


			}
			else
			{
				idx = dialogues.Count;
				skipped = true;
				timeline.GetComponent<PlayableDirector>().time = 82.0;
			}
		}
	}

	void LateUpdate()
    {
        if (skipped)
        {
            if (idx == 18)
                timeline.GetComponent<PlayableDirector>().Pause();
            skipped = false;
        }
    }
   


}
