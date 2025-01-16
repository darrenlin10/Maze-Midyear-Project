using UnityEngine;
using UnityEngine.SceneManagement; // For scene loading/reloading

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public MazeGenerator mazeGenerator;
    public MazeGrid mazeGrid;

    [Header("Enemy Spawning")]
    public GameObject enemyPrefab;
    public int numberOfEnemies = 3;

    [Header("Player Settings")]
    public PlayerAttack playerAttack;  // Drag your PlayerAttack script here
    public Transform player;           // The Player transform

    [Header("Boss Settings")]
    public BossAI boss;               // The boss reference if needed

    public enum GameState { Maze, BossFight, Victory, Defeat }
    public GameState currentState = GameState.Maze;

    public GameObject defeatPanel;
    public GameObject victoryPanel;

    public GameObject bossHealthPanel;

    void Start()
    {
        // Start in Maze state
        currentState = GameState.Maze;

        // 1. Generate the maze
        mazeGenerator.GenerateMaze();

        // 2. Create the grid
        mazeGrid.CreateGrid(mazeGenerator.GetMazeData(), mazeGenerator.cellSize);

        // 3. Spawn enemies in random walkable cells
        SpawnEnemies();

        // Disable player attack initially (player can’t fight until exit is found)
        if (playerAttack) playerAttack.enabled = false;
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

    // Called by MazeExitTrigger when the player reaches the maze exit
    public void OnMazeExitReached()
    {
        currentState = GameState.BossFight;
        
        if (playerAttack) playerAttack.enabled = true;
        Debug.Log("Maze exit reached! Boss fight enabled. Player can now attack.");
        bossHealthPanel.SetActive(true);
    }

    // Called if the player’s HP reaches 0 or if the boss kills the player
    public void OnPlayerDefeated()
    {
        if (currentState == GameState.Defeat || currentState == GameState.Victory) return;
        currentState = GameState.Defeat;
        Debug.Log("Game Over! Player has been defeated.");

        defeatPanel.SetActive(true);
        bossHealthPanel.SetActive(false);
    }

    // Called by BossAI when boss HP <= 0
    public void OnBossDefeated()
    {
        if (currentState == GameState.Defeat) return;
        currentState = GameState.Victory;
        Debug.Log("Victory! Boss was defeated!");

        victoryPanel.SetActive(true);
        bossHealthPanel.SetActive(false);
    }
}
