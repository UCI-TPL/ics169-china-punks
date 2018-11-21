using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour {

    public string lvName;
    public Button Quit_Button;

    // Use this for initialization
    void Start()
    {
        Button quit_btn = Quit_Button.GetComponent<Button>();
        quit_btn.onClick.AddListener(doExitGame);
    }

    // Update is called once per frame
    void Update () {
        AudioListener.volume = 0.5f;
        if (Input.anyKey)
        {
            SceneManager.LoadScene(lvName);
        }
    }

    void doExitGame()
    {
        Debug.Log("quit");
        Application.Quit();
    }
}
