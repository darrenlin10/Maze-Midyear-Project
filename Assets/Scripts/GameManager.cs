using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Advanced Maze Script")]
    public Maze advancedMazeScript; // Reference to your advanced "Maze" script

    [Header("Maze Grid (for Pathfinding)")]
    public MazeGrid mazeGrid;       // Drag the MazeGrid object here
    public float cellSize = 1f;     // Each cell's size for pathfinding

    [Header("Enemy Spawning")]
    public GameObject enemyPrefab;
    public int numberOfEnemies = 3;

    [Header("Player Settings")]
    public PlayerAttack playerAttack;  
    public Transform player;  

    [Header("Boss Settings")]
    public BossAI boss;              

    public enum GameState { Maze, BossFight, Victory, Defeat }
    public GameState currentState = GameState.Maze;

    [Header("UI Panels")]
    public GameObject defeatPanel;
    public GameObject victoryPanel;
    public GameObject bossHealthPanel;

    void Start()
    {
        // Make this a singleton instance
        Instance = this;

        // Hide UI panels at the start
        if (defeatPanel) defeatPanel.SetActive(false);
        if (victoryPanel) victoryPanel.SetActive(false);
        if (bossHealthPanel) bossHealthPanel.SetActive(false);

        // Start in Maze state
        currentState = GameState.Maze;

        // 1. The advanced Maze script creates MazeData in its Awake().
        //    Let's grab that MazeData now for pathfinding.
        bool[,] data = advancedMazeScript.MazeData;
        // This array has dimensions (2*Length - 1, 2*Width - 1)

        // 2. Pass MazeData to MazeGrid for pathfinding
        mazeGrid.CreateGrid(data, cellSize);

        // 3. Spawn the player in the first walkable cell
        PlacePlayer(data);

        // 4. Spawn enemies in random walkable cells
        SpawnEnemies();

        // Disable player attack initially (player can’t fight until exit is found)
        if (playerAttack)
        {
            playerAttack.enabled = false;
        }
    }

    private void PlacePlayer(bool[,] data)
    {
        if (!player)
        {
            Debug.LogWarning("No Player Transform assigned in GameManager!");
            return;
        }

        int width = data.GetLength(0);   // 2 * Length - 1
        int height = data.GetLength(1);  // 2 * Width - 1

        bool placedPlayer = false;
        for (int x = 0; x < width && !placedPlayer; x++)
        {
            for (int y = 0; y < height && !placedPlayer; y++)
            {
                if (data[x, y]) // a walkable cell
                {
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

        if (playerAttack)
            playerAttack.enabled = true;

        Debug.Log("Maze exit reached! Boss fight enabled. Player can now attack.");

        // Show boss HP UI
        if (bossHealthPanel) bossHealthPanel.SetActive(true);
    }

    // Called if the player’s HP reaches 0 or if the boss kills the player
    public void OnPlayerDefeated()
    {
        // Only handle defeat if we're not already in defeat/victory
        if (currentState == GameState.Defeat || currentState == GameState.Victory) return;

        currentState = GameState.Defeat;
        Debug.Log("Game Over! Player has been defeated.");

        // Show defeat UI and hide boss HP
        if (defeatPanel) defeatPanel.SetActive(true);
        if (bossHealthPanel) bossHealthPanel.SetActive(false);
    }

    // Called by BossAI when boss HP <= 0
    public void OnBossDefeated()
    {
        // If we already lost, don't override with victory
        if (currentState == GameState.Defeat) return;

        currentState = GameState.Victory;
        Debug.Log("Victory! Boss was defeated!");

        // Show victory UI and hide boss HP
        if (victoryPanel) victoryPanel.SetActive(true);
        if (bossHealthPanel) bossHealthPanel.SetActive(false);
    }

    // (Optional) for UI buttons
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}