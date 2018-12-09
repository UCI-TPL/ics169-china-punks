using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {
    public GameObject DialogueManager;
    public GameObject MapController;
    public GameObject TurnController;
    public GameObject InGameUI;
    public GameObject PlayerCharacter1;
    public GameObject PlayerCharacter2;
    public GameObject PlayerCharacter3;
    public GameObject PlayerCharacter4;
    public int Level;

    private DialogueManager dm;
    private Map_Control mc;
    private Turn_Control tc;
    private InGameUI ui;
    private GameObject InGameHUD;

    private int prev_turn_count;
    private int curt_turn_count;

	// Use this for initialization
	void Start () {
        DialogueManager.gameObject.SetActive(true);

        dm = DialogueManager.GetComponent<DialogueManager>();
        mc = MapController.GetComponent<Map_Control>();
        tc = TurnController.GetComponent<Turn_Control>();
        ui = InGameUI.GetComponent<InGameUI>();

        prev_turn_count = 1;
        curt_turn_count = mc.turn_count;

        if (Level == 1)
        {
            InGameHUD = InGameUI.transform.GetChild(4).gameObject;
            InGameHUD.transform.GetChild(0).gameObject.SetActive(false);
            InGameHUD.transform.GetChild(1).gameObject.SetActive(false);
            InGameHUD.transform.GetChild(2).gameObject.SetActive(false);
        }
    }
	
	// Update is called once per frame
	void Update () {
        curt_turn_count = mc.turn_count;

        if (curt_turn_count - prev_turn_count == 1){
            if (dm.step.Count <= 0)
            {
                dm.NextStep();
                DialogueManager.gameObject.SetActive(true);

                if (dm.step.Count <= 0){
                    DialogueManager.gameObject.SetActive(false);
                }
            }
            if (Level == 1)
            {

                if (curt_turn_count == 3)
                {
                    InGameHUD.transform.GetChild(0).gameObject.SetActive(true);
                }
                if (curt_turn_count == 4)
                {
                    InGameHUD.transform.GetChild(2).gameObject.SetActive(true);
                }

            }
            prev_turn_count = curt_turn_count;
        }
	}

}
