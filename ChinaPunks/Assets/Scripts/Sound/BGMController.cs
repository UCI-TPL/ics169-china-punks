using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BGMController : MonoBehaviour {

    Scene Current_Scene;
    static BGMController instance;

    public AudioSource _AudioSource;
    public AudioClip _AudioClip_lobby;
    public AudioClip _AudioClip_InGame;

    // Use this for initialization
    void Start () {
		_AudioSource.clip = _AudioClip_InGame;

        _AudioSource.Play();

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if(instance != this)
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update () {
        Current_Scene = SceneManager.GetActiveScene();

        if (Current_Scene.name == "SampleScene")
        {
            if (_AudioSource.clip == _AudioClip_lobby)
            {
                _AudioSource.clip = _AudioClip_InGame;
                _AudioSource.Play();
            }
        }
		if (Current_Scene.name == "Level1")
        {
			//Debug.Log("level !!!1");

			//_AudioSource.clip = _AudioClip_InGame;
            //_AudioSource.Play();
        }
    }
}
