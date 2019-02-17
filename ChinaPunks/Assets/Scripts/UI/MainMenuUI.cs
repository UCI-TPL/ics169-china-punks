using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour {
	
    public Button Quit_Button;
	private string lvName;

    // Use this for initialization
    void Start()
    {
        Button quit_btn = Quit_Button.GetComponent<Button>();
        quit_btn.onClick.AddListener(doExitGame);
		lvName = "TowerBase";
    }

    // Update is called once per frame
    void Update () {
        AudioListener.volume = 0.5f;
    }

    void doExitGame()
    {
        Debug.Log("quit");
        Application.Quit();
    }

    // Function for the start button to load the vase scene
	public void EnterBase()
	{
		SceneManager.LoadScene(lvName);
	}
}
