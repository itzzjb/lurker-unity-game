using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sway : MonoBehaviour
{
    public float swayAmount = 0.5f;      // The amount of sway to apply to the pistol
    public float smoothFactor = 2f;      // The smooth factor for the sway movement

    private Quaternion initialRotation;  // The initial rotation of the pistol
    private Transform playerCamera;      // Reference to the player's camera

    void Start()
    {
        // Get the main camera's transform and store it in playerCamera
        playerCamera = Camera.main.transform;

        // Store the initial local rotation of the pistol
        initialRotation = transform.localRotation;
    }

    void Update()
    {
        // Get the mouse input for X and Y axes and invert them
        float inputX = -Input.GetAxis("Mouse X") * swayAmount;
        float inputY = -Input.GetAxis("Mouse Y") * swayAmount;

        // Calculate the target rotation based on the mouse input and initial rotation
        Quaternion targetRotation = Quaternion.Euler(inputY, inputX, 0f) * initialRotation;

        // Smoothly interpolate the current rotation towards the target rotation
        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, Time.deltaTime * smoothFactor);
    }
}