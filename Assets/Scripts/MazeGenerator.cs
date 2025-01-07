using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    [Header("Maze Settings")]
    public int mazeWidth = 10;
    public int mazeHeight = 10;
    public float cellSize = 4f;

    [Header("Prefabs")]
    public GameObject wallPrefab;
    public GameObject floorPrefab;

    [Header("Player Reference")]
    public Transform player;  // Drag your Player here in the Inspector

    // We'll store our maze walkable data in a 2D array: true=walkable, false=wall
    private bool[,] mazeData;

    public void GenerateMaze()
    {
        mazeData = new bool[mazeWidth, mazeHeight];

        // 1. Initialize everything as walls
        for (int x = 0; x < mazeWidth; x++)
        {
            for (int y = 0; y < mazeHeight; y++)
            {
                mazeData[x, y] = false;
            }
        }

        // 2. Carve out a path (very simple random walk example)
        int currentX = 0;
        int currentY = 0;
        mazeData[currentX, currentY] = true;

        for (int i = 0; i < (mazeWidth * mazeHeight); i++)
        {
            int rnd = Random.Range(0, 4);
            switch (rnd)
            {
                case 0: // up
                    currentY = Mathf.Clamp(currentY + 1, 0, mazeHeight - 1);
                    break;
                case 1: // down
                    currentY = Mathf.Clamp(currentY - 1, 0, mazeHeight - 1);
                    break;
                case 2: // right
                    currentX = Mathf.Clamp(currentX + 1, 0, mazeWidth - 1);
                    break;
                case 3: // left
                    currentX = Mathf.Clamp(currentX - 1, 0, mazeWidth - 1);
                    break;
            }
            mazeData[currentX, currentY] = true;
        }

        // 3. Build the 3D maze from mazeData
        BuildMaze3D();

        // 4. Spawn the player inside the maze at a walkable cell
        if (player != null)
        {
            // First, try the (0,0) cell (where we started carving).
            if (mazeData[0, 0])
            {
                // Place slightly above floor to avoid clipping
                player.position = new Vector3(0f * cellSize, 1.5f, 0f * cellSize);
            }
            else
            {
                // If (0,0) isn't walkable for some reason, pick the first walkable we find
                for (int x = 0; x < mazeWidth; x++)
                {
                    for (int y = 0; y < mazeHeight; y++)
                    {
                        if (mazeData[x, y])
                        {
                            player.position = new Vector3(x * cellSize, 1.5f, y * cellSize);
                            return; // Once placed, we're done
                        }
                    }
                }
                Debug.LogWarning("No walkable cell found! Player not placed.");
            }
        }
        else
        {
            Debug.LogWarning("No Player Transform assigned to MazeGenerator!");
        }
    }

    private void BuildMaze3D()
    {
        // Clear old maze objects
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
      for (int x = 0; x < mazeWidth; x++)
        {
            for (int y = 0; y < mazeHeight; y++)
            {
                Vector3 cellPos = new Vector3(x * cellSize, 0, y * cellSize);

                // Always place floor
                GameObject floor = Instantiate(floorPrefab, cellPos, Quaternion.identity, transform);
                floor.transform.localScale = new Vector3(cellSize, 1, cellSize);

                // If not walkable, spawn walls
                if (!mazeData[x, y])
                {
                    GameObject wall = Instantiate(wallPrefab, cellPos + Vector3.up * 1.5f, Quaternion.identity, transform);
                    wall.transform.localScale = new Vector3(cellSize, 3, cellSize);
                }
            }
        }
    }

    public bool[,] GetMazeData()
    {
        return mazeData;
    }
}