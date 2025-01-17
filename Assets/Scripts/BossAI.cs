using UnityEngine;
using UnityEngine.SceneManagement;

public class BossAI : MonoBehaviour
{
    public static BossAI Instance;
    public float detectRange = 20f;
    public float attackRange = 10f;
    public float speed = 2f;
    public Transform player;

    public GameObject projectilePrefab;
    public Transform projectileSpawnPoint;
    public float shootCooldown = 2f;
    public float bulletDamage = 10f;

    private float shootTimer = 0f;
    public int maxHealth = 50;
    public int currentHealth;

    // Health bar UI
    public GameObject healthBarPrefab;
    private GameObject healthBarInstance;

    void Update()
    {
        if (!player) return;
        float dist = Vector3.Distance(transform.position, player.position);

        if (dist <= detectRange)
        {
            // Face player
            Vector3 dir = (player.position - transform.position);
            dir.y = 0;
            transform.rotation = Quaternion.LookRotation(dir);

            // Move if out of attack range
            if (dist > attackRange)
            {
                transform.Translate(Vector3.forward * speed * Time.deltaTime);
            }
            else
            {
                // Shoot
                shootTimer += Time.deltaTime;
                if (shootTimer >= shootCooldown)
                {
                    shootTimer = 0f;
                    ShootProjectile();
                }
            }
        }
    }

    private void ShootProjectile()
    {
        if (projectilePrefab && projectileSpawnPoint)
        {
            GameObject bullet = Instantiate(projectilePrefab, projectileSpawnPoint.position, projectileSpawnPoint.rotation);
            var proj = bullet.GetComponent<Projectile>();
            if (proj != null) proj.damage = bulletDamage;
        }
    }
}
