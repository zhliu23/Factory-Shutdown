/***************************************************************
* file: EnemyHealth.cs
* author: Gabriel Talavera, Duy Le, Joshua Chau, Kaythari Phon, Zhen Liu, David Silvan
* class: CS 470.01 - Game Development
*
* assignment: Quarter Project
* date last modified: 5/29/2017
*
* purpose: Give attached object health and functions to take damage
*
****************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour {

    //Variables to define max health and damage will be taken
    public float enemyMaxHP;
    //public float damageModifer;
    public bool drops;
    public GameObject dropItem;
    public float additionalHP;

    private bool detected;             
    private float currentHP;        //Current HP

	//method: Start
	//purpose: Used for initialization
	void Start () {
        currentHP = enemyMaxHP;
	}

	//method: Update
	//purpose: nothing
	void Update () {
		
	}

    //method: addDamage
	//purpose: adds damage to the object that this script is attached to
	//and destroys the object if the health reaches 0
    public void addDamage(float damage)
    {
        //damage = damage * damageModifer;
        if (damage <= 0f)
        {
            return;
        }
        currentHP -= damage;
		Debug.Log ("Damage added to enemy, currentHP: " + currentHP);
        if(currentHP <= 0)
        {
            killEnemy();
        }
    }

	//method: killEnemy
	//purpose: Destroy the attached object and possibly drop item
    private void killEnemy()
    {
        Destroy(gameObject.transform.root.gameObject);
        if(drops)
        {
            Instantiate(dropItem, transform.position, transform.rotation);
        }

    }
}
