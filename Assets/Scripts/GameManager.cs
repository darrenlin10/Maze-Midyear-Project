using UnityEngine;

public class GameManager : MonoBehaviour
{
    public MazeGenerator mazeGenerator;
    public MazeGrid mazeGrid;

    public GameObject enemyPrefab;
    public int numberOfEnemies = 3;

    void Start()
    {
        // 1. Generate the maze
        mazeGenerator.GenerateMaze();

        // 2. Create the grid
        mazeGrid.CreateGrid(mazeGenerator.GetMazeData(), mazeGenerator.cellSize);

        // 3. Spawn enemies in random walkable cells
        SpawnEnemies();
    }

    void SpawnEnemies()
    {
        for (int i = 0; i < numberOfEnemies; i++)
        {
            Vector3 spawnPos = GetRandomWalkablePosition();
            Instantiate(enemyPrefab, spawnPos + Vector3.up * 1f, Quaternion.identity);
        }
    }

    Vector3 GetRandomWalkablePosition()
    {
        bool valid = false;
        Vector3 result = Vector3.zero;
        while (!valid)
        {
            int randX = Random.Range(0, mazeGrid.width);
            int randY = Random.Range(0, mazeGrid.height);

            Node node = mazeGrid.grid[randX, randY];
            if (node.walkable)
            {
                valid = true;
                result = node.worldPosition;
            }
        }
        return result;
    }
}
