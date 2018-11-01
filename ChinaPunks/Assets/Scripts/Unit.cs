using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour {
    public Map_Control mc;
    public List<GameObject> mapInfo = new List<GameObject>();

    public int currentPos;
    public int moveRange;

    public int health;
    public int attack_damge;

    public Animator anim;

    //Check if a unit has complete its turn
    public bool moveComplete;
    public bool turnComplete;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public virtual void TurnUpdate(){

    }

    public virtual void Health_Change(int damage){
        anim.Play("Attacked");
        health -= damage;
        Debug.Log("Ahhhh, damage taken: " + damage.ToString());

        if (health <= 0){
            Debug.Log(this.gameObject.name + " is Dead!");
            Destroy(this.gameObject);
        }
    }
}
