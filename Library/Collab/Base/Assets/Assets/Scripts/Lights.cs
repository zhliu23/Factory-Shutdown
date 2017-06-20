﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lights : MonoBehaviour {
	public Color colorNormal = Color.blue;
	public Color colorPlayerSeen = Color.red;
	public Light lt;
	RaycastHit hit;
	private bool playerSeen;

	// Use this for initialization
	void Start () {
		lt = GetComponent<Light> ();
	}
	
	// Update is called once per frame
	void Update () {
		//Determines if player is spotted
		if ((Physics.Raycast (transform.position, transform.forward, out hit, 20)) ||
		    (Physics.Raycast (transform.position, (transform.forward + (transform.right / 15)).normalized, out hit, 20)) ||
		    (Physics.Raycast (transform.position, (transform.forward - (transform.right / 15)).normalized, out hit, 20)) ||
		    (Physics.Raycast (transform.position, (transform.forward + (transform.up / 15)).normalized, out hit, 20)) ||
		    (Physics.Raycast (transform.position, (transform.forward - (transform.up / 15)).normalized, out hit, 20))) {
			if (hit.collider.tag == "Player") {
				playerSeen = true;
			} else {
				playerSeen = false;
			}
		}
		//If player is seen then the lights change color to red, else turns blue
		if (playerSeen) {
			lt.color = Color.Lerp (colorNormal, colorPlayerSeen, 1f);
		}
		else {
		lt.color = Color.Lerp(colorNormal, colorPlayerSeen, 0f);
		}
	}
}