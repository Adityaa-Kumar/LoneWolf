using UnityEngine;

public class PlayerDetect : MonoBehaviour
{
    [Header("Detection Settings")]
    public string playerTag = "Player";

    [Header("Rotation Settings")]
    public float minRotationSpeed = 60f;  // Minimum random speed
    public float maxRotationSpeed = 180f; // Maximum random speed

    private float rotationSpeed;
    private Transform parentTransform;

    private void Start()
    {
        // Cache the parent transform (the sprite)
        parentTransform = transform.parent;
        if (parentTransform == null)
        {
            Debug.LogWarning("VisionTrigger: No parent found. Vision cone will not rotate.");
        }

        // Assign a random rotation speed
        rotationSpeed = Random.Range(minRotationSpeed, maxRotationSpeed);
    }

    private void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {
            Debug.Log("Player entered vision cone!");
            // Trigger behavior, e.g., alert NPC
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {
            Debug.Log("Player left vision cone.");
            // Stop alert, etc.
        }
    }

    public void rotate()
    {
        if (parentTransform != null)
        {
            // Rotate around the parent (sprite's) position on Z axis
            transform.RotateAround(parentTransform.position, Vector3.forward, rotationSpeed * Time.deltaTime);
        }
    }
}
