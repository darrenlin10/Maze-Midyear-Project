using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Pathfinding))]
public class PathfindingAI : MonoBehaviour
{
    public Transform player;
    public float moveSpeed = 3f;
    public float updatePathInterval = 1f;
    public float nextNodeDistance = 0.5f;

    private Pathfinding pathfinding;
    private List<Node> currentPath;
    private int currentNodeIndex;
    private float pathTimer;
    private bool attackCooldown;

    // Continuous damage settings
    private float damagePerSecond = 10f; // 10 HP/sec

    void Start()
    {
        pathfinding = GetComponent<Pathfinding>();
    }

    void Update()
    {
        if (!player) return;

        pathTimer += Time.deltaTime;
        if (pathTimer >= updatePathInterval)
        {
            pathTimer = 0f;
            CalculatePath();
        }

        FollowPath();
    }

    void CalculatePath()
    {
        currentPath = pathfinding.FindPath(transform.position, player.position);
        currentNodeIndex = 0;
    }

    void FollowPath()
    {
        if (currentPath == null || currentPath.Count == 0) return;
        if (currentNodeIndex >= currentPath.Count) return;

        Node targetNode = currentPath[currentNodeIndex];
        Vector3 targetPos = targetNode.worldPosition + Vector3.up * 1.0f; 
        // Add a little Y offset if your maze floors are at y=0

        // Move toward the node
        Vector3 dir = (targetPos - transform.position).normalized;
        transform.position += dir * moveSpeed * Time.deltaTime;

        // Face movement direction
        if (dir != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(dir);
        }

        // If close enough to the node, move to the next
        if (Vector3.Distance(transform.position, targetPos) < nextNodeDistance)
        {
            currentNodeIndex++;
        }
    }

    // Instead of instant kill, do continuous damage
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Get the player's health script
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null && !attackCooldown)
            {
                // 10 HP/sec => damagePerSecond * Time.deltaTime each frame
                playerHealth.TakeDamage(damagePerSecond);
                attackCooldown = true;
            }
        }
    }
}
