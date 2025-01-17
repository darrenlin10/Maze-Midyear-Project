using UnityEngine;
using UnityEngine.SceneManagement;

public class BossHealth : MonoBehaviour
{
    public float bossHealth = 100f;

    public void TakeDamage(float dmg)
    {
        bossHealth -= dmg;
        if (bossHealth <= 0f)
        {
            bossHealth = 0f; // clamp at 0 to avoid going negative

            Destroy(gameObject);
            // Notify GameManager that player is dead
            GameManager gm = FindObjectOfType<GameManager>();
            if (gm != null)
            {
                gm.OnBossDefeated();
            }
        }
    }
}
