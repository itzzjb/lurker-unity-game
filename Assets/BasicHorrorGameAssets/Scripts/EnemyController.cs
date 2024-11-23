using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public Transform[] waypoints; // Array of waypoints for the enemy to patrol
    public float idleTime = 2f; // Time the enemy spends idling at each waypoint
    public float walkSpeed = 2f; // Walking speed of the enemy
    public float chaseSpeed = 4f; // Chasing speed of the enemy
    public float sightDistance = 10f; // Distance at which the enemy can detect the player
    public AudioClip idleSound; // Sound played when the enemy is idling
    public AudioClip walkingSound; // Sound played when the enemy is walking
    public AudioClip chasingSound; // Sound played when the enemy is chasing the player

    private int currentWaypointIndex = 0; // Index of the current waypoint
    private NavMeshAgent agent; // Reference to the NavMeshAgent component
    private Animator animator; // Reference to the Animator component
    private float idleTimer = 0f; // Timer to track idle time
    private Transform player; // Reference to the player's transform
    private AudioSource audioSource; // Reference to the AudioSource component

    // Enum to represent the different states of the enemy
    private enum EnemyState { Idle, Walk, Chase }
    private EnemyState currentState = EnemyState.Idle; // Current state of the enemy

    private bool isChasingAnimation = false; // Flag to check if the chasing animation is playing

    private void Start()
    {
        // Initialize references to components
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        audioSource = GetComponent<AudioSource>();

        // Set the initial destination to the first waypoint
        SetDestinationToWaypoint();
    }

    private void Update()
    {
        // Handle behavior based on the current state
        switch (currentState)
        {
            case EnemyState.Idle:
                // Increment the idle timer
                idleTimer += Time.deltaTime;

                // Set the appropriate animation states
                animator.SetBool("IsWalking", false);
                animator.SetBool("IsChasing", false);

                // Play the idle sound
                PlaySound(idleSound);

                // Check if the idle time has elapsed
                if (idleTimer >= idleTime)
                {
                    // Move to the next waypoint
                    NextWaypoint();
                }

                // Check for player detection
                CheckForPlayerDetection();
                break;

            case EnemyState.Walk:
                // Reset the idle timer
                idleTimer = 0f;

                // Set the appropriate animation states
                animator.SetBool("IsWalking", true);
                animator.SetBool("IsChasing", false);

                // Play the walking sound
                PlaySound(walkingSound);

                // Check if the enemy has reached the waypoint
                if (agent.remainingDistance <= agent.stoppingDistance)
                {
                    // Switch to the idle state
                    currentState = EnemyState.Idle;
                }

                // Check for player detection
                CheckForPlayerDetection();
                break;

            case EnemyState.Chase:
                // Reset the idle timer
                idleTimer = 0f;

                // Set the chase speed
                agent.speed = chaseSpeed;

                // Set the destination to the player's position
                agent.SetDestination(player.position);

                // Set the chasing animation flag
                isChasingAnimation = true;

                // Set the appropriate animation states
                animator.SetBool("IsChasing", true);

                // Play the chasing sound
                PlaySound(chasingSound);

                // Check if the player is out of sight
                if (Vector3.Distance(transform.position, player.position) > sightDistance)
                {
                    // Switch to the walk state and restore the walking speed
                    currentState = EnemyState.Walk;
                    agent.speed = walkSpeed;
                }
                break;
        }
    }

    private void CheckForPlayerDetection()
    {
        // Perform a raycast to check for player detection
        RaycastHit hit;
        Vector3 playerDirection = player.position - transform.position;

        if (Physics.Raycast(transform.position, playerDirection.normalized, out hit, sightDistance))
        {
            // Check if the raycast hit the player
            if (hit.collider.CompareTag("Player"))
            {
                // Switch to the chase state
                currentState = EnemyState.Chase;
                Debug.Log("Player detected!");
            }
        }
    }

    private void PlaySound(AudioClip soundClip)
    {
        // Play the specified sound clip if it is not already playing
        if (!audioSource.isPlaying || audioSource.clip != soundClip)
        {
            audioSource.clip = soundClip;
            audioSource.Play();
        }
    }

    private void NextWaypoint()
    {
        // Move to the next waypoint in the array
        currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        SetDestinationToWaypoint();
    }

    private void SetDestinationToWaypoint()
    {
        // Set the destination to the current waypoint
        agent.SetDestination(waypoints[currentWaypointIndex].position);

        // Switch to the walk state and set the walking speed
        currentState = EnemyState.Walk;
        agent.speed = walkSpeed;
        animator.enabled = true;
    }

    private void OnDrawGizmos()
    {
        // Draw a green raycast line at all times and switch to red when the player is detected
        Gizmos.color = currentState == EnemyState.Chase ? Color.red : Color.green;
        Gizmos.DrawLine(transform.position, player.position);
    }
}