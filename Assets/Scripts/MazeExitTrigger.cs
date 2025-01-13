using UnityEngine;

public class MazeExitTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
    {
            GameManager gm = FindObjectOfType<GameManager>();
            if (gm != null)
    {
                gm.OnMazeExitReached();
            }
        }
    }
}
