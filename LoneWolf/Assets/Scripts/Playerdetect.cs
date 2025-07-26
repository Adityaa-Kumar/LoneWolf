using UnityEngine;

public class PlayerDetect : MonoBehaviour
{
    public string playerTag = "Player";
    public GameManager GameManager;
    public float detectRate = 5.0f;
    public float escapeRate = 2.5f;
    private bool PlayerInVision = false;

    void FixedUpdate()
    {
        if (PlayerInVision == true && GameManager.detect < 1.0f)
        {
            GameManager.detect += detectRate * Time.fixedDeltaTime;
            GameManager.detect = Mathf.Min(GameManager.detect, 1.0f);
        }

        if (PlayerInVision == false && GameManager.detect > 0.0f)
        {
            GameManager.detect -= escapeRate * Time.fixedDeltaTime;
            GameManager.detect = Mathf.Max(GameManager.detect, 0.0f);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {
            PlayerInVision = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {
            PlayerInVision = false;
        }
    }
}
