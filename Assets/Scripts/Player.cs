using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Prime31;

[RequireComponent(typeof(CharacterController2D))]
public class Player : MonoBehaviour
{
    public float movementSpeed = 10f;
    public float gravity = -10f;
    public CharacterController2D controller;

    public Vector3 motion; // Store the difference in movement
    private void Reset()
    {
        controller = GetComponent<CharacterController2D>();
    }

    void Update()
    {
        // Get horizontal input
        float inputH = Input.GetAxis("Horizontal");
        // Generate movement for left/right
        motion.x = inputH * movementSpeed;
        if (controller.isGrounded)
        {
            motion.y = 0f;
        }
        // Apply gravity
        motion.y += gravity * Time.deltaTime;
        // Move using Character Controller 2D
        controller.Move(motion * Time.deltaTime);
    }
}
