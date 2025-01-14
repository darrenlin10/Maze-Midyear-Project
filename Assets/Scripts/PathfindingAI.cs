using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;
using System.Collections;

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

    private float attackCoolDown = 1f;
    private bool canAttack = true;


    // Continuous damage settings
    private float damage = 10f; // 10 HP/sec

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

     private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && canAttack)
        {
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                StartCoroutine(DealDamage(playerHealth));
            }
        }
    }

    private IEnumerator DealDamage(PlayerHealth playerHealth)
    {
        canAttack = false;
        playerHealth.TakeDamage(damage);
        yield return new WaitForSeconds(attackCoolDown);
        canAttack = true;
    }
}
