using System.Collections;
using UnityEngine;

public class KillCam : MonoBehaviour
{
    [Header("Kill Cam Settings")]
    public GameObject kc;
    public GameManager gameManager;
    public Transform circleTransform; // Reference to the circle sprite child
    public float circleRadius = 2f;   // Set this to match the sprite's visual radius
    private bool isKilling = false;
    public float hungerIncrement = 0.2f;

    [Header("Debug Settings")]
    public bool showDebugCircle = true;
    public Color debugCircleColor = Color.red;
    public Color debugCircleColorWhenKilling = Color.yellow;

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

        if (circleTransform == null)
        {
            Debug.LogWarning("Circle Transform not assigned! Using this transform instead.", this);
            circleTransform = transform;
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isKilling)
        {
            StartCoroutine(KillPause());
        }
    }

    void OnDrawGizmos()
    {
        if (!showDebugCircle || circleTransform == null) return;

        // Set gizmo color based on killing state
        Gizmos.color = isKilling ? debugCircleColorWhenKilling : debugCircleColor;

        // Draw the kill radius circle using multiple line segments
        DrawGizmoCircle(circleTransform.position, circleRadius);

        // Draw a slightly transparent filled circle for better visibility
        Color fillColor = Gizmos.color;
        fillColor.a = 0.1f;
        Gizmos.color = fillColor;
        Gizmos.DrawSphere(circleTransform.position, circleRadius);

        // Show sheep within range when killing
        if (isKilling)
        {
            GameObject[] sheepObjects = GameObject.FindGameObjectsWithTag("Sheep");
            Vector3 circleCenter = circleTransform.position;

            foreach (GameObject sheep in sheepObjects)
            {
                if (sheep == null) continue;

                float distance = Vector3.Distance(sheep.transform.position, circleCenter);
                if (distance <= circleRadius)
                {
                    // Draw line to sheep in range
                    Gizmos.color = Color.green;
                    Gizmos.DrawLine(circleCenter, sheep.transform.position);

                    // Draw small sphere at sheep position
                    Gizmos.DrawWireSphere(sheep.transform.position, 0.2f);
                }
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        if (circleTransform == null) return;

        // Always show detailed gizmos when selected
        Gizmos.color = Color.cyan;
        DrawGizmoCircle(circleTransform.position, circleRadius);

        // Show all sheep and their distances
        GameObject[] sheepObjects = GameObject.FindGameObjectsWithTag("Sheep");
        Vector3 circleCenter = circleTransform.position;

        foreach (GameObject sheep in sheepObjects)
        {
            if (sheep == null) continue;

            float distance = Vector3.Distance(sheep.transform.position, circleCenter);
            bool inRange = distance <= circleRadius;

            // Color code: green if in range, red if out of range
            Gizmos.color = inRange ? Color.green : Color.red;
            Gizmos.DrawLine(circleCenter, sheep.transform.position);

            // Draw distance text (only visible in Scene view)
#if UNITY_EDITOR
            UnityEditor.Handles.color = Gizmos.color;
            UnityEditor.Handles.Label(sheep.transform.position + Vector3.up * 0.5f,
                                    $"Dist: {distance:F1}");
#endif
        }
    }

    // Helper method to draw a circle using Gizmos
    private void DrawGizmoCircle(Vector3 center, float radius, int segments = 64)
    {
        if (segments < 3) segments = 3;

        float angleStep = 360f / segments;
        Vector3 prevPoint = center + new Vector3(radius, 0, 0);

        for (int i = 1; i <= segments; i++)
        {
            float angle = i * angleStep * Mathf.Deg2Rad;
            Vector3 nextPoint = center + new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, 0);
            Gizmos.DrawLine(prevPoint, nextPoint);
            prevPoint = nextPoint;
        }
    }

    public void DestroyClosestSheep()
    {
        GameObject[] sheepObjects = GameObject.FindGameObjectsWithTag("Sheep");
        if (sheepObjects.Length == 0)
        {
            Debug.Log("No sheep found to destroy");
            return;
        }

        GameObject nearestSheep = null;
        float minDistance = Mathf.Infinity;
        Vector3 circleCenter = circleTransform.position;

        foreach (GameObject sheep in sheepObjects)
        {
            if (sheep == null) continue;

            float distance = Vector3.Distance(sheep.transform.position, circleCenter);
            if (distance <= circleRadius && distance < minDistance)
            {
                minDistance = distance;
                nearestSheep = sheep;
            }
        }

        if (nearestSheep != null)
        {
            Debug.Log($"Destroying sheep at distance: {minDistance:F2}", nearestSheep);
            Destroy(nearestSheep);

            if (gameManager != null)
            {
                gameManager.PauseGame();
                gameManager.hunger += hungerIncrement;
                Debug.Log($"Hunger increased by {hungerIncrement}. Current hunger: {gameManager.hunger}");
            }

            if (kc != null)
            {
                kc.SetActive(true);
            }
        }
        else
        {
            Debug.Log($"No sheep within kill radius ({circleRadius})");
        }
    }

    public IEnumerator KillPause()
    {
        isKilling = true;
        Debug.Log("Kill sequence started");

        DestroyClosestSheep();

        float pauseEndTime = Time.realtimeSinceStartup + 1f;
        while (Time.realtimeSinceStartup < pauseEndTime)
        {
            yield return null;
        }

        if (kc != null)
            kc.SetActive(false);

        if (gameManager != null)
            gameManager.ResumeGame();

        isKilling = false;
        Debug.Log("Kill sequence ended");
    }

    // Public method to adjust radius at runtime for testing
    [ContextMenu("Test Kill Range")]
    public void TestKillRange()
    {
        GameObject[] sheepObjects = GameObject.FindGameObjectsWithTag("Sheep");
        Vector3 circleCenter = circleTransform.position;

        Debug.Log($"=== Kill Range Test (Radius: {circleRadius}) ===");
        Debug.Log($"Circle Center: {circleCenter}");
        Debug.Log($"Total Sheep: {sheepObjects.Length}");

        int sheepInRange = 0;
        foreach (GameObject sheep in sheepObjects)
        {
            if (sheep == null) continue;

            float distance = Vector3.Distance(sheep.transform.position, circleCenter);
            bool inRange = distance <= circleRadius;

            if (inRange) sheepInRange++;

            Debug.Log($"Sheep '{sheep.name}': Distance {distance:F2} - {(inRange ? "IN RANGE" : "OUT OF RANGE")}");
        }

        Debug.Log($"Sheep in kill range: {sheepInRange}/{sheepObjects.Length}");
    }
}