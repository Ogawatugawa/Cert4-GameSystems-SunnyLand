using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Prime31;

[RequireComponent(typeof(CharacterController2D))]
public class Player : MonoBehaviour
{
    public float movementSpeed = 10f;
    public float jumpHeight = 10f;
    public float gravity = -10f;
    public bool IsRight = true;
    public bool IsJumping = false;
    public SpriteRenderer rend;
    public CharacterController2D controller;
    public Animator anim;

    public Vector3 motion; // Store the difference in movement
    private void Reset()
    {
        controller = GetComponent<CharacterController2D>();
        anim = GetComponent<Animator>();
        rend = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // Get horizontal and vertical input
        float inputH = Input.GetAxis("Horizontal");
        float inputV = Input.GetAxis("Vertical");

        // Apply gravity
        motion.y += gravity * Time.deltaTime;
        // If player is standing on ground
        if (controller.isGrounded)
        {
            // Set downward movement to 0 
            motion.y = 0f;
            anim.SetBool("IsJumping", false);

            // If we press Jump
            if (Input.GetButtonDown("Jump"))
            {
                // Run Jump()
                Jump();
            }
        }

        if (anim.GetBool("IsJumping"))
        {
            anim.SetFloat("JumpY", motion.y);
        }

        // Climb up or down depending on Y value
        Climb(inputV);
        // Move left or right depending on X value
        Move(inputH);

        Crouch(inputV);

        // Move using Character Controller 2D
        controller.Move(motion * Time.deltaTime);

        //Animation();
    }

    public void Move(float inputH)
    {
        // Generate movement for left/right
        motion.x = inputH * movementSpeed;
        anim.SetBool("IsRunning", inputH != 0);
        rend.flipX = inputH < 0;
    }

    public void Climb(float inputV)
    {

    }

    public void Hurt()
    {

    }

    public void Jump()
    {
        motion.y += jumpHeight;
        anim.SetBool("IsJumping", true);
    }

    public void Crouch(float inputV)
    {
        if (!anim.GetBool("IsRunning"))
        {
            anim.SetBool("IsCrouching", inputV < 0); 
        }
    }

    public void Animation ()
    {
        float inputH = Input.GetAxis("Horizontal");

        if (inputH != 0)
        {
            if (inputH > 0)
            {
                IsRight = true;
            }

            else
            {
                IsRight = false;
            }
        }

        if (!IsRight)
        {
            rend.flipX = true;
        }

        else
        {
            rend.flipX = false;
        }

    }
}
