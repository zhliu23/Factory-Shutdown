/***************************************************************
* file: BulletScript.cs
* author: Gabriel Talavera, Duy Le, Joshua Chau, Kaythari Phon, Zhen Liu, David Silvan
* class: CS 470.01 - Game Development
*
* assignment: Quarter Project
* date last modified: 5/29/2017
*
* purpose: Allows bullet to detect object it collided with.
*
****************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour {

	public float range = 30f; //Bullet travel max travel range.

    public float speed; // Speed of the bullet
    Vector3 prevPosition; //Bullet's previous position in the last rendered fram
	private bool hit = false;

    private void Start()
    {
        //Record the initial bullet position
        prevPosition = transform.position;
    }

	//method: Update
    //purpose: check if the bullet has hit an object, and if that
	//object is an enemy, deal damage to the enemy
    void Update () {
        //Obtain the bullet's traveling direction.
        transform.Translate(0f, 0f, speed * Time.deltaTime);

        //Create an array of RaycastHit to record every single objects the bullet collieded. 
        RaycastHit[] hits = Physics.RaycastAll(new Ray(prevPosition, (transform.position - prevPosition).normalized), (transform.position - prevPosition).magnitude);

		if (hits.Length > 0 && !hit) {
			hit = true;
			GameObject gameObject = hits[0].collider.gameObject;
			Debug.Log (gameObject.name);
			EnemyHealth enemyHealth = gameObject.GetComponent<EnemyHealth> ();
			if (enemyHealth != null) {
				enemyHealth.addDamage (10);
			}
		}
		destroyBullet ();
    }

	//method: destroyBullet
	//purpose: destroys the bullet object after 1 second
	private void destroyBullet() {
		Destroy (gameObject, 1);
	}
}
