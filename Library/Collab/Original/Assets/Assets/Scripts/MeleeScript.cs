/***************************************************************
* file: MeleeScript.cs
* author: Gabriel Talavera, Duy Le, Joshua Chau, Kaythari Phon, Zhen Liu, David Silvan
* class: CS 470.01 - Game Development
*
* assignment: Quarter Project
* date last modified: 5/29/2017
*
* purpose: Handle the melee attack of player
*
****************************************************************/
using System.Collections;
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

            Collider[] attacked = Physics.OverlapSphere(transform.position, knockBackRadius, shootableMask);
        }
	}
}
