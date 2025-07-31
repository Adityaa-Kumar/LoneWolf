using UnityEngine;

public class PlayerDetect : MonoBehaviour
{
    [Header("Detection Settings")]
    public string playerTag = "Player";

    [Header("Detection Rate Scaling")]
    public float baseDetectRate = 5.0f;    // Detect rate at Score = 0
    public float maxDetectRate = 10.0f;    // Detect rate at max Score
    public float baseEscapeRate = 2.5f;    // Escape rate at Score = 0
    public float minEscapeRate = 1.0f;     // Escape rate at max Score

    [Range(0f, 1f)]
    public float detectValue = 0f;

    [Header("References")]
    public GameManager gameManager;

    private bool playerInVision = false;
    private bool wasDetected = false;

    // Current rates, updated dynamically each FixedUpdate
    private float detectRate;
    private float escapeRate;

    // Set this to your game's designed max Score for scaling (adjust as needed)
    public float maxScore = 100f;

    void Start()
    {
        // Validate GameManager reference
        if (gameManager == null)
        {
            gameManager = FindFirstObjectByType<GameManager>();
            if (gameManager == null)
            {
                Debug.LogError("GameManager reference not found! Please assign it in the inspector.", this);
            }
        }

        // Initialize GameManager detect value
        if (gameManager != null)
        {
            gameManager.detect = detectValue;
        }
    }

    void FixedUpdate()
    {
        if (gameManager == null) return;

        // Normalize Score between 0 and 1 for scaling rates
        float ScoreNormalized = Mathf.Clamp01(gameManager.Score / maxScore);

        // Calculate dynamic detection and escape rates based on Score
        detectRate = Mathf.Lerp(baseDetectRate, maxDetectRate, ScoreNormalized);
        escapeRate = Mathf.Lerp(baseEscapeRate, minEscapeRate, ScoreNormalized);

        bool valueChanged = false;

        if (playerInVision && detectValue < 1.0f)
        {
            detectValue += detectRate * Time.fixedDeltaTime;
            detectValue = Mathf.Clamp01(detectValue);
            valueChanged = true;

            if (!wasDetected && detectValue >= 1.0f)
            {
                wasDetected = true;
                OnPlayerDetected();
            }
        }
        else if (!playerInVision && detectValue > 0.0f)
        {
            detectValue -= escapeRate * Time.fixedDeltaTime;
            detectValue = Mathf.Max(detectValue, 0.0f);
            valueChanged = true;

            if (wasDetected && detectValue <= 0.0f)
            {
                wasDetected = false;
                OnPlayerEscaped();
            }
        }

        if (valueChanged)
        {
            gameManager.detect = detectValue;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {
            playerInVision = true;
            Debug.Log($"Player entered detection zone - Current detect value: {detectValue:F2}", this);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {
            playerInVision = false;
            Debug.Log($"Player exited detection zone - Current detect value: {detectValue:F2}", this);
        }
    }

    // Event methods - extend or override as needed
    protected virtual void OnPlayerDetected()
    {
        Debug.Log("Player fully detected!", this);
        // Add your detection logic here (e.g., alert enemies, trigger events)
    }

    protected virtual void OnPlayerEscaped()
    {
        Debug.Log("Player escaped detection!", this);
        // Add logic for player escaping detection here
    }

    // Public getters
    public bool IsPlayerDetected() => detectValue >= 1.0f;
    public bool IsPlayerInVision() => playerInVision;
    public float GetDetectionProgress() => detectValue;

    // Reset detection values
    public void ResetDetection()
    {
        detectValue = 0f;
        playerInVision = false;
        wasDetected = false;

        if (gameManager != null)
        {
            gameManager.detect = detectValue;
        }
    }
}