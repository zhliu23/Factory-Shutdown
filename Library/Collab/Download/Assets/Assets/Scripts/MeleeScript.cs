﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeScript : MonoBehaviour {

    public float damage;
    public float knockBack;
    public float knockBackRadius;
    public float meleeRate;

    public Animator ani;

    private float nextMelee;
    private int shootableMask;

    PlayerController player;

	public GameObject robot;
	// Use this for initialization
	void Start () {
        shootableMask = LayerMask.GetMask("Shootable");
        ani = transform.root.GetComponent<Animator>();
        player = transform.root.GetComponent<PlayerController>();
        nextMelee = 0f;
	}
	
	// Update is called once per frame
	void Update () {
        if(Input.GetKeyDown(KeyCode.V) && nextMelee < Time.time && !player.getSprint())
        {
            ani.SetTrigger("Melee");
            nextMelee = Time.time + meleeRate;

			if (robot != null) {
				EnemyHealth enemyHealth = robot.GetComponent<EnemyHealth> ();
				if (enemyHealth != null) {
					enemyHealth.addDamage (10);
				}
			}
            Collider[] attacked = Physics.OverlapSphere(transform.position, knockBackRadius, shootableMask);
        }
	}

	void OnTriggerEnter(Collider c){
		if (c.tag == "RobotEnemy") {
			robot = c.gameObject;
		}
	}
	void OnTriggerExit(Collider c){
		if (c.tag == "RobotEnemy") {
			robot = null;
		}
	}
}
