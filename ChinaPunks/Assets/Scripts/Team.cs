using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This is the class that holds the status of the player team. It will carry these states across multitple scene
public static class Team{
    
	static int money = 0;
	static List<GameObject> team = new List<GameObject>();
    
    /* Function that change the number of money
     * Input is the number of change
     */
	public static void change_money(int change){
		money += change;
	}

    /* Function that is called when the player has chosen the team member
     * Input is the name of the character that will be added to the current team build 
     */
	public static void add_member(string name){
		GameObject character = Resources.Load<GameObject>("Characters/" + name);
		team.Add(character);
	}
    
}
