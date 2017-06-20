using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolNoRangeAttack : MonoBehaviour {
	//Transform array sets up points for the agent to navigate to
	public Transform[] points;
	RaycastHit hit;
	//destination sets up next destination point
	private int destination = 0;
	private UnityEngine.AI.NavMeshAgent agent;
	public float visionDistance = 20;
	private float curTime = 0;
	public float pauseDuration = 4;
	private bool playerSpotted = false;
	//Lower the widthOfVision the wider the field of view
	//Same with heightOfVision
	//The larger the number the narrower the field of view
	public int widthOfVision = 20;
	public float heightOfVision = 10;
	Animator anim;

	//Initialization, sets the Enemy Nav Mesh, And grabs the animator
	void Start() {
		agent = GetComponent<UnityEngine.AI.NavMeshAgent> ();
		agent.autoBraking = true;
		anim = agent.GetComponent<Animator> ();
		GotoNextPoint ();
	}

	//GoToNextPoint Method is used to determine the next location for the enemy
	void GotoNextPoint() {
		//If player isn't seen then the enemy follows its next waypoint
		if (!playerSpotted) {
			//if no more points to navigate to it does nothing
			if (points.Length == 0)
				return;
			//Sets agent to move to the selected destination
			agent.destination = points [destination].position;
			//curTime is used later, resets to 0 here
			curTime = 0;
			//The next point in the array is set up as the destination, cycles to start if needed
			destination = (destination + 1) % points.Length;
		} else {
			//if player is seen then the enemy follows the player
			agent.destination = hit.point;
		}
	}

	void Update(){
		//Draws the ray, (position of enemy, where the ray shoots out from here it is using blue z-axis * distance enemy can
		// see, sets the color, .5f widens the ray a marginal amount. Setting it too high will look weird)
	/*
		if (playerSpotted) {
			Debug.DrawRay (transform.position, transform.forward * visionDistance, Color.red);
			Debug.DrawRay (transform.position, (transform.forward + (transform.up / heightOfVision)).normalized * visionDistance, Color.red);
			Debug.DrawRay (transform.position, (transform.forward - (transform.up / heightOfVision)).normalized * visionDistance, Color.red);
			Debug.DrawRay (transform.position, (transform.forward + (transform.right / widthOfVision)).normalized * visionDistance, Color.red);
			Debug.DrawRay (transform.position, (transform.forward - (transform.right / widthOfVision)).normalized * visionDistance, Color.red);
		} else {
			Debug.DrawRay (transform.position, transform.forward * visionDistance, Color.blue);
			Debug.DrawRay (transform.position, (transform.forward + (transform.up / widthOfVision)).normalized * visionDistance, Color.blue);
			Debug.DrawRay (transform.position, (transform.forward - (transform.up / widthOfVision)).normalized * visionDistance, Color.blue);
			Debug.DrawRay (transform.position, (transform.forward + (transform.right / widthOfVision)).normalized * visionDistance, Color.blue);
			Debug.DrawRay (transform.position, (transform.forward - (transform.right / widthOfVision)).normalized * visionDistance, Color.blue);
		}
		*/
		//If player is spotted using the raycasts it sets the bool playerSpotted to true and marks their position to use
		//In the GoToNextPoint method where the enemy will follow the player
		if ( (Physics.Raycast (transform.position, transform.forward, out hit, visionDistance)) ||
			(Physics.Raycast (transform.position, (transform.forward + (transform.right/widthOfVision)).normalized, out hit, visionDistance)) ||
			(Physics.Raycast (transform.position, (transform.forward - (transform.right/widthOfVision)).normalized, out hit, visionDistance)) ||
			(Physics.Raycast (transform.position, (transform.forward + (transform.up/heightOfVision)).normalized, out hit, visionDistance)) ||
			(Physics.Raycast (transform.position, (transform.forward - (transform.up/heightOfVision)).normalized, out hit, visionDistance))) {
			if (hit.collider.tag == "Player") {
				//Player is found here so enemy will do something
				agent.destination = hit.point;
				playerSpotted = true;
			}
		}
		else {
			playerSpotted = false;
		}
		//as the agent approaches its destination it finds its next destination
		if (agent.remainingDistance < 0.5f) {
			//Since current time was reset to 0
			//It will allow the enemy to idle for a brief moment before proceeding to
			//its next waypoint
			if (curTime == 0) {
				curTime = Time.time;
				//Currently only works for RobotEnemy 
				//To pause for other enemies set the "RobotIdle_001 to what their
				//Idle animation is called
				anim.SetTrigger ("RobotIdle_001");
			} 
			if ((Time.time - curTime) >= pauseDuration) {
				GotoNextPoint ();
			}
		}
	}

}