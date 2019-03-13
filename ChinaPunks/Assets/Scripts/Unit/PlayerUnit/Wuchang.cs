using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wuchang : UserUnit{

    public GameObject Wuchang_Effect_Prefab;

    public override void Skill()
    {
        base.Skill();

        foreach (GameObject ob in mc.units_state)
        {
            if (ob != null && ob.tag == "EnemyUnit")
            {
                ob.GetComponent<Unit>().Health_Change(skill_damage);

                Vector3 EffectPos = new Vector3(ob.transform.position.x, ob.transform.position.y, -1);
                GameObject Wuchang_Effect = Instantiate(Wuchang_Effect_Prefab, EffectPos, Quaternion.identity);
                Destroy(Wuchang_Effect, 0.6f);

                turnComplete = true;
                mc.reset();
            }
        }
    }
}
