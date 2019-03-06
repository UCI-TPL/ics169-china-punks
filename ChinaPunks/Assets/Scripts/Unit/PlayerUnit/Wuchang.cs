using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wuchang : UserUnit{

    public override void Skill()
    {
        base.Skill();

        foreach (GameObject ob in mc.units_state)
        {
            if (ob != null && ob.tag == "EnemyUnit")
            {
                ob.GetComponent<Unit>().Health_Change(skill_damage);
            }
        }
    }
}
