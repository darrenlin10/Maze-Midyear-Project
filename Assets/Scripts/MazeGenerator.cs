using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    [Header("References")]
    public Maze mazeScript;       // Drag your Maze script object here in Inspector
    public MazeGrid mazeGrid;     // Drag the MazeGrid object here
    public Transform player;      // The player to place in the maze
    public float cellSize = 1f;   // If you want each cell to be scaled differently for pathfinding

    void Start()
    {
        // 1. Maze script should have already generated MazeData in Awake()
        //    So let's get it here:
        bool[,] data = mazeScript.MazeData;
        int mazeWidth = data.GetLength(0);   // = 2 * Length - 1
        int mazeHeight = data.GetLength(1);  // = 2 * Width - 1

        // 2. Pass MazeData to MazeGrid for pathfinding
        mazeGrid.CreateGrid(data, cellSize);

        // 3. Place player in the first walkable cell
        PlacePlayerInMaze(data);
    }

    private void PlacePlayerInMaze(bool[,] data)
    {
        int width = data.GetLength(0);
        int height = data.GetLength(1);

        if (player == null)
        {
            Debug.LogWarning("No Player Transform assigned to MazeGenerator!");
            return;
        }

        bool placedPlayer = false;
        for (int x = 0; x < width && !placedPlayer; x++)
        {
            for (int y = 0; y < height && !placedPlayer; y++)
            {
                if (data[x, y]) // walkable
                {
                    // Place slightly above that cell to avoid clipping
                    player.position = new Vector3(x * cellSize, 1.5f, y * cellSize);
                    placedPlayer = true;
                }
            }
        }

        if (!placedPlayer)
        {
            Debug.LogWarning("No walkable cell found in MazeData! Player not placed.");
        }
    }
}
