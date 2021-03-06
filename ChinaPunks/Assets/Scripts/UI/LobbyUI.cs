﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LobbyUI : MonoBehaviour {

    public string lvName = "SampleScene";

    public GameObject SettingHUD;
    public Button Setting_Button;
    public Button CloseSetting_Button;
    public Slider MusicVol;
    public Slider SoundVol;

    public GameObject UserInfo_HUD;
    public Button UserInfo_Button;
    public Button CloseUserInfo_Button;

    public Button Map_Close_button;
    public GameObject MapSelectHUD;

    public Button Inventory_Open_button;
    public Button Inventory_Close_button;
    public GameObject Inventory_HUD;

    public GameObject ShopHUD;
    public Button Shop_Button;
    public Button CloseShop_Button;

    public GameObject BarrackHUD;
    public Button Barrack_Button;
    public Button CloseBarrack_Button;

    public GameObject RecruitButton;
    public GameObject Completed;

    GameObject database;

    // Use this for initialization
    void Start()
    {
        MusicVol.value = 0.5f;
        SoundVol.value = 0.5f;

        SettingHUD.SetActive(false);

        Button setting_btn = Setting_Button.GetComponent<Button>();
        setting_btn.onClick.AddListener(OpenSetting);

        Button Close_setting_btn = CloseSetting_Button.GetComponent<Button>();
        Close_setting_btn.onClick.AddListener(CloseSetting);

        UserInfo_HUD.SetActive(false);

        MapSelectHUD.SetActive(false);
        Button map_close_btn = Map_Close_button.GetComponent<Button>();
        map_close_btn.onClick.AddListener(MapCloseOnClick);

        database = GameObject.Find("ItemDatabase");
        database.GetComponent<InventoryHUD>().enabled = false;
    }

    // Update is called once per frame
    void Update () {
        AudioListener.volume = MusicVol.value;
        //AudioListener.volume = SoundVol.value;
    }

    public void StartLevel1(){
        SceneManager.UnloadSceneAsync("Player_Camp");
        SceneManager.LoadSceneAsync("Level1");
        //Application.LoadLevel("Level1");
    }

    public void StartLevel2()
    {
        SceneManager.UnloadSceneAsync("Player_Camp");
        SceneManager.LoadSceneAsync("Level2");
        //Application.LoadLevel("Level2");
    }

    public void StartLevel3()
    {
        SceneManager.UnloadSceneAsync("Player_Camp");
        SceneManager.LoadSceneAsync("Level3");
        //Application.LoadLevel("Level3");
    }

    public void StartOnClick()
    {
        SceneManager.LoadScene(lvName);
    }

    public void MapSelectOnClick()
    {
        MapSelectHUD.SetActive(true);
    }

    public void MapCloseOnClick()
    {
        MapSelectHUD.SetActive(false);
    }

    public void OpenSetting()
    {
        SettingHUD.SetActive(true);
    }

    public void CloseSetting()
    {
        SettingHUD.SetActive(false);
    }

    public void OpenUserinfo()
    {
        UserInfo_HUD.SetActive(true);
    }

    public void CloseUserinfo()
    {
        UserInfo_HUD.SetActive(false);
    }

    public void OpenInventory()
    {
        Inventory_HUD.SetActive(true);
        database.GetComponent<InventoryHUD>().enabled = true;
    }

    public void CloseInventory()
    {
        Inventory_HUD.SetActive(false);
    }

    public void OpenShop()
    {
        ShopHUD.SetActive(true);
    }

    public void CloseShop()
    {
        ShopHUD.SetActive(false);
    }

    public void OpenBarrack()
    {
        BarrackHUD.SetActive(true);
    }

    public void CloseBarrack()
    {
        BarrackHUD.SetActive(false);
    }

    public void RecruitClick()
    {
        RecruitButton.SetActive(false);
        Completed.SetActive(true);
    }

    public void doExitGame()
    {
        Debug.Log("quit");
        Application.Quit();
    }
}
