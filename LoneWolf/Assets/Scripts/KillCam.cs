using System.Collections;
using UnityEngine;

public class KillCam : MonoBehaviour
{
    public GameObject kc;
    public GameManager GameManager;
    public Transform circleTransform; // Reference to the circle sprite child
    public float circleRadius = 2f;   // Set this to match the sprite's visual radius

    private bool isKilling = false;

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isKilling)
        {
            StartCoroutine(KillPause());
        }
    }

    public void DestroyClosestSheep()
    {
        GameObject[] sheepObjects = GameObject.FindGameObjectsWithTag("Sheep");
        if (sheepObjects.Length == 0)
            return;

        GameObject nearestSheep = null;
        float minDistance = Mathf.Infinity;
        Vector3 circleCenter = circleTransform.position;

        foreach (GameObject sheep in sheepObjects)
        {
            float distance = Vector3.Distance(sheep.transform.position, circleCenter);
            if (distance <= circleRadius && distance < minDistance)
            {
                minDistance = distance;
                nearestSheep = sheep;
            }
        }

        if (nearestSheep != null)
        {
            Destroy(nearestSheep);
        }
    }

    public IEnumerator KillPause()
    {
        isKilling = true;

        if (GameManager != null)
            GameManager.PauseGame();

        if (kc != null)
            kc.SetActive(true);

        DestroyClosestSheep();

        float pauseEndTime = Time.realtimeSinceStartup + 1f;
        while (Time.realtimeSinceStartup < pauseEndTime)
        {
            yield return null;
        }

        if (kc != null)
            kc.SetActive(false);

        if (GameManager != null)
            GameManager.ResumeGame();

        isKilling = false;
    }
}
