using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player; // Reference to the player object
    public float mouseSensitivity = 200f; // Sensitivity of the mouse movement
    public float maxVerticalAngle = 100f; // Clamp vertical rotation to avoid flipping

    private float xRotation = 0f; // Stores vertical rotation

    void Start()
    {
        // Lock the cursor to the game screen and hide it
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // Get mouse input
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Escape))
    {
        if (Cursor.lockState == CursorLockMode.Locked)
        {
            // Unlock the cursor and make it visible
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            // Lock the cursor and hide it again
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

        // Adjust vertical rotation (pitch)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -maxVerticalAngle, maxVerticalAngle);

        // Rotate the camera vertically
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Rotate the player horizontally based on mouse X input
        player.Rotate(Vector3.up * mouseX);
    }
}
