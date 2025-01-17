using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

[RequireComponent(typeof(Pathfinding))]
public class PathfindingAI : MonoBehaviour
{
    [Header("References")]
    public Transform player;          // The target to chase
    private Pathfinding pathfinding;  // We'll cache this at runtime

    [Header("Movement Settings")]
    public float moveSpeed = 3f;
    public float updatePathInterval = 1f;
    public float nextNodeDistance = 0.5f;

    [Header("Attack Settings")]
    public float attackCoolDown = 1f;  // 1 second between attacks
    public float damage = 10f;         // 10 HP dealt per attack
    private bool canAttack = true;

    private List<Node> currentPath;
    private int currentNodeIndex;
    private float pathTimer;

    void Start()
    {
        pathfinding = GetComponent<Pathfinding>();
    }

    void Update()
    {
        if (!player) return;

        // Periodically recalc path
        pathTimer += Time.deltaTime;
        if (pathTimer >= updatePathInterval)
        {
            pathTimer = 0f;
            CalculatePath();
        }

        // Move along the path if we have one
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

        // Move toward the next node in the path
        Node targetNode = currentPath[currentNodeIndex];
        Vector3 targetPos = targetNode.worldPosition + Vector3.up * 1.0f;
        // ^ If your floors are exactly at y=0, you might want a small offset for the AI’s pivot

        Vector3 dir = (targetPos - transform.position).normalized;
        transform.position += dir * moveSpeed * Time.deltaTime;

        // Optional: face the movement direction
        if (dir != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(dir);
        }

        // Check if we're close enough to the node to proceed
        if (Vector3.Distance(transform.position, targetPos) < nextNodeDistance)
        {
            currentNodeIndex++;
        }
    }

    // Continuous or repeated damage on collision with the player
    private void OnCollisionStay(Collision collision)
    {
        // If we’re colliding with the Player and we can attack
        if (collision.gameObject.CompareTag("Player") && canAttack)
        {
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                StartCoroutine(DealDamage(playerHealth));
            }
        }
    }

    // Attack cooldown logic
    private IEnumerator DealDamage(PlayerHealth playerHealth)
    {
        canAttack = false;
        playerHealth.TakeDamage(damage);
        // Wait for cooldown


        yield return new WaitForSeconds(attackCoolDown);
        canAttack = true;
    }
}