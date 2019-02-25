using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tauren : UserUnit {



    public override void Skill()
    {
        base.Skill();

        foreach (int p in Skill_range())
        {
            mc.mark_tile[p] = true;
            mc.map_tiles[p].GetComponent<SpriteRenderer>().color = new Color(0, 0, 200);
            mc.skill_tiles.Add(p, new List<int>() {p});
        }


    }

    public List<int> Skill_range()
    {
        List<int> range = new List<int>();

        KeyValuePair<int, int> current_pos = new KeyValuePair<int, int>(mc.picked_pos % mc.map_size, mc.picked_pos / mc.map_size);
        //top boundary & no teammate character
        if (current_pos.Value + 1 < mc.map_size)
        {
            range.Add(current_pos.Key + (current_pos.Value + 1) * mc.map_size);

        }
        //bottom boundary & no teammate character
        if (current_pos.Value - 1 >= 0)
        {
            range.Add(current_pos.Key + (current_pos.Value - 1) * mc.map_size);
        }
        //left boundary & no teammate character
        if (current_pos.Key - 1 >= 0)
        {
            range.Add(current_pos.Key - 1 + current_pos.Value * mc.map_size);
        }
        //right boundary & no teammate character
        if (current_pos.Key + 1 < mc.map_size)
        {
            range.Add(current_pos.Key + 1 + current_pos.Value * mc.map_size);
        }

        return range;
    }

}
