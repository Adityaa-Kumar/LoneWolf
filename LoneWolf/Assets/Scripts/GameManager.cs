using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Bars")]
    [Range(0f, 1f)]
    public float hunger = 0f;
    [Range(0f, 1f)]
    public float detect = 0f;

    [Header("Sheep Spawner Settings")]
    public GameObject sheepPrefab;
    public GameObject mapObject; // Assign your "Square" GameObject here
    public int sheepCount = 10;

    private SpriteRenderer mapRenderer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
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
    }

    // Update is called once per frame
    void FixedUpdate()
    {

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
}
