using UnityEngine;

public class MazeGrid : MonoBehaviour
{
    [HideInInspector] public int width;
    [HideInInspector] public int height;
    [HideInInspector] public float cellSize = 1f;

    public Node[,] grid;

    // Called after the MazeGenerator is done
    public void CreateGrid(bool[,] mazeData, float _cellSize)
    {
        width = mazeData.GetLength(0);
        height = mazeData.GetLength(1);
        cellSize = _cellSize;

        grid = new Node[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                bool walkable = mazeData[x, y];
                Vector3 worldPos = new Vector3(x * cellSize, 0, y * cellSize);
                grid[x, y] = new Node(walkable, worldPos, x, y);
            }
        }
    }

    public Node GetNodeFromWorldPos(Vector3 worldPos)
    {
        int x = Mathf.RoundToInt(worldPos.x / cellSize);
        int y = Mathf.RoundToInt(worldPos.z / cellSize);

        x = Mathf.Clamp(x, 0, width - 1);
        y = Mathf.Clamp(y, 0, height - 1);

        return grid[x, y];
    }

    public System.Collections.Generic.List<Node> GetNeighbours(Node node)
    {
        System.Collections.Generic.List<Node> neighbours = new System.Collections.Generic.List<Node>();

        // 4-direction
        int[] dx = { 0, 1, 0, -1 };
        int[] dy = { 1, 0, -1, 0 };

        for (int i = 0; i < 4; i++)
        {
            int checkX = node.gridX + dx[i];
            int checkY = node.gridY + dy[i];

            if (checkX >= 0 && checkX < width && checkY >= 0 && checkY < height)
            {
                neighbours.Add(grid[checkX, checkY]);
            }
        }

        return neighbours;
    }
}
