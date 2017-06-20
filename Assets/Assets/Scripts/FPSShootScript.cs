/***************************************************************
* file: FPSShootScript.cs
* author: Gabriel Talavera, Duy Le, Joshua Chau, Kaythari Phon, Zhen Liu, David Silvan
* class: CS 470.01 - Game Development
*
* assignment: Quarter Project
* date last modified: 5/29/2017
*
* purpose: Handle FPS perspective shooting
*
****************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSShootScript : MonoBehaviour {

    public float shootRate = 0.15f;         //Shoot rate
    public float weaponRange;               //Max range of bullet
    public GameObject bullet;               //Bullet object
    public float speed;                     //Bullet speed

    private float nextShot;                 //Time of the next shot

    private GameObject playerObject;        //Player object.
    private PlayerController player;        //PlayerController object
    private Animator ani;                   //Animator object
    private Camera MainCam, FPSCam;         //Camera objects
    private Vector3 localScale;             //The local scale


    //Audio related variables
    private AudioSource m_AudioSource;
    public AudioClip[] m_GunShotSounds;
    public AudioClip m_SlashSound;

    // Use this for initialization
    void Start () {
        playerObject = GameObject.FindGameObjectWithTag("Player");
        player = playerObject.GetComponent<PlayerController>();
        ani = playerObject.GetComponent<Animator>();
        MainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        FPSCam = transform.parent.parent.parent.GetComponent<Camera>();
        m_AudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void LateUpdate () {

        //If FPS camera is active, go to function that handles FPS shooting.
		if(FPSCam.enabled)
        {
            FPSShoot();
        }
	}

    //Function to handle the FPS shooting perspective
    private void FPSShoot()
    {
        if (Input.GetMouseButtonDown(0) && Time.time > nextShot)
        {
            //Calculate time for the next shot
            nextShot = Time.time + shootRate;

            //Create the bullet
            GameObject projectile = Instantiate(bullet, transform.position, Quaternion.identity);

            //Fetch the rigidbody component from the bullet
            Rigidbody rigid = projectile.GetComponent<Rigidbody>();
            rigid.AddForce(-transform.forward * speed);

            //Play gun shot sound
            PlayGunshotAudio();

            //Destroy the bullet after 3 seconds
            Destroy(projectile, 4f);
            
        }

        //Play melee sound when press V
        if (Input.GetKeyDown(KeyCode.V))
        {
            PlaySlashSound();
        }
    }

    //Play melee sound
    private void PlaySlashSound()
    {
        m_AudioSource.clip = m_SlashSound;
        m_AudioSource.volume =0.50f;
        m_AudioSource.Play();
    }

    //Play gunshot audio
    public void PlayGunshotAudio()
    {
        // pick & play a random gun sound from the array,
        // excluding sound at index 0
        int n = Random.Range(1, m_GunShotSounds.Length);
        m_AudioSource.clip = m_GunShotSounds[n];
        m_AudioSource.volume = 1f;
        m_AudioSource.PlayOneShot(m_AudioSource.clip);
        // move picked sound to index 0 so it's not picked next time
        m_GunShotSounds[n] = m_GunShotSounds[0];
        m_GunShotSounds[0] = m_AudioSource.clip;
    }
}
