using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject howToPlayPanel;  // Assign a UI panel that displays instructions

    // Called by the Start button
    public void OnStartButton()
    {
        SceneManager.LoadScene("Maze Ex");
    }

    // Called by the How to Play button
    public void OnHowToPlayButton()
    {
        if (howToPlayPanel != null)
        {
            howToPlayPanel.SetActive(true);
        }
    }

    // Called by a close button on the HowToPlayPanel, or the back button
    public void OnCloseHowToPlay()
    {
        if (howToPlayPanel != null)
        {
            howToPlayPanel.SetActive(false);
        }
    }
}
