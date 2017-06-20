/***************************************************************
* file: EnemyDamage.cs
* author: Gabriel Talavera, Duy Le, Joshua Chau, Kaythari Phon, Zhen Liu, David Silvan
* class: CS 470.01 - Game Development
*
* assignment: Quarter Project
* date last modified: 5/29/2017
*
* purpose: Give the attached object ability to deal damage
*
****************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour {


    public float damage;
    public float damageRate;
    public float pushbackForce;

    private float nextDamage;
    private bool playerInRange;

    GameObject player;
    PlayerHealth playerHP;

	//method: Start
	//purpose: Used for initialization of variables
	void Start () {
        nextDamage = Time.time;
        player = GameObject.FindGameObjectWithTag("Player");
        playerHP = player.GetComponent<PlayerHealth>();
	}
	
	//method: Update
	//purpose: Have the enemy attack if the player is in range
	void Update () {
		if(playerInRange)
        {
            Attack();
        }
	}

	//method: Attack
	//purpose: Attack the collided object with damage and rate based on defined variables
    private void Attack()
    {
        if(nextDamage <= Time.time)
        {
            playerHP.addDamage(damage);
            nextDamage = Time.time + damageRate;

            pushBack(player.transform);
        }
    }

	//method: pushBack
	//purpose: Pushback collided object when attacking
    private void pushBack(Transform pushedObject)
    {
        Vector3 pushDirection = new Vector3(0, (pushedObject.position.y - transform.position.y), 0).normalized;
        pushDirection *= pushbackForce;

        Rigidbody pushedRB = pushedObject.GetComponent<Rigidbody>();
        pushedRB.velocity = Vector3.zero;
        pushedRB.AddForce(pushDirection, ForceMode.Impulse);
    }

	//method: OnTriggerEnter
	//purpose: When the player collides with the enemy, set the player
	//to be in range
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            playerInRange = true;
        }
    }

	//method: OnTriggerExit
	//purpose: When the player is no longer colliding with the enemy, set the player
	//to be out of range
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            playerInRange = false;
        }
    }
}
