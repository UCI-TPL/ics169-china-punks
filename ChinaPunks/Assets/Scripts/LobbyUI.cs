using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LobbyUI : MonoBehaviour {

    public string lvName = "SampleScene";
    public Button Start_Game_Button;
    public Button Map_Selection_button;
    public Button Map_Close_button;

    public GameObject MapSelectHUD;

    // Use this for initialization
    void Start()
    {
        MapSelectHUD.SetActive(false);

        Button start_btn = Start_Game_Button.GetComponent<Button>();
        start_btn.onClick.AddListener(StartOnClick);

        Button map_close_btn = Map_Close_button.GetComponent<Button>();
        map_close_btn.onClick.AddListener(MapCloseOnClick);

    }

    // Update is called once per frame
    void Update () {
    }

    public void StartOnClick()
    {
        SceneManager.LoadScene(lvName);
    }

    public void MapSelectOnClick()
    {
        Debug.Log("clicked");
        MapSelectHUD.SetActive(true);
    }

    public void MapCloseOnClick()
    {
        MapSelectHUD.SetActive(false);
    }
}
