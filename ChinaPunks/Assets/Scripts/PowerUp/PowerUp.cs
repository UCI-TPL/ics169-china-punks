using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp
{

    // attribute must be exactly same as the sprite name
    public string attribute;
    public string description;
    public int price;
    public List<float> change_range;

    // Function called when the player character acquire the item
    public void powerUp(UserUnit userUnit)
    {
        int change_number = (int)Mathf.Round(Random.Range(change_range[0], change_range[1]));
        switch (attribute)
        {
            case "health":
                userUnit.current_health += change_number;
                if(userUnit.current_health > userUnit.health) {
                    userUnit.current_health = userUnit.health;
                }
                break;

            case "moveRange":
                userUnit.moveRange += change_number;
                userUnit.view_range += change_number;
                break;

            case "attack_damage":
                userUnit.attack_damage += change_number;
                break;

            case "skill_damage":
                userUnit.skill_damage += change_number;
                break;
        }
    }
}
