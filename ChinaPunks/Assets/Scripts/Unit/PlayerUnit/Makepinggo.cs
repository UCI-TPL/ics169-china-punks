using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Makepinggo : UserUnit {


    public override void Skill(){
        base.Skill();

        //foreach (int p in mc.expansion_of_tiles[mc.picked_pos])
        //{
        //    mc.map_tiles[p].GetComponent<SpriteRenderer>().color = new Color(0, 0, 200);
        //}

        KeyValuePair<int, int> current_pos = new KeyValuePair<int, int>(mc.picked_pos % mc.map_size, mc.picked_pos / mc.map_size);
        //top boundary & no teammate character
        if (current_pos.Value + 2 < mc.map_size)
        {
            List<int> temp = new List<int>();
            temp.Add(current_pos.Key + (current_pos.Value + 2) * mc.map_size);

            mc.map_tiles[(current_pos.Key + (current_pos.Value + 2) * mc.map_size)].GetComponent<SpriteRenderer>().color = new Color(0, 0, 200);
            mc.skill_tiles.Add((current_pos.Key + (current_pos.Value + 2) * mc.map_size), temp);
        }
        //bottom boundary & no teammate character
        if (current_pos.Value - 2 >= 0)
        {
            List<int> temp = new List<int>();
            temp.Add(current_pos.Key + (current_pos.Value - 2) * mc.map_size);

            mc.map_tiles[(current_pos.Key + (current_pos.Value - 2) * mc.map_size)].GetComponent<SpriteRenderer>().color = new Color(0, 0, 200);
            mc.skill_tiles.Add((current_pos.Key + (current_pos.Value - 2) * mc.map_size), temp);
        }
        //left boundary & no teammate character
        if (current_pos.Key - 2 >= 0)
        {
            List<int> temp = new List<int>();
            temp.Add(current_pos.Key - 2 + current_pos.Value * mc.map_size);

            mc.map_tiles[(current_pos.Key - 2 + current_pos.Value * mc.map_size)].GetComponent<SpriteRenderer>().color = new Color(0, 0, 200);
            mc.skill_tiles.Add((current_pos.Key - 2 + current_pos.Value * mc.map_size), temp);
        }
        //right boundary & no teammate character
        if (current_pos.Key + 2 < mc.map_size)
        {
            List<int> temp = new List<int>();
            temp.Add(current_pos.Key + 2 + current_pos.Value * mc.map_size);

            mc.map_tiles[(current_pos.Key + 2 + current_pos.Value * mc.map_size)].GetComponent<SpriteRenderer>().color = new Color(0, 0, 200);
            mc.skill_tiles.Add((current_pos.Key + 2 + current_pos.Value * mc.map_size), temp);
        }


    }

    public List<int> Attack_range(){
        List<int> range = new List<int>();

        KeyValuePair<int, int> current_pos = new KeyValuePair<int, int>(mc.picked_pos % mc.map_size, mc.picked_pos / mc.map_size);
        //top boundary & no teammate character
        if (current_pos.Value + 2 < mc.map_size)
        {
            range.Add(current_pos.Key + (current_pos.Value + 2) * mc.map_size);

        }
        //bottom boundary & no teammate character
        if (current_pos.Value - 2 >= 0)
        {
            range.Add(current_pos.Key + (current_pos.Value - 2) * mc.map_size);
        }
        //left boundary & no teammate character
        if (current_pos.Key - 2 >= 0)
        {
            range.Add(current_pos.Key - 2 + current_pos.Value * mc.map_size);
        }
        //right boundary & no teammate character
        if (current_pos.Key + 2 < mc.map_size)
        {
            range.Add(current_pos.Key + 2 + current_pos.Value * mc.map_size);
        }


        return range;
    }
}
