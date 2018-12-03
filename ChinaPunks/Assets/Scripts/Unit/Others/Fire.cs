using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour {
    
    public int pos;
    public int fire_speed;
    public bool fire_moving;
    Map_Control mc;
    Vector3 des;
    Vector3 sta;

    //float xy_time = 2.0f;
    //float dy = 0.4f;
    //float x_speed;
    //float y_speed;

    bool inv;

	// Use this for initialization
	void Start () {

    }
	
	// Update is called once per frame
	void Update () {
        if (fire_moving)
        {
            //float step = fire_speed * Time.deltaTime;
            //transform.position = Vector3.MoveTowards(transform.position, des, step);


            if (Vector3.Distance(transform.position,des) < 0.05f)
            {

                Debug.Log("Fire");
                fire_moving = false;
                if (mc.map_tiles[pos].GetComponent<Tile>().tile_type == "Hide")
                {
                    mc.map_tiles[pos].transform.GetChild(0).gameObject.SetActive(false);
                }
                mc.map_tiles[pos].GetComponent<Tile>().tile_type = "";
                mc.map_tiles[pos].GetComponent<SpriteRenderer>().sprite = Resources.LoadAll<Sprite>("Lava")[0];
                Destroy(gameObject);
            }

        }

	}

    public void fire_move(){
        GameObject map = GameObject.Find("map_tiles");
        mc = map.GetComponent<Map_Control>();
        Vector3 xyPosition = mc.map_tiles[pos].transform.position;
        des = new Vector3(xyPosition.x, xyPosition.y + 0.7f, xyPosition.z - 1.0f);

        fire_moving = true;

        
        //iTween.MoveTo(gameObject, des, 2);
    }
}
