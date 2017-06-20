/***************************************************************
* file: PlayerHealth.cs
* author: Duy Le
* class: CS 470.01 - Game Development
*
* assignment: Quarter Project
* date last modified: 5/29/2017
*
* purpose: Give player health and functions to take damage
*
****************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour {

    public float fullHP = 100;      //Player's max HP
    private float currentHP;        //Player's current HP

	//method: Start
	//purpose: Used for initialization
	void Start () {
        currentHP = fullHP;
	}
	
	//method: Update
	//purpose: nothing
	void Update () {
		
	}

	//method: OnCollisionEnter
	//purpose: When the Player is hit, decrease their health
	void OnCollisionEnter(Collision col) {
		if (col.gameObject.name == "Dog") {
			addDamage(10);
		}
		else if (col.gameObject.name == "RobotEnemy") {
			addDamage(20);
		}
		else if (col.gameObject.name == "Dragon") {
			addDamage(30);
		}
	}

	//method: addDamage
	//purpose: adds damage to the player and destroys the player if health reaches 0
    public void addDamage(float damage)
    {
        currentHP -= damage;
        if (currentHP <= 0)
            playerDie();
    }

	//method: killEnemy
	//purpose: Destroy the player object
    public void playerDie()
    {
		//Restarts the game when player dies
        Destroy(gameObject);
		Application.LoadLevel(1);
    }
}
