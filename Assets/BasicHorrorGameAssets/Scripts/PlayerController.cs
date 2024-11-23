using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This Will Auto Add Character Controller To Gameobject If It's Not Already Applied:
[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    // Camera:
    public Camera playerCam; // Reference to the player's camera

    // Movement Settings:
    public float walkSpeed = 3f; // Speed at which the player walks
    public float runSpeed = 5f; // Speed at which the player runs
    public float jumpPower = 0f; // Power of the player's jump
    public float gravity = 10f; // Gravity affecting the player

    // Camera Settings:
    public float lookSpeed = 2f; // Speed of camera rotation
    public float lookXLimit = 75f; // Limit for vertical camera rotation
    public float cameraRotationSmooth = 5f; // Smoothness of camera rotation

    // Ground Sounds:
    public AudioClip[] woodFootstepSounds; // Footstep sounds for wood surfaces
    public AudioClip[] tileFootstepSounds; // Footstep sounds for tile surfaces
    public AudioClip[] carpetFootstepSounds; // Footstep sounds for carpet surfaces
    public Transform footstepAudioPosition; // Position from where footstep sounds are played
    public AudioSource audioSource; // Audio source for playing footstep sounds

    private bool isWalking = false; // Flag to check if the player is walking
    private bool isFootstepCoroutineRunning = false; // Flag to check if the footstep coroutine is running
    private AudioClip[] currentFootstepSounds; // Current set of footstep sounds based on the surface

    Vector3 moveDirection = Vector3.zero; // Direction of player movement
    float rotationX = 0; // Rotation around the X-axis
    float rotationY = 0; // Rotation around the Y-axis

    // Camera Zoom Settings:
    public int ZoomFOV = 35; // Field of view when zoomed in
    public int initialFOV; // Initial field of view
    public float cameraZoomSmooth = 1; // Smoothness of camera zoom

    private bool isZoomed = false; // Flag to check if the camera is zoomed in

    // Can The Player Move?:
    private bool canMove = true; // Flag to check if the player can move

    CharacterController characterController; // Reference to the CharacterController component

    void Start()
    {
        // Ensure We Are Using The Character Controller Component:
        characterController = GetComponent<CharacterController>();

        // Lock And Hide Cursor:
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Initialize current footstep sounds to wood sounds by default
        currentFootstepSounds = woodFootstepSounds;
    }

    void Update()
    {
        // Walking/Running In Action:
        Vector3 forward = transform.TransformDirection(Vector3.forward); // Forward direction based on player orientation
        Vector3 right = transform.TransformDirection(Vector3.right); // Right direction based on player orientation

        bool isRunning = Input.GetKey(KeyCode.LeftShift); // Check if the player is running

        float curSpeedX = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Vertical") : 0; // Calculate forward/backward speed
        float curSpeedY = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Horizontal") : 0; // Calculate left/right speed
        float movementDirectionY = moveDirection.y; // Preserve the current vertical movement direction
        moveDirection = (forward * curSpeedX) + (right * curSpeedY); // Combine forward and right movement

        // Jumping In Action:
        if (Input.GetButton("Jump") && canMove && characterController.isGrounded)
        {
            moveDirection.y = jumpPower; // Apply jump power if the player is grounded and can move
        }
        else
        {
            moveDirection.y = movementDirectionY; // Preserve the current vertical movement direction
        }

        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime; // Apply gravity if the player is not grounded
        }

        characterController.Move(moveDirection * Time.deltaTime); // Move the player

        // Camera Movement In Action:
        if (canMove)
        {
            rotationX -= Input.GetAxis("Mouse Y") * lookSpeed; // Calculate vertical rotation
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit); // Clamp vertical rotation

            rotationY += Input.GetAxis("Mouse X") * lookSpeed; // Calculate horizontal rotation

            Quaternion targetRotationX = Quaternion.Euler(rotationX, 0, 0); // Target vertical rotation
            Quaternion targetRotationY = Quaternion.Euler(0, rotationY, 0); // Target horizontal rotation

            playerCam.transform.localRotation = Quaternion.Slerp(playerCam.transform.localRotation, targetRotationX, Time.deltaTime * cameraRotationSmooth); // Smoothly rotate the camera vertically
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotationY, Time.deltaTime * cameraRotationSmooth); // Smoothly rotate the player horizontally
        }

        // Zooming In Action:
        if (Input.GetButtonDown("Fire2"))
        {
            isZoomed = true; // Enable zoom
        }

        if (Input.GetButtonUp("Fire2"))
        {
            isZoomed = false; // Disable zoom
        }

        if (isZoomed)
        {
            playerCam.GetComponent<Camera>().fieldOfView = Mathf.Lerp(playerCam.fieldOfView, ZoomFOV, Time.deltaTime * cameraZoomSmooth); // Smoothly zoom in
        }
        else
        {
            playerCam.GetComponent<Camera>().fieldOfView = Mathf.Lerp(playerCam.fieldOfView, initialFOV, Time.deltaTime * cameraZoomSmooth); // Smoothly zoom out
        }

        // Play footstep sounds when walking
        if ((curSpeedX != 0f || curSpeedY != 0f) && !isWalking && !isFootstepCoroutineRunning)
        {
            isWalking = true; // Set walking flag
            StartCoroutine(PlayFootstepSounds(1.3f / (isRunning ? runSpeed : walkSpeed))); // Start footstep sounds coroutine
        }
        else if (curSpeedX == 0f && curSpeedY == 0f)
        {
            isWalking = false; // Reset walking flag
        }
    }

    // Play footstep sounds with a delay based on movement speed
    IEnumerator PlayFootstepSounds(float footstepDelay)
    {
        isFootstepCoroutineRunning = true; // Set coroutine running flag

        while (isWalking)
        {
            if (currentFootstepSounds.Length > 0)
            {
                int randomIndex = Random.Range(0, currentFootstepSounds.Length); // Select a random footstep sound
                audioSource.transform.position = footstepAudioPosition.position; // Set audio source position
                audioSource.clip = currentFootstepSounds[randomIndex]; // Set audio clip
                audioSource.Play(); // Play footstep sound
                yield return new WaitForSeconds(footstepDelay); // Wait for the next footstep sound
            }
            else
            {
                yield break; // Exit coroutine if no footstep sounds are available
            }
        }

        isFootstepCoroutineRunning = false; // Reset coroutine running flag
    }

    // Detect ground surface and set the current footstep sounds array accordingly
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Wood"))
        {
            currentFootstepSounds = woodFootstepSounds; // Set footstep sounds to wood
        }
        else if (other.CompareTag("Tile"))
        {
            currentFootstepSounds = tileFootstepSounds; // Set footstep sounds to tile
        }
        else if (other.CompareTag("Carpet"))
        {
            currentFootstepSounds = carpetFootstepSounds; // Set footstep sounds to carpet
        }
    }
}