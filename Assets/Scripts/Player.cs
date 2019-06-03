using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Prime31;

[RequireComponent(typeof(CharacterController2D))]
public class Player : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float jumpHeight = 10f;
    public float gravity = -10f;
    public float centreRadius = 0.5f;
    public bool IsRight = true;
    public bool IsJumping = false;
    public bool IsClimbing = false;
    public SpriteRenderer rend;
    public CharacterController2D controller;
    public Animator anim;

    public Vector3 velocity; // Store the difference in movement
    private void Reset()
    {
        controller = GetComponent<CharacterController2D>();
        anim = GetComponent<Animator>();
        rend = GetComponent<SpriteRenderer>();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, centreRadius);
    }

    void Update()
    {
        // Get horizontal and vertical input
        float inputH = Input.GetAxis("Horizontal");
        float inputV = Input.GetAxisRaw("Vertical");

        // Apply gravity
        velocity.y += gravity * Time.deltaTime;
        // If player is standing on ground
        if (controller.isGrounded)
        {
            // Set downward movement to 0 
            velocity.y = 0f;
            anim.SetBool("IsGrounded", true);

            // If we press Jump
            if (Input.GetButtonDown("Jump"))
            {
                // Run Jump()
                Jump();
            }
        }

        if (!anim.GetBool("IsGrounded"))
        {
            anim.SetFloat("JumpY", velocity.y);
        }

        // Climb up or down depending on Y value
        Climb(inputH, inputV);

        if (!IsClimbing)
        {
            // Move left or right depending on X value
            Move(inputH);
        }

        Crouch(inputV);

        // Move using Character Controller 2D
        controller.Move(velocity * Time.deltaTime);

        //Animation();
    }

    public void Move(float inputH)
    {
        // Generate movement for left/right
        velocity.x = inputH * moveSpeed;
        anim.SetBool("IsRunning", inputH != 0);
        if (inputH != 0)
        {
            rend.flipX = inputH <= 0;
        }
    }

    public void Climb(float inputH, float inputV)
    {
        bool IsOverLadder = false;

        #region Part 1 - Detecting Ladders
        // Get a list of all hit objects overlapping point
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, centreRadius);

        // Loop through all hit objects
        foreach (var hit in hits)
        {
            if (hit.tag == "Ground")
            {
                IsClimbing = false;
                IsOverLadder = false;
                break;
            }
            // Check if tagged "Ladder"
            if (hit.tag == "Ladder")
            {
                // Player is overlapping a Ladder!
                IsOverLadder = true;
                break; // Exit just the foreach loop if we are over a ladder
            }
        }
        // If the player is overlapping AND input vertical is made
        if (IsOverLadder && inputV != 0)
        {
            //    The player is in Climbing state!
            IsClimbing = true;
        }
        #endregion

        #region Part 2 - Translating the Player

        Vector3 inputDir = new Vector3(inputH, inputV, 0);
        // If player is climbing
        if (IsClimbing)
        {
            anim.SetBool("IsClimbing", true);
            velocity.y = 0;
            transform.Translate(inputDir * moveSpeed * Time.deltaTime);
        }
        //    Move player up and down on the ladder (additionally move left and right)
        #endregion
        if (!IsOverLadder)
        {
            anim.SetBool("IsClimbing", false);
            anim.SetBool("IsClimbing", IsClimbing);
            IsClimbing = false;
        }

        anim.SetFloat("ClimbSpeed", inputDir.magnitude * moveSpeed);
    }

    public void Hurt()
    {

    }

    public void Jump()
    {
        velocity.y = jumpHeight;
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
