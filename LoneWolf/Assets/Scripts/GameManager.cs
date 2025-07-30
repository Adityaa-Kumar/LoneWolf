using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Score")]
    public int Score;
    public TMP_Text scoreText;

    [Header("Bars")]
    [Range(0f, 1f)]
    public float hunger = 1f;  // Starting hunger can be full (1f) or as needed
    [Range(0f, 1f)]
    public float detect = 0f;

    [Header("Hunger Decrease Rate Scaling")]
    public float baseHungerDecreaseRate = 0.01f; // Minimum decrease rate when Score = 0
    public float maxHungerDecreaseRate = 0.05f;   // Maximum decrease rate when Score >= maxScoreForHungerScaling
    public int maxScoreForHungerScaling = 30;    // Adjust according to your game's max score

    [Header("Sheep Spawner Settings")]
    public GameObject sheepPrefab;
    public GameObject mapObject; // Assign your "Square" GameObject here
    public int sheepCount = 10;

    [Header("Game Over Logic")]
    public GameOverManager gameOverManager;

    [Header("Pause")]
    public PauseManager pauseManager;

    private SpriteRenderer mapRenderer;
    private float hungerDecreaseRate;

    void Start()
    {
        if (mapObject != null)
        {
            mapRenderer = mapObject.GetComponent<SpriteRenderer>();
            if (mapRenderer == null)
            {
                Debug.LogError("No SpriteRenderer found on the assigned GameObject!");
                return;
            }
        }
        else
        {
            Debug.LogError("Map GameObject not assigned!");
            return;
        }

        SpawnSheep();

        // Initialize hungerDecreaseRate at start
        hungerDecreaseRate = baseHungerDecreaseRate;

        StartCoroutine(DecreaseHungerOverTime());
    }

    void FixedUpdate()
    {
        // Dynamically update hungerDecreaseRate based on current score
        float normalizedScore = Mathf.Clamp01((float)Score / maxScoreForHungerScaling);
        hungerDecreaseRate = Mathf.Lerp(baseHungerDecreaseRate, maxHungerDecreaseRate, normalizedScore);

        // Check for Game Over condition based on detect value
        if (detect >= 1.0f || hunger <= 0.0f)
        {
            gameOverManager.ShowGameOver();
        }

        // Update the score text UI
        if (scoreText != null)
        {
            scoreText.text = "X " + Score;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pauseManager.ShowPauseScreen();
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        AudioListener.pause = true;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        AudioListener.pause = false;
    }

    public void SpawnSheep()
    {
        // Use the bounds of the map's SpriteRenderer
        Bounds bounds = mapRenderer.bounds;

        for (int i = 0; i < sheepCount; i++)
        {
            float randomX = Random.Range(bounds.min.x, bounds.max.x);
            float randomY = Random.Range(bounds.min.y, bounds.max.y);
            Vector3 spawnPosition = new Vector3(randomX, randomY, 0f);
            Instantiate(sheepPrefab, spawnPosition, Quaternion.identity);
        }
    }

    System.Collections.IEnumerator DecreaseHungerOverTime()
    {
        while (true)
        {
            hunger -= hungerDecreaseRate * Time.deltaTime;
            hunger = Mathf.Clamp01(hunger);
            yield return null;
        }
    }
}
