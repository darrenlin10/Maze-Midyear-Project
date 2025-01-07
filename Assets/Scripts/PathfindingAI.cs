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

    void Start()
    {
        pathfinding = GetComponent<Pathfinding>();
    }

    void Update()
    {
        if (player == null) return;

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

        // Optional: Make AI face movement direction
        if (dir != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(dir);
        }

        // Check distance
        if (Vector3.Distance(transform.position, targetPos) < nextNodeDistance)
        {
            currentNodeIndex++;
        }
    }

    // If AI collides with player => player dies or restarts
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerHealth>()?.TakeDamage(9999f);
        }
    }
}
