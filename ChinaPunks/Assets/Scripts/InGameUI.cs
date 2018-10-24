using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InGameUI : MonoBehaviour {

    public Button exit;

    // Use this for initialization
    void Start () {
        Button _exit = exit.GetComponent<Button>();
        _exit.onClick.AddListener(exit_on_click);

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void exit_on_click()
    {
        SceneManager.LoadScene("Player_Lobby");
        Debug.Log("clicked");
    }
}
