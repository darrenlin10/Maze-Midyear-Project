// using UnityEngine;

// public class PlayerController : MonoBehaviour
// {
//     public float speed = 5f;
//     public int health = 100;

//     private Rigidbody rb;
//     private Vector3 movement;

//     // Attack variables
//     public GameObject projectilePrefab;
//     public Transform firePoint;
//     public float attackRate = 5f;
//     private float nextAttackTime = 0f;

//     // Audio variables
//     public AudioClip shootingSound;
//     public AudioClip hurtSound;
//     public AudioClip dyingSound;
//     private AudioSource audioSource;

//     public bool isGrounded = true;
// public float jumpForce = 5f; // The force applied to make the player jump

//     void Start()
//     {
//         rb = GetComponent<Rigidbody>();
//         UIManager.Instance.UpdateHealth(health);

//         // Freeze rotation on X and Z axes to prevent tipping over
//         rb.freezeRotation = true;

//         // Get or add AudioSource component
//         audioSource = GetComponent<AudioSource>();
//         if (audioSource == null)
//         {
//             // Add an AudioSource component if one doesn't exist
//             audioSource = gameObject.AddComponent<AudioSource>();
//         }
//     }

//     void Update()
//     {
//         if (GameManager.Instance != null && GameManager.Instance.isGameOver)
//             return;

//         // Get input
//         float moveHorizontal = Input.GetAxis("Horizontal"); // A/D keys
//         float moveVertical = Input.GetAxis("Vertical");     // W/S keys

//         // Calculate movement relative to player's orientation
//         movement = transform.right * moveHorizontal + transform.forward * moveVertical;
//         movement.Normalize();

//         if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
//         {
//             Jump();
//         }
//         // Handle attacking
//         if (Input.GetButtonDown("Fire1") && Time.time >= nextAttackTime)
//         {
//             Shoot();
//             nextAttackTime = Time.time + 1f / attackRate;
//         }
//     }

//     void FixedUpdate()
//     {
//         if (GameManager.Instance != null && GameManager.Instance.isGameOver)
//             return;

//         // Move the player
//         rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
//     }

//     public void TakeDamage(int damage)
//     {
//         // Play hurt sound
//         if (hurtSound != null && audioSource != null)
//         {
//             audioSource.PlayOneShot(hurtSound);
//         }

//         health -= damage;
//         UIManager.Instance.UpdateHealth(health);

//         if (health <= 0)
//         {
//             health = 0;
//             UIManager.Instance.UpdateHealth(health);

//             // Play dying sound
//             if (dyingSound != null && audioSource != null)
//             {
//                 audioSource.PlayOneShot(dyingSound);
//             }

//             GameManager.Instance.GameOver(false);
//         }
//     }

//     public void Heal(int amount)
//     {
//         health += amount;
//         if (health > 100)
//             health = 100;

//         UIManager.Instance.UpdateHealth(health);
//     }

//     void Shoot()
//     {
//         // Instantiate projectile
//         if (projectilePrefab != null && firePoint != null)
//         {
//             Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);

//             // Play shooting sound
//             if (shootingSound != null && audioSource != null)
//             {
//                 audioSource.PlayOneShot(shootingSound);
//             }
//         }
//     }

//     void Jump()
//     {
//         // Apply an upward force to the Rigidbody
//         rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
//         isGrounded = false; // Player is no longer on the ground
//     }

//     void OnCollisionEnter(Collision collision)
//     {
//         // Check if the player is touching the ground
//         if (collision.gameObject.CompareTag("Ground"))
//         {
//             isGrounded = true;
//         }
//     }
// }