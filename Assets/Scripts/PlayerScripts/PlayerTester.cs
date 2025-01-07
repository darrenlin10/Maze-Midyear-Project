using UnityEngine;

public class PlayerTester : MonoBehaviour
{
    public float speed = 5f;
    public int health = 100;

    private Rigidbody rb;
    private Vector3 movement;

    // Attack variables


    // Audio variables

    private AudioSource audioSource;

    public bool isGrounded = true;
    public float jumpForce = 5f; // The force applied to make the player jump

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Freeze rotation on X and Z axes to prevent tipping over
        rb.freezeRotation = true;

        // Get or add AudioSource component
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            // Add an AudioSource component if one doesn't exist
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void Update()
    {

        // Get input
        float moveHorizontal = Input.GetAxis("Horizontal"); // A/D keys
        float moveVertical = Input.GetAxis("Vertical");     // W/S keys

        // Calculate movement relative to player's orientation
        movement = transform.right * moveHorizontal + transform.forward * moveVertical;
        movement.Normalize();

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }
        // Handle attacking
    }

    void FixedUpdate()
    {
        // Move the player
        rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
    }





    void Jump()
    {
        // Apply an upward force to the Rigidbody
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        isGrounded = false; // Player is no longer on the ground
    }

    void OnCollisionEnter(Collision collision)
    {
        // Check if the player is touching the ground
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
}