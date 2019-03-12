using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttDmgPowerUp : PowerUp {

    public AttDmgPowerUp() {
        attribute = "attack_damage";
        description = "Increase the attack damage of all units";
        price = 40;
        change_range = new List<float>() { 2.0f, 8.0f };

    }

}
