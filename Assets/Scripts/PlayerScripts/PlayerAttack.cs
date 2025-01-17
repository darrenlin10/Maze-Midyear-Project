// using UnityEngine;

// public class PlayerAttack : MonoBehaviour
// {
//     public float attackRange = 2f;
//     public float attackDamage = 10f;
//     public KeyCode attackKey = KeyCode.Mouse0; // Left mouse click to attack
//     public LayerMask enemyLayers; // Boss or NPC layers

//     void Update()
//     {
//         if (Input.GetKeyDown(attackKey))
//         {
//             AttemptAttack();
//         }
//     }

//     void AttemptAttack()
//     {
//         Collider[] hits = Physics.OverlapSphere(transform.position, attackRange, enemyLayers);
//         foreach (Collider hit in hits)
//         {
//             // Boss or NPC script
//             if (hit.CompareTag("Boss"))
//             {
//                 FuzzyBossAI boss = hit.GetComponent<FuzzyBossAI>();
//                 if (boss != null)
//                 {
//                     boss.TakeDamage(attackDamage);
//                 }
//             }
//             else if (hit.CompareTag("NPC"))
//             {
//                 // If you had a script for damageable NPCs, you could put it here
//             }
//         }
//     }

//     // For debugging in the editor
//     void OnDrawGizmosSelected()
//     {
//         Gizmos.color = Color.red;
//         Gizmos.DrawWireSphere(transform.position, attackRange);
//     }
// }