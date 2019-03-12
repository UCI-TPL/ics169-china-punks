using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPowerUp : PowerUp {

    public HealthPowerUp()
    {
        attribute = "health";
        description = "Increase the health points of all units";
        price = 100;
        change_range = new List<float>() { 10f, 50f };

    }
}
