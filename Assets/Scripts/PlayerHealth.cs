using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public float playerHealth = 100f;

    public void TakeDamage(float dmg)
    {
        playerHealth -= dmg;
        if (playerHealth <= 0f)
        {
            Die();
        }
    }

    void Die()
    {
        // Reload the scene or handle game over
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
