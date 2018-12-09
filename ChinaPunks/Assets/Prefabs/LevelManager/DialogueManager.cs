using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DialogueManager : MonoBehaviour {
    public Button continue_btn;
    public List<string> step1;
    public List<string> step2;
    public List<string> step3;
    public List<string> step4;
    public List<string> step5;

    public List<string> step;
    private Text content;
    // Use this for initialization
    void Start () {
        //Button _continue_btn = continue_btn.GetComponent<Button>();
        //_continue_btn.onClick.AddListener(() => ShowNextText());

        content = transform.GetChild(1).GetComponent<Text>();
        step = step1;
    }
	
	// Update is called once per frame
	void Update () {
        if(Input.GetMouseButtonDown(0)){
            ShowNextText();
        }

		
	}

    void ShowNextText(){
        if (step.Count > 0)
        {
            content.text = step[0];
            step.RemoveAt(0);
        }
        else{
            gameObject.SetActive(false);
            content.text = "Hi, It's me again.";
        }
    }

    public void NextStep(){
        if (step1.Count <= 0){
            step = step2;
        }
        if(step2.Count <= 0){
            step = step3;
        }
        if(step3.Count <= 0){
            step = step4;
        }
    }
}
