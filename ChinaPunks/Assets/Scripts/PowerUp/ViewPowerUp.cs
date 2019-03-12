using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewPowerUp : PowerUp {

    public ViewPowerUp()
    {
        attribute = "view_range";
        description = "Increase the view range of all units";
        price = 30;
        change_range = new List<float>() { 1.0f, 3.0f };

    }
}
