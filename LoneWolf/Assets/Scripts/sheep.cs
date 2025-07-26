using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class Sheep : MonoBehaviour
{
    [Header("Movement Settings")]
    [Tooltip("Movement speed of the character")]
    [SerializeField] private float moveSpeed = 3f;
    [Tooltip("How long to move in seconds before stopping")]
    [SerializeField] private float moveDuration = 2f;

    private Rigidbody2D rb;
    private Vector2 moveDirection;
    private bool isMoving = true;

    // Reference to your FOV triangle GameObject (assign in Inspector)
    [SerializeField] private Transform fovTriangle;

    // Add this line to store the target FOV rotation
    private Quaternion targetFOVRotation;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(MoveAndWaitRoutine());

        // Initialize targetFOVRotation
        float initialAngle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
        targetFOVRotation = Quaternion.Euler(0f, 0f, initialAngle);
    }

    void FixedUpdate()
    {
        rb.linearVelocity = isMoving ? moveDirection * moveSpeed : Vector2.zero;

        if (isMoving && fovTriangle != null)
        {
            // Calculate target angle based on move direction
            float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
            targetFOVRotation = Quaternion.Euler(0f, 0f, angle);

            // Smoothly rotate towards the target rotation
            float smoothSpeed = 5f; // Adjust for faster/slower rotation
            fovTriangle.localRotation = Quaternion.Lerp(fovTriangle.localRotation, targetFOVRotation, Time.fixedDeltaTime * smoothSpeed);
        }
    }

    IEnumerator MoveAndWaitRoutine()
    {
        while (true)
        {
            // Pick a new random direction and move
            PickRandomDirection();
            isMoving = true;
            yield return new WaitForSeconds(moveDuration);

            // Stop moving
            isMoving = false;
            float waitTime = Random.Range(7f, 10f); // Wait 7-10 seconds
            yield return new WaitForSeconds(waitTime);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Reflect move direction on collision while moving
        if (isMoving)
        {
            Vector2 normal = collision.contacts[0].normal;
            moveDirection = Vector2.Reflect(moveDirection, normal).normalized;
        }
    }

    void PickRandomDirection()
    {
        float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
        moveDirection = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)).normalized;
    }

    public void SetSpeed(float newSpeed)
    {
        moveSpeed = Mathf.Max(0, newSpeed);
    }
}