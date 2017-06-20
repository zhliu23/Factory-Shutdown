/***************************************************************
* file: WeaponControl.cs
* author: Gabriel Talavera, Duy Le, Joshua Chau, Kaythari Phon, Zhen Liu, David Silvan
* class: CS 470.01 - Game Development
*
* assignment: Quarter Project
* date last modified: 5/29/2017
*
* purpose: Control player's combat mechanic
*
****************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponControl : MonoBehaviour {

	public float shootRate = 0.2f;
    public float weaponRange;
    public float speed;

    //Pistol variable
    private GameObject pistolChild;
    
    //Bullet variable
    public GameObject bullet;

    //Variables for shooting
    private float nextShot;
    private WaitForSeconds shotDuration = new WaitForSeconds(0.07f);
    private LineRenderer laserLine;

	//Adding bones for animation
	public Transform RightArm;
	public Transform RightElbow;
	public Transform RightHand;
	public Transform RightThumb;
	public Transform RightFingers;
	public Transform LeftArm;
	public Transform LeftElbow;
	public Transform LeftHand;
	public Transform LeftThumb;
	public Transform LeftFingers;
	public Transform pistol;
	public Transform knife;
	public Transform Body;

    //Neccesary objects for this code
    private GameObject playerObject;
    private PlayerController player;
    private Animator ani;
    private Camera MainCam, FPSCam;
    private Vector3 localScale;

    //Audio stuff
    private AudioSource m_AudioSource;
    public AudioClip[] m_GunShotSounds;
    public AudioClip m_SlashSound;


    // Use this for initialization
    void Start () {
        m_AudioSource = GetComponent<AudioSource>();

        speed = 1000f;        //Speed of bullet
        nextShot = 0f;
        pistol.parent = LeftThumb;
        knife.parent = RightThumb;
        playerObject = GameObject.FindGameObjectWithTag("Player");
        player = playerObject.GetComponent<PlayerController>();
        ani = playerObject.GetComponent<Animator>();
        MainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        FPSCam = GameObject.FindGameObjectWithTag("FPSCam").GetComponent<Camera>();
        localScale = transform.localScale;

        laserLine = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update () {
        if(MainCam.enabled)
        {
            if (Input.GetKeyDown(KeyCode.V))
            {
                PlaySlashSound();
            }
            if (Input.GetMouseButton(1) && !player.getSprint())
            {
                Shoot();
            }
        }
	}

	//Handle the animation
	void LateUpdate(){
        ani.SetBool("Aim", false);
        if(MainCam.enabled)
        {
            //Player is holding right click and not sprinting = aiming animation play
            if (Input.GetMouseButton(1) && !player.getSprint())
            {
                ani.SetBool("Aim", true);
                Vector3 mousePos, shootDirection;
                mousePos = Input.mousePosition;
                mousePos.z = Camera.main.transform.position.z;
                shootDirection = Camera.main.ScreenToWorldPoint(mousePos);
                shootDirection = (shootDirection - transform.position);
                float rotate = shootDirection.x;
                shootDirection.Normalize();
                float angle = Mathf.Atan2(shootDirection.y, shootDirection.x) * Mathf.Rad2Deg;

                if(player.isFacingForward())
                {
                    if (playerObject.GetComponent<Animator>().GetBool("IsWalking") && !player.getCrouch())
                    {
                        RightArm.transform.localRotation = Quaternion.Euler(new Vector3(180, -angle - 80, 0));
                        LeftArm.transform.localRotation = Quaternion.Euler(new Vector3(180, -angle - 80, 0));
                    }
                    else if (player.getCrouch())
                    {
                        RightArm.transform.localEulerAngles = new Vector3(180, -angle - 90, 0);
                        LeftArm.transform.localEulerAngles = new Vector3(180, -angle - 90, 0);
                    }
                    else
                    {
                        RightArm.transform.localEulerAngles = new Vector3(180, -angle - 65, 0);
                        LeftArm.transform.localEulerAngles = new Vector3(180, -angle - 65, 0);
                    }
                }
                
                else
                {

                    if (playerObject.GetComponent<Animator>().GetBool("IsWalking") && !player.getCrouch())
                    {
                        RightArm.transform.localEulerAngles = new Vector3(180,angle + 105, 0);
                        LeftArm.transform.localEulerAngles = new Vector3(180, angle + 105, 0);
                    }
                    else if (player.getCrouch())
                    {
                        RightArm.transform.localEulerAngles = new Vector3(180, angle + 95, 0);
                        LeftArm.transform.localEulerAngles = new Vector3(180, angle + 95, 0);
                    }
                    else
                    {
                        RightArm.transform.localEulerAngles = new Vector3(180, angle + 120, 0);
                        LeftArm.transform.localEulerAngles = new Vector3(180, angle + 120, 0);
                    }
                }

                RightElbow.transform.localPosition = new Vector3(-1.50f, 0.71f, -0.13f);
                RightElbow.transform.localRotation = Quaternion.Euler(new Vector3(20f, -20f, 77.569f));

                //Arm and pistol will point towards mouse when rightclick is held. 
                /*
                LeftArm.transform.localRotation = Quaternion.Euler (new Vector3 (18.453f, -54.829f*-(shootDirection.y-40), -153.999f));
                //pistol.transform.localRotation = Quaternion.Euler (new Vector3 (62.246f, 289.91f*(shootDirection.y-20), 268.552f));

                RightArm.transform.localPosition = new Vector3 (-1.79f, -0.64f, 0.89f);
                RightArm.transform.localRotation = Quaternion.Euler (new Vector3 (50.371f, -32.423f, 174.502f));

                RightElbow.transform.localPosition = new Vector3 (-1.50f, 0.71f, -0.13f);
                RightElbow.transform.localRotation = Quaternion.Euler (new Vector3 (20f, -20f, 77.569f));

                RightHand.transform.localPosition = new Vector3 (-1f, 0f, 0f);
                RightHand.transform.localRotation = Quaternion.Euler (new Vector3 (-120f, 0f, 0f));

                RightFingers.transform.localPosition = new Vector3 (0f, 0f, 0f);
                RightFingers.transform.localRotation = Quaternion.Euler (new Vector3 (0f, 0f, 0f));

                RightThumb.transform.localPosition = new Vector3 (0f, 0f, 0f);
                RightThumb.transform.localRotation = Quaternion.Euler (new Vector3 (0, 0, 0));

                LeftArm.transform.localPosition = new Vector3 (-1.469f, 1.102f, -0.049f);
    //			LeftArm.transform.localRotation = Quaternion.Euler (new Vector3 (18.453f, -54.829f, -153.999f));

                LeftElbow.transform.localPosition = new Vector3 (-1.82f, 0.1f, 0.23f);
                LeftElbow.transform.localRotation = Quaternion.Euler (new Vector3 (-30.126f, 109.515f, -49.413f));

                LeftHand.transform.localPosition = new Vector3 (-1f, 0f, 0f);
                LeftHand.transform.localRotation = Quaternion.Euler (new Vector3 (145f, 0f, 0f));

                LeftFingers.transform.localPosition = new Vector3 (-0.5f, 0f, 0f);
                LeftFingers.transform.localRotation = Quaternion.Euler (new Vector3 (-190f, -30f, 50f));

                LeftThumb.transform.localPosition = new Vector3 (-0.5f, 0f, 0f);

                LeftThumb.transform.localRotation = Quaternion.Euler (new Vector3 (-100f, -90f, 0f));

                pistol.transform.localPosition = new Vector3 (0.1f, -0.41f, 1.31f);
                pistol.transform.localRotation = Quaternion.Euler (new Vector3 (62.246f, 289.91f, 268.552f));

                knife.transform.localPosition = new Vector3 (-0.96f, -0.43f, -0.18f);
                knife.transform.localRotation = Quaternion.Euler (new Vector3 (-19.329f, 186.652f, -3.369f));
                */
            }
            else
            {
                ani.SetBool("Aim", false);
                pistol.transform.localPosition = new Vector3(-0.59f, 0.91f, -0.62f);
                pistol.transform.localRotation = Quaternion.Euler(new Vector3(23.573f, 159.493f, 0.622f));

                knife.transform.localPosition = new Vector3(-0.88f, -0.01f, -0.32f);
                knife.transform.localRotation = Quaternion.Euler(new Vector3(-50.654f, -190.79f, 34.392f));
            }
        }        
	}

	void OnCollisionEnter() {
		Destroy (gameObject);
	}

    //Handle shooting system
    private void Shoot()
    {
        //To shoot, player must be aiming by holding down right mouse button, and then click to shoot
        if (Input.GetMouseButtonDown(0)  && nextShot < Time.time)
        {
            nextShot = Time.time + shootRate;

            Vector3 mousePos, shootDirection;

            //Fetching mouse psotion in 2D space (screen)
            mousePos = Input.mousePosition;
            mousePos.z = Camera.main.transform.position.z;

            //Convert the 2D position into 3D ingame position
            shootDirection = Camera.main.ScreenToWorldPoint(mousePos);
            shootDirection = (shootDirection - transform.position).normalized;

            //Debug.Log(shootDirection);

            //Create the bullet
            GameObject projectile = Instantiate(bullet, transform.position, Quaternion.identity);

            //Fetch the rigidbody component from the bullet
            Rigidbody rigid = projectile.GetComponent<Rigidbody>();

            //Play gunshot sound
            PlayGunshotAudio();

            //Shoot the bullet toward the direction
            rigid.velocity = new Vector3(-shootDirection.x * speed, -shootDirection.y * speed);

            //Destroy the bullet after 3 seconds
            Destroy(projectile, 4f);
        }
    }

    //Play gunshot audio
    public void PlayGunshotAudio()
    {
        // pick & play a random gun sound from the array,
        // excluding sound at index 0
        int n = Random.Range(1, m_GunShotSounds.Length);
        m_AudioSource.clip = m_GunShotSounds[n];
        m_AudioSource.volume = 0.3f;
        m_AudioSource.PlayOneShot(m_AudioSource.clip);
        // move picked sound to index 0 so it's not picked next time
        m_GunShotSounds[n] = m_GunShotSounds[0];
        m_GunShotSounds[0] = m_AudioSource.clip;
    }

    //Play melee audio
    private void PlaySlashSound()
    {
        m_AudioSource.clip = m_SlashSound;
        m_AudioSource.volume = 0.25f;
        m_AudioSource.Play();
    }
}
