using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {
    public List<GameObject> mapInfo = new List<GameObject>(100);

    public List<int> pickedTile = new List<int>(100);
    public int tileIndex = 0;
    public List<int> unitPos = new List<int>();

    public bool mouseDown;
    public bool mouseUp;
    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

        if (pickedTile.Count > 0 && unitPos.Contains(pickedTile[0]))
        {
            foreach (int a in pickedTile)
            {
                mapInfo[a - 1].GetComponent<SpriteRenderer>().color = new Color(0, 200, 0);
            }
        }
        else{
            pickedTile.Clear();
            mouseDown = false;
        }

        if (mouseUp)
        {
            mouseDown = false;

            foreach (int a in pickedTile)
            {
                mapInfo[a - 1].GetComponent<SpriteRenderer>().color = new Color(255, 255, 255);
            }
        }
    }
}
