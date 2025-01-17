using UnityEngine;
using UnityEngine.SceneManagement;  // For restarting the scene, if desired

public class PatrollingAI : MonoBehaviour
{
    [Header("Patrol Settings")]
    public Transform[] patrolPoints;     // Assign via Inspector (the “pivot points”)
    public float moveSpeed = 2f;         // Patrol movement speed
    public float waypointThreshold = 0.5f;  // How close to a waypoint before moving to the next

    [Header("Chase Settings")]
    public float chaseRange = 5f;        // Distance at which AI starts chasing
    public float chaseSpeed = 4f;        // Movement speed when chasing

    [Header("Player Reference")]
    public Transform player;             // Assign the Player transform in the Inspector

    private int currentPatrolIndex;
    private bool isChasing = false;

    void Start()
    {
        if (patrolPoints.Length > 0)
        {
            currentPatrolIndex = 0;
            transform.position = patrolPoints[currentPatrolIndex].position;
        }
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // If player is within chaseRange, start chasing
        if (distanceToPlayer < chaseRange)
        {
            isChasing = true;
        }
        else
        {
            // If player is out of range, return to patrol
            isChasing = false;
        }

        if (isChasing)
        {
            ChasePlayer();
        }
        else
        {
            Patrol();
        }
    }

    void Patrol()
    {
        if (patrolPoints.Length == 0) return;

        Transform targetWaypoint = patrolPoints[currentPatrolIndex];
        // Move towards current waypoint
        transform.position = Vector3.MoveTowards(
            transform.position,
            targetWaypoint.position,
            moveSpeed * Time.deltaTime
        );

        // Check if we are near the waypoint
        float distanceToWaypoint = Vector3.Distance(transform.position, targetWaypoint.position);
        if (distanceToWaypoint < waypointThreshold)
        {
            // Move to the next waypoint index
            currentPatrolIndex++;
            if (currentPatrolIndex >= patrolPoints.Length)
            {
                currentPatrolIndex = 0;  // Loop back to the first
            }
        }
    }

    void ChasePlayer()
    {
        if (player == null) return;

        transform.position = Vector3.MoveTowards(
            transform.position,
            player.position,
            chaseSpeed * Time.deltaTime
        );
    }

    // If we want the AI to reset the game when it touches the player
    // Make sure your AI or your Player has a suitable collider and rigidbody
    // with the correct collision/trigger settings.
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Player “loses” scenario:
            RestartLevel();
        }
    }

    // If using triggers instead of collisions:
    // private void OnTriggerEnter(Collider other)
    // {
    //     if (other.CompareTag("Player"))
    //     {
    //         RestartLevel();
    //     }
    // }

    void RestartLevel()
    {
        // Option 1: Reload the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        // Option 2: Move player to a spawn point manually (if you have one):
        // collision.gameObject.transform.position = playerSpawnPoint.position;
    }
}
