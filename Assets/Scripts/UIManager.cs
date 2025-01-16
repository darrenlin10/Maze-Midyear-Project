using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public PlayerHealth playerHealth;

    public BossHealth bossHealth;
    
    public TMP_Text healthText; // Reference to a UI Text element

    public TMP_Text bossHealthText;

    void Update()
    {
        if (playerHealth != null && healthText != null)
        {
            healthText.text = "Health: " + playerHealth.playerHealth;
        }

        if (bossHealth != null && bossHealthText != null)
        {
            bossHealthText.text = "Boss Health: " + bossHealth.bossHealth;
        }
    }

    public void RestartButton()
    {
        SceneManager.LoadScene("Maze Ex");
    }

    public void QuitButton()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
