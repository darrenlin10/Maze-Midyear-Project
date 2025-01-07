using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public float attackRange = 2f;
    public float attackDamage = 10f;
    public KeyCode attackKey = KeyCode.Mouse0;
    public LayerMask enemyLayer;

    void Update()
    {
        if (Input.GetKeyDown(attackKey))
        {
            Attack();
        }
    }

    void Attack()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, attackRange, enemyLayer);
        foreach (Collider hit in hits)
        {
            if (hit.CompareTag("Boss"))
            {
                BossAI boss = hit.GetComponent<BossAI>();
                if (boss != null) boss.TakeDamage(attackDamage);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
