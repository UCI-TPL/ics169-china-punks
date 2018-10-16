using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseTileDetection : MonoBehaviour {
    
    private int position;
    public GameObject controller;
    GameController controlInfo;

	// Use this for initialization
	void Start () {
        controlInfo = controller.GetComponent<GameController>();
        position = int.Parse(gameObject.name);
    }
	
	// Update is called once per frame
	void Update () {
        

    }

    private void OnMouseEnter()
    {
        int index = controlInfo.pickedTile.Count;
        if (controlInfo.mouseDown)
        {
            if (!controlInfo.pickedTile.Contains(position))
            {
                if (isConnected(position, controlInfo.pickedTile[index - 1]))
                {
                    controlInfo.pickedTile.Add(position);
                
                }
            }
        }

    }
    private void OnMouseExit()
    {
        
    }

    private void OnMouseDown()
    {
        controlInfo.mouseDown = true;

        if (!controlInfo.pickedTile.Contains(position))
        {
            controlInfo.pickedTile.Add(position);
        }
    }

    private void OnMouseUp()
    {
        if (controlInfo.mouseDown)
        {
            controlInfo.mouseUp = true;
        }
    }

    bool isConnected(int first, int second)
    {
        if (first + 10 == second || first - 10 == second || first + 1 == second || first - 1 == second)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
