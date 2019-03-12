using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovRangePowerUp : PowerUp {

    public MovRangePowerUp()
    {
        attribute = "moveRange";
        description = "Increase the move range of all units";
        price = 20;
        change_range = new List<float>() { 1.0f, 1.0f };

    }
}
