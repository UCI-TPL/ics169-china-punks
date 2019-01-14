using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monk : UserUnit {


    float attack_dmg;
    int move_range;

    public float LostControl_Probability;
    public bool control_lost;


    void Start()
    {
        mc.units_state[currentPos] = gameObject;
        mapInfo = mc.map_tiles;                                                          //get map info from GameController
        Vector3 xyPosition = mapInfo[currentPos].transform.position;
        transform.position = new Vector3(xyPosition.x, xyPosition.y + 0.7f, xyPosition.z - 1.0f);

        //store values for Reset_Skill
        attack_dmg = attack_damage;
        move_range = moveRange;
        current_health = health;
    }

    public override void Skill(){
        ////0 ->[+attack,-movement], 1->[-attack,+movement]
        //int dice = Random.Range(0, 2);
        ////change value
        //if (dice == 0)
        //{
        //    attack_damage *= 2;
        //    moveRange--;
        //}
        //else if(dice == 1){
        //    attack_damage /= 2;
        //    moveRange++;
        //}
        //turnComplete = true;
        //coolDown += skill_cd;
        //provocative = true;
        //mc.provocative = true;
        //mc.reset();


        check_control();

        attack_damage += 3;
        moveRange = 1;
        turnComplete = true;
        coolDown = skill_cd;
        LostControl_Probability += 0.25f;
        mc.reset();



    }
    public override void Reset_Skill()
    {
        base.Reset_Skill();
        attack_damage = attack_dmg;
        moveRange = move_range;
        //provocative = false;
        //mc.provocative = false;
    }

    private void check_control(){
        float value = Random.Range(0f, 1f);
        if (value <= LostControl_Probability)
            control_lost = true;
    }
}
