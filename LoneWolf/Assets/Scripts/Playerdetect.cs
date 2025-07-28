using UnityEngine;

public class PlayerDetect : MonoBehaviour
{
    [Header("Detection Settings")]
    public string playerTag = "Player";
    public float detectRate = 5.0f;
    public float escapeRate = 2.5f;
    [Range(0f, 1f)]
    public float detectValue = 0f;

    [Header("References")]
    public GameManager gameManager;

    private bool playerInVision = false;
    private bool wasDetected = false;

    void Start()
    {
        // Validate references
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
        // Only update if GameManager exists
        if (gameManager == null) return;

        bool valueChanged = false;

        if (playerInVision && detectValue < 1.0f)
        {
            detectValue += detectRate * Time.fixedDeltaTime;
            detectValue = Mathf.Clamp01(detectValue);
            valueChanged = true;

            // Check if player was just detected
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

            // Check if player escaped detection
            if (wasDetected && detectValue <= 0.0f)
            {
                wasDetected = false;
                OnPlayerEscaped();
            }
        }

        // Only update GameManager if value changed (performance optimization)
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

    // Event methods that can be overridden or extended
    protected virtual void OnPlayerDetected()
    {
        Debug.Log("Player fully detected!", this);
        // Add any additional logic when player is fully detected
    }

    protected virtual void OnPlayerEscaped()
    {
        Debug.Log("Player escaped detection!", this);
        // Add any additional logic when player escapes
    }

    // Public methods for external access
    public bool IsPlayerDetected() => detectValue >= 1.0f;
    public bool IsPlayerInVision() => playerInVision;
    public float GetDetectionProgress() => detectValue;

    // Method to reset detection (useful for game resets, respawning, etc.)
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