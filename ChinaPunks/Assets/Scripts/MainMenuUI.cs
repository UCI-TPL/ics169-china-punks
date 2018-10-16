using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour {

    public string lvName = "SampleScene";
    public Button New_Game_Button;
    public Button Continue_Game_Button;
    public Button Setting_Button;
    public Button CloseSetting_Button;
    public Button Quit_Button;

    public Toggle MusicMute;
    public Slider MusicVol;

    public Toggle SoundMute;
    public Slider SoundVol;

    public GameObject SettingHUD;

    // Use this for initialization
    void Start()
    {
        MusicVol.value = 0.5f;
        SoundVol.value = 0.5f;

        SettingHUD.SetActive(false);

        Button start_btn = New_Game_Button.GetComponent<Button>();
        start_btn.onClick.AddListener(StartOnClick);

        Button setting_btn = Setting_Button.GetComponent<Button>();
        setting_btn.onClick.AddListener(OpenSetting);

        Button Close_setting_btn = CloseSetting_Button.GetComponent<Button>();
        Close_setting_btn.onClick.AddListener(CloseSetting);

        Button quit_btn = Quit_Button.GetComponent<Button>();
        quit_btn.onClick.AddListener(doExitGame);
    }

    // Update is called once per frame
    void Update () {
        if (!MusicMute.isOn)
        {
            AudioListener.volume = MusicVol.value;
            AudioListener.volume = 0;
        }

        if (MusicMute.isOn)
        {
            AudioListener.volume = MusicVol.value;
        }

        //if (SoundMute.isOn)
        //{
        //    AudioListener.volume = SoundVol.value;
        //    AudioListener.volume = 0;
        //}

        //if (!SoundMute.isOn)
        //{
        //    AudioListener.volume = SoundVol.value;
        //}
    }

    void StartOnClick()
    {
        SceneManager.LoadScene(lvName);
    }

    void OpenSetting()
    {
        SettingHUD.SetActive(true);
    }

    void CloseSetting()
    {
        SettingHUD.SetActive(false);
    }

    void doExitGame()
    {
        Debug.Log("quit");
        Application.Quit();
    }
}
