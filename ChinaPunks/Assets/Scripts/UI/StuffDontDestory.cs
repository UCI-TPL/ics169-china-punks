using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StuffDontDestory : MonoBehaviour {

    Scene Current_Scene;
    static StuffDontDestory instance;
	// Use this for initialization
	void Start () {
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
        if (Current_Scene.name == "ToBeContinue")
        {
            Destroy(gameObject);
        }
        Debug.Log(Current_Scene.name);
    }
}
