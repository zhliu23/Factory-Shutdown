/***************************************************************
* file: CameraFollow.cs
* author: Gabriel Talavera, Duy Le, Joshua Chau, Kaythari Phon, Zhen Liu, David Silvan
* class: CS 470.01 - Game Development
*
* assignment: Quarter Project
* date last modified: 5/29/2017
*
* purpose: Make the attached camera to follow the target object smoothly
*           Plus the peek mechanic
*
****************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    private float xMax;

    public Transform lookAt;                    //Target object
    public float smoothing;                     //Camera smoothing rate
    public float freelookSpeed;                 //Speed of camera when peeking
    public float freelookHorizontalLimit, freelookVerticalLimit;        //Limit of how far one can peek vertically and horizontally
    public Vector3 targetCamPosition;           //

    private Vector3 offset;
    private float freelookHorizontalRange, freelookVerticalRange;
    private PlayerController cc;

    // Use this for initialization
    private void Start () {
        offset = transform.position - lookAt.position;
        smoothing = 5f;
        freelookSpeed = 1f;
        freelookHorizontalLimit = 30f;
        freelookVerticalLimit = 10f;
        freelookHorizontalRange = 0f;
        freelookVerticalRange = 0f;
        cc = lookAt.GetComponent<PlayerController>();
    }

    private void Update()
    {
        //If user is holding down middle mouse button
        if (Input.GetMouseButton(2))
        {
            if(cc.GetComponent<CharacterController>().isGrounded)
            {
                FreeLook();
            }
            else
            {
                targetCamPosition = lookAt.position + offset;
            }
        }
        //When user release or isn't holding down middle mouse button
        else
        {
            freelookVerticalRange = 0f;
            freelookHorizontalRange = 0f;
            targetCamPosition = lookAt.position + offset;
        }

        //Move camera to target location
        transform.position = Vector3.Lerp(transform.position, targetCamPosition, smoothing * Time.deltaTime);
    }

    private void FreeLook()
    {
        //Freeze character movement
        cc.Stop();
        //Peek left
        if (Input.GetKey(KeyCode.A) && freelookHorizontalRange != -freelookHorizontalLimit)
        {
            targetCamPosition.x -= freelookSpeed;
            freelookHorizontalRange -= freelookSpeed;
        }

        //Peek right
        if (Input.GetKey(KeyCode.D) && freelookHorizontalRange != freelookHorizontalLimit)
        {
            targetCamPosition.x += freelookSpeed;
            freelookHorizontalRange += freelookSpeed;
        }

        //Peek up
        if (Input.GetKey(KeyCode.W) && freelookVerticalRange != freelookVerticalLimit)
        {
            targetCamPosition.y += freelookSpeed;
            freelookVerticalRange += freelookSpeed;
        }

        //Peek down
        if (Input.GetKey(KeyCode.S) && freelookVerticalRange != -freelookVerticalLimit)
        {
            targetCamPosition.y -= freelookSpeed;
            freelookVerticalRange -= freelookSpeed;
        }
    }
}
