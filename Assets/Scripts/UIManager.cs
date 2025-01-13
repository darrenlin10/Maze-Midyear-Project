using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public PlayerHealth playerHealth;
    public TMP_Text healthText; // Reference to a UI Text element

    void Update()
    {
        if (playerHealth != null && healthText != null)
        {
            healthText.text = "Health: " + playerHealth.playerHealth;
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
