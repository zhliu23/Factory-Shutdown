/***************************************************************
* file: PlayerController.cs
* author: Duy Le
* class: CS 470.01 - Game Development
*
* assignment: Quarter Project
* date last modified: 5/29/2017
*
* purpose: Control player's movement mechanic
*
****************************************************************/

using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    private CharacterController cc;

    private float input;
    private float verticalVelocity;
    private float characterHeight;

    //Location variables
	private Vector3 moveVector;
    private Vector3 lastMoveVector;
    private Quaternion originalRotation;

    //Animator control
    public Animator ani;
    public bool walking;

    //Speed, Jump and Gravity control
    public float jumpPower;
    public float walkingJumpPower;
    public float sprintJumpPower;
    public float crouchJumpPower;
    public float speed;
    public float gravity;
    public float sprintSpeed;
    public float crouchSprintSpeed;
    public float walkingSpeed;
    public float sneakSpeed;

    public bool lockJump;

    //Audio
    private AudioSource m_AudioSource;
    [SerializeField] private AudioClip[] m_FootstepSounds;    // an array of footstep sounds that will be randomly selected from.
    [SerializeField] private AudioClip m_JumpSound;           // the sound played when character leaves the ground.
    [SerializeField] private AudioClip m_LandSound;           // the sound played when character touches back on ground.

    //Boolean variables define character's current movement state.
    private bool climb;
	private bool facingForward;
    private bool crouch;
    private bool sprint;
    private bool previouslyJump;
    private float aniOGspeed;

    //FPS perspective must have variables
    private Camera MainCam;
    private Camera FpsCam;
    private CameraFollow camFollow;
    private bool fpsAim;
    private Animator fpsani;

    void Start () {
        cc = GetComponent<CharacterController>();

        //Audio Setup
        m_AudioSource = GetComponent<AudioSource>();

        //animation
        ani = GetComponent<Animator>();
        walking = false;
        aniOGspeed = ani.speed;
        fpsani = GameObject.FindGameObjectWithTag("FPSCharacter").GetComponent<Animator>();

        //Camera Setup
        originalRotation = transform.rotation;
        MainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        FpsCam = transform.Find("FPSCam").GetComponent<Camera>();
        camFollow = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFollow>();
        MainCam.enabled = true;
        FpsCam.enabled = false;

        //Initializing character's various speed, jump power, and other mechanics.
        characterHeight = cc.height;
        walkingJumpPower = 20f;
        sprintJumpPower = 25f;
        crouchJumpPower = 15f;
        gravity = 50f;
        sprintSpeed = 14f;
        crouchSprintSpeed = 10f;
        walkingSpeed = 8f;
        sneakSpeed = 4f;
        crouch = false;
        sprint = false;
        lockJump = true;
        facingForward = true;
        previouslyJump = false;

        //Set speed and jump power to nornal value
        speed = walkingSpeed;
        jumpPower = walkingJumpPower;

		//melee
		//nextMelee = 0f;
    }

    void Update () {
        try
        {
            //Camera settings             
            if (MainCam.enabled && !FpsCam.enabled)
            {
                originalRotation = transform.rotation;
            }
            if (cc.isGrounded && cc.velocity == Vector3.zero)
            {
                if (Input.GetKeyDown(KeyCode.C))
                {
                    MainCam.enabled = !MainCam.enabled;
                    FpsCam.enabled = !FpsCam.enabled;
                    camFollow.enabled = !camFollow.enabled;
                    transform.rotation = originalRotation;
                    cc.transform.rotation = originalRotation;
                    //transform.rotation = Quaternion.Slerp(transform.rotation, originalRotation, Time.time * 0f);
                }
            }
            //If maincam is active, go to function that control side scroll character movement
            if(MainCam.enabled)
            {
                ani = GetComponent<Animator>();
                MovementControl();
            }
            else //Change the animator object to FPS animator object
            {
                ani = fpsani;
            }

        } catch (System.Exception e)
        {
            UnityEngine.Debug.Log(e.ToString());
            System.Diagnostics.Debug.WriteLine(e.ToString());
        }
    }

    //Handle moving and jumping
    private void MovementControl()
    {
        moveVector = Vector3.zero;
        input = Input.GetAxis("Horizontal") * speed;

		//<----animation
		walking = false;

        //Play walking or crouch walking animation
		if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) {
            //walking = true;
            //ani.SetBool("IsWalking", true);
            if(Mathf.Abs(cc.velocity.x) > 0)
            {
                if (crouch)
                {
                    ani.SetBool("IsWalking", false);
                    ani.SetBool("CrouchWalk", true);
                }
                else
                {
                    ani.SetBool("IsWalking", true);
                    ani.SetBool("CrouchWalk", false);
                }
            }
        } else
        {
            ani.SetBool("IsWalking", false);
            ani.SetBool("CrouchWalk", false);
        }

        // Rotate model accordingly         
        if (Input.GetKeyDown (KeyCode.A) && facingForward) {
            transform.Rotate (0.0f, -180.0f, 0.0f);
			facingForward = false;
		}

		if (Input.GetKeyDown (KeyCode.D) && !facingForward) {
            transform.Rotate (0.0f, 180.0f, 0.0f);
			facingForward = true;
		}
        
		//Quits application
		if (Input.GetKeyDown (KeyCode.Escape)) {
			Application.Quit ();
		}

		//Restarts Game
		if( Input.GetKeyDown(KeyCode.R) ){
			Application.LoadLevel(1);
		}


		// Rotate model when aiming
		if (Input.GetMouseButton (1) && !sprint) {
			Vector3 mousePos = Input.mousePosition;
			mousePos.z = 21.91f;

			Vector3 worldPos = Camera.main.ScreenToWorldPoint (mousePos);
			//print ("Mouse Position: " + worldPos.x);
			//print ("Player Position: " + transform.position.x);

			if (worldPos.x < transform.position.x && facingForward) {
				transform.Rotate (0.0f, -180.0f, 0.0f);
				facingForward = false;
			}

			if (worldPos.x > transform.position.x && !facingForward) {
				transform.Rotate (0.0f, 180.0f, 0.0f);
				facingForward = true;
			}
		}
        

        //Character Movement Handle
        if (cc.isGrounded)
        {
            if(previouslyJump == true)
            {
                PlayLandingSound();
            }
            previouslyJump = false;
            climb = true;

			//animation
			ani.SetBool("OnGround",true);
			ani.SetBool ("Jump", false);
            //

            //Crouch Toggle
			if (Input.GetKeyDown (KeyCode.LeftControl)) {
				crouch = !crouch;
				ani.SetBool("Crouch", crouch);
			} 

            /*
			if (crouch && walking) {
				ani.SetBool ("CrouchWalk", true);
			} else {
				ani.SetBool ("CrouchWalk", false);
			}
            */

            //Sprint Toggle
            if (Input.GetKey(KeyCode.LeftShift))
            {
                sprint = true;
            }
            else
            {
                sprint = false;
            }

            //Adjust player's speed according to the player's action
            if (crouch)
            {
                cc.center = new Vector3(cc.center.x, 0, cc.center.z);
                cc.height = characterHeight * 0.85f;
                //animation
                ani.SetBool("Sprint", false);

                //

                //Crouch Sprint Speed
                if (sprint)
                {
                    ani.speed = 2.0f;
                    speed = crouchSprintSpeed;
                }
                //Crouch Speed
                else
                {
                    ani.speed = aniOGspeed;
                    speed = sneakSpeed;
                }
            }
            else if (!crouch)
            {
                cc.center = new Vector3(cc.center.x, -0.5f, cc.center.z);
                cc.height = characterHeight;
                //Sprint Speed
                if (sprint)
                {
                    if(Mathf.Abs(cc.velocity.x) > walkingSpeed)
                    {
                        //animation
                        ani.SetBool("Sprint", true);
                        //
                    }
                    speed = sprintSpeed;
                }
                //walking Speed
                else
                {
                    //animation
                    ani.SetBool("Sprint", false);
                    //
                    speed = walkingSpeed;
                }
            }

            //Jump
            if (Input.GetKeyDown(KeyCode.Space))
            {
                PlayJumpSound();
 				//<----animation
				ani.SetBool ("Jump", true);
				ani.SetBool ("OnGround", false);
                //---->
                previouslyJump = true;
                verticalVelocity = jumpPower;

            }
            moveVector.x = input;
        }
        else
        {
            /*
			//*<----animation
			if (Input.GetKeyUp (KeyCode.LeftShift)) {
				ani.SetBool ("Sprint", false);
                speed = walkingSpeed;
			}
			//---->
            */
            previouslyJump = true;
            verticalVelocity -= gravity * Time.deltaTime;
            if (lockJump)
            {
				moveVector.x = lastMoveVector.x;
            }
            else
            {
                moveVector.x = input;
            }
        }

        moveVector.y = verticalVelocity;
		cc.Move(moveVector * Time.deltaTime);
		lastMoveVector = moveVector;
    }

    //Collision Dectection
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        //When characterController detect collision on its sides
        if(cc.collisionFlags == CollisionFlags.Sides)
        {
            // Wall Jump and Wall Climb mechanic
            if (Input.GetKeyDown(KeyCode.Space))
            {
                //Determine the jump power based on character's current action
                if (crouch && !sprint)
                    jumpPower = crouchJumpPower;
                else if (sprint && !crouch)
                    jumpPower = sprintJumpPower;
                else
                    jumpPower = walkingJumpPower;

                //Wall jump and climb when moving left
                if (lastMoveVector.x > 0)
                {
                    //Wall Jump
                    if (Input.GetKey(KeyCode.A))
                    {
                        moveVector = hit.normal * speed;
                        verticalVelocity = jumpPower;
                    }
                    //Wall Climb
                    if (Input.GetKey(KeyCode.D))
                    {
                        if (climb)
                        {
                            climb = false;
							moveVector.x = hit.normal.x * -speed;
                            verticalVelocity = jumpPower;					
                        }
                    }
					ani.SetBool ("JumpOffRightWall", true);
                }
                //Wall jump and climb when moving right
                else if (lastMoveVector.x < 0)
                {
                    //Wall jump
                    if (Input.GetKey(KeyCode.D))
                    {
                        moveVector = hit.normal * speed;
                        verticalVelocity = jumpPower;
                    }
                    //Wall climb
                    if (Input.GetKey(KeyCode.A))
                    {
                        if (climb)
                        {
                            climb = false;
                            moveVector.x = hit.normal.x * -speed;
                            verticalVelocity = jumpPower;	
                        }
                    }
					ani.SetBool ("JumpOffLeftWall", true);
                }
            }
            speed = walkingSpeed;
            jumpPower = walkingJumpPower;
        }
		//<--animation
		if (!cc.isGrounded && hit.gameObject.tag == "Wall") {
			ani.SetBool ("OnAnyWall", true);
			if (lastMoveVector.x > 0) {
				ani.SetBool ("OnRightWall", true);
			} else if (lastMoveVector.x < 0) {
				ani.SetBool ("OnLeftWall", true);
			}
		} else {
			ani.SetBool ("OnAnyWall", false);
			ani.SetBool ("OnLeftWall", false);
			ani.SetBool ("OnRightWall", false);
			ani.SetBool ("JumpOffRightWall", false);
			ani.SetBool ("JumpOffLeftWall", false);
		}	
		//---->
	}

	void OnTriggerExit(Collider c) {
		if(c.gameObject.tag == "Wall"){
			ani.SetBool ("OnAnyWall", false);	
			ani.SetBool ("JumpOffRightWall", false);
			ani.SetBool ("JumpOffLeftWall", false);
		}

	}
	void OnTriggerEnter(Collider c) {
		if(c.gameObject.tag == "Wall"){
			ani.SetBool ("OnAnyWall", true);
			ani.SetBool ("JumpOffRightWall", false);
			ani.SetBool ("JumpOffLeftWall", false);
		}

	}

    //Return true if player is sprinting
    public bool getSprint()
    {
        return sprint;
    }

    //Return true if player is crouching
    public bool getCrouch()
    {
        return crouch;
    }

    //Return the current player's speed
    public float getSpeed()
    {
        return cc.velocity.x;
    }

    //Return true if the character model is facing right
	public bool isFacingForward() {
		return facingForward;
	}

    //Allows to set the character's facing direction
    public void setFacingForward(bool value)
    {
        facingForward = value;
    }

    //Freeze player's location, used for peeking mechanic
    public void Stop()
    {
        moveVector = Vector3.zero;
        cc.Move(moveVector * Time.deltaTime);
        lastMoveVector = moveVector;
    }

    //Get character's original rotation in space
    public Quaternion getOriginalRotation()
    {
        return originalRotation;
    }

    //Play landing sound
    private void PlayLandingSound()
    {
        m_AudioSource.clip = m_LandSound;
        m_AudioSource.volume = 0.25f;
        m_AudioSource.Play();
    }

    //Play jumping sound
    private void PlayJumpSound()
    {
        m_AudioSource.clip = m_JumpSound;
        m_AudioSource.volume = 0.25f;
        m_AudioSource.Play();
    }

    //Play footsteps sound
    public void PlayFootStepAudio(int action)
    {
        if (!cc.isGrounded)
        {
            return;
        }
        // pick & play a random footstep sound from the array,
        // excluding sound at index 0
        int n = Random.Range(1, m_FootstepSounds.Length);
        m_AudioSource.clip = m_FootstepSounds[n];
        switch(action)
        {
            case 0:
                m_AudioSource.volume = 0.25f;
                break;
            case 1:
                m_AudioSource.volume = 0.5f;
                break;
            case 2:
                m_AudioSource.volume = 1f;
                break;
        }
        m_AudioSource.PlayOneShot(m_AudioSource.clip);
        // move picked sound to index 0 so it's not picked next time
        m_FootstepSounds[n] = m_FootstepSounds[0];
        m_FootstepSounds[0] = m_AudioSource.clip;
    }

    //Handle FPS animation
    private void LateUpdate()
    {
        if(FpsCam.enabled)
        {
            FPSController();
        }
    }

    //return the FPS cam object
    public Camera getFPSCam()
    {
        return FpsCam;
    }

    //Function that control the FPS animation
    private void FPSController()
    {
        //ani.SetBool("Reg_Shoot", false);
        //ani.SetBool("Melee", false);
        if (Input.GetMouseButtonDown(1))
        {
            fpsAim = !fpsAim;
        }
        if(fpsAim)
        {
            ani.SetBool("ADS", true);
            if (Input.GetMouseButtonDown(0))
            {
                ani.SetTrigger("ADS_Shoot");
            }
            if (Input.GetKeyDown(KeyCode.V))
            {
                ani.SetTrigger("Melee");
            }
        }
        else
        {
            ani.SetBool("ADS", false);
            if (Input.GetMouseButtonDown(0))
            {
                ani.SetTrigger("Reg_Shoot");
            }
            if (Input.GetKeyDown(KeyCode.V))
            {
                ani.SetTrigger("Melee");
            }
        }
    }
}