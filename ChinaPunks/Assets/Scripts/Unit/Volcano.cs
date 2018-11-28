using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Volcano : Unit {

    public int SprayCD;
    int turn;
    public GameObject fire_prefab;
    public bool fire_moving;

    public GameObject fire1;
    public GameObject fire2;


    void Update()
    {
        if(fire_moving){
            if (!fire1.GetComponent<Fire>().fire_moving && !fire2.GetComponent<Fire>().fire_moving)
                fire_moving = false;
        }

    }

    public void Spray(){
        List<int> allowed_tiles = new List<int>();
        for (int i = 0; i < mc.map_size * mc.map_size;++i){
            if(mc.units_state[i] == null
               && mc.map_tiles[i].GetComponent<Tile>().tile_type != "Snow"
               && mc.map_tiles[i].GetComponent<Tile>().tile_type != "Muddy"
              ){
                allowed_tiles.Add(i);
            }
        }

        int index1 = Random.Range(0, allowed_tiles.Count);
        int f1 = allowed_tiles[index1];
        allowed_tiles.Remove(f1);
        int index2 = Random.Range(0, allowed_tiles.Count);
        int f2 = allowed_tiles[index2];

        mc.map_tiles[f1].GetComponent<Tile>().on_fire = true;
        mc.map_tiles[f2].GetComponent<Tile>().on_fire = true;


        fire1 = Instantiate(fire_prefab,transform.position,transform.rotation);
        fire1.GetComponent<Fire>().pos = f1;

        fire2 = Instantiate(fire_prefab, transform.position, transform.rotation);
        fire2.GetComponent<Fire>().pos = f2;

        
        fire1.GetComponent<Fire>().fire_move();
        fire2.GetComponent<Fire>().fire_move();
        fire_moving = true;


    }

}
