using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
}
