using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public float attackRange = 2f;
    public float attackDamage = 10f;
    public KeyCode attackKey = KeyCode.Mouse0;
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float attackRate = 5f;
    private float nextAttackTime = 0f;

    // Audio variables
    private AudioSource audioSource;
    public AudioClip shootingSound;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    void Update()
    {
        if (Input.GetButtonDown("Fire1") && Time.time >= nextAttackTime)
        {
            Attack();
            nextAttackTime = Time.time + 1f / attackRate;
        }
    }

    void Attack()
    {
        if (projectilePrefab != null && firePoint != null)
        {
            Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);

            // Play shooting sound
            if (shootingSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(shootingSound);
            }
        }
        
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
