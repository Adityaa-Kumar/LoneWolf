using UnityEngine;

public class VisionTrigger : MonoBehaviour
{
    public string playerTag = "Player";

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
