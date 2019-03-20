using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillDmgPowerUp : PowerUp {

    public SkillDmgPowerUp()
    {
        attribute = "skill_damage";
        description = "Increase the skill damage of all units";
        price = 50;
        change_range = new List<float>() { 1.0f, 2.0f };

    }
}
