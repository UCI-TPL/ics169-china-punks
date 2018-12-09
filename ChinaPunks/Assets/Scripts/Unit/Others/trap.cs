using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trap : MonoBehaviour {


    public int pos;
    public bool triggered;
    public bool fading;
    public int visible_time;
    public Map_Control mc;

    void Start () {
        set_trap();                                            
    }
	
	// Update is called once per frame
	void Update () {
        if(visible_time == 0 && !fading){
            visible_time = -1;
            fading = true;
        }

        if(triggered && !fading){
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
            fading = true;
        }

        if(fading){
            if (gameObject.GetComponent<SpriteRenderer>().color.a > 0f)
            {
                gameObject.GetComponent<SpriteRenderer>().color =
                    new Color(1f, 1f, 1f, gameObject.GetComponent<SpriteRenderer>().color.a - 0.01f);
            }
            else if (triggered)
                Destroy(gameObject);
            else{
                fading = false;
            }
        }

	}

    public void set_trap(){
        //GameObject map = GameObject.Find("map_tiles");
        //mc = map.GetComponent<Map_Control>();
        Vector3 xyPosition = mc.map_tiles[pos].transform.position;
        transform.position = new Vector3(xyPosition.x, xyPosition.y + 0.4f, xyPosition.z - 1.0f);
    }
}
