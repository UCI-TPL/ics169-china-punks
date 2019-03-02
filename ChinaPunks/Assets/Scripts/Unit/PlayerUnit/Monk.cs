using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monk : UserUnit {


    public float dmgUp_by_skill;


    public override void Skill(){
        
        attack_damage += dmgUp_by_skill;
        turnComplete = true;
        coolDown = skill_cd;
        mc.reset();
        GetComponent<Animator>().SetBool("drunk", true);


    }
    public override void Reset_Skill()
    {
        attack_damage = _attack_damage;
        //GetComponent<Animator>().Play("Monk_Idle");
        GetComponent<Animator>().SetBool("drunk", false);
    }

}
