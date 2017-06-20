using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponControl : MonoBehaviour {

	public float shootRate = 0.2f;
	public GameObject bullet;
    public float speed;

	float nextShot;

	// Use this for initialization
	void Start () {
        speed = 1000f;        //Speed of bullet
        nextShot = 0f;
	}
	
	// Update is called once per frame
	void Update () {
		PlayerController player = transform.root.GetComponent<PlayerController> ();

        //To shoot, player must be aiming by holding down right mouse button, and then click to shoot
		if (Input.GetMouseButtonDown (0) && Input.GetMouseButton(1) && nextShot < Time.time) {

            Vector3 mousePos, shootDirection;

            //Fetching mouse psotion in 2D space (screen)
            mousePos = Input.mousePosition;
            mousePos.z = Camera.main.transform.position.z;

            //Convert the 2D position into 3D ingame position
            shootDirection = Camera.main.ScreenToWorldPoint(mousePos);
            shootDirection = (shootDirection - transform.position).normalized;

            Debug.Log(shootDirection);

            //Create the bullet
			GameObject projectile = Instantiate (bullet, transform.position, Quaternion.identity);

            //Fetch the rigidbody component from the bullet
            Rigidbody rigid = projectile.GetComponent<Rigidbody>();

            //Shoot the bullet toward the direction
            rigid.velocity = new Vector3(-shootDirection.x * speed, -shootDirection.y * speed);

            //Destroy the bullet after 3 seconds
            Destroy(projectile, .5f);
		}
	}

	void OnCollisionEnter() {
		Destroy (gameObject);
	}
}
