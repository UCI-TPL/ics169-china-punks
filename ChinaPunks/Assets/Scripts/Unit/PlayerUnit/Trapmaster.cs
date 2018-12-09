using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trapmaster : UserUnit {

    public GameObject trap_prefab;

    public override void Skill(){
        GameObject trap = Instantiate(trap_prefab);
        trap.GetComponent<trap>().pos = currentPos;
        trap.GetComponent<trap>().mc = mc;
        mc.map_tiles[currentPos].GetComponent<Tile>().trap = trap;
        mc.reset();
    }


}
