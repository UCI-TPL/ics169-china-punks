using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monk : UserUnit {


    private float attack_dmg;
    private int move_range;

    void Start()
    {
        mc.units_state[currentPos] = gameObject;
        mapInfo = mc.map_tiles;                                                          //get map info from GameController
        Vector3 xyPosition = mapInfo[currentPos].transform.position;
        transform.position = new Vector3(xyPosition.x, xyPosition.y + 0.7f, xyPosition.z - 1.0f);

        //store values for Reset_Skill
        attack_dmg = attack_damage;
        move_range = moveRange;
    }

    public override void Skill(){
        //0 ->[+attack,-movement], 1->[-attack,+movement]
        int dice = Random.Range(0, 2);
        //change value
        if (dice == 0)
        {
            attack_damage *= 2;
            moveRange--;
        }
        else if(dice == 1){
            attack_damage /= 2;
            moveRange++;
        }
        turnComplete = true;
        coolDown += skill_cd;
        provocative = true;
        mc.provocative = true;
        mc.reset();

    }
    public override void Reset_Skill()
    {
        base.Reset_Skill();
        attack_damage = attack_dmg;
        moveRange = move_range;
        provocative = false;
        mc.provocative = false;
    }
}
