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
            playerHealth = 0f; // clamp at 0 to avoid going negative

            // Notify GameManager that player is dead
            GameManager gm = FindObjectOfType<GameManager>();
            if (gm != null)
            {
                gm.OnPlayerDefeated();
            }
        }
    }
}
