using UnityEngine;

public class VisionTrigger : MonoBehaviour
{
    [Header("Detection Settings")]
    public string playerTag = "Player";

    [Header("Rotation Settings")]
    public float rotationSpeed = 90f; // Degrees per second

    private Transform parentTransform;

    private void Start()
    {
        // Cache the parent transform (the sprite)
        parentTransform = transform.parent;
        if (parentTransform == null)
        {
            Debug.LogWarning("VisionTrigger: No parent found. Vision cone will not rotate.");
        }
    }

    private void Update()
    {
        if (parentTransform != null)
        {
            // Rotate around the parent (sprite's) position on Z axis
            transform.RotateAround(parentTransform.position, Vector3.forward, rotationSpeed * Time.deltaTime);
        }
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
}
