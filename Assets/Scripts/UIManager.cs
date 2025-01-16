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

    public TMP_Text stageText;

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

        if (stageText != null)
        {
            switch(GameManager.Instance.currentState)
            {
                case GameManager.GameState.Maze:
                    stageText.text = "Current Stage: Maze";
                    break;
                case GameManager.GameState.BossFight:
                    stageText.text = "Current Stage: Boss Fight";
                    break;
                case GameManager.GameState.Defeat:
                    stageText.text = "Current Stage: Defeated";
                    break;   
                case GameManager.GameState.Victory:
                    stageText.text = "Current Stage: Victory";
                    break;
            }
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
