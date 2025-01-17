using UnityEngine;

public class MazeExitTrigger : MonoBehaviour
{
    public Transform player;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager gm = FindObjectOfType<GameManager>();
            if (gm != null)
            {
                gm.OnMazeExitReached();
            }
            player.position = new Vector3(20, 0, 60);
        }
    }
}
