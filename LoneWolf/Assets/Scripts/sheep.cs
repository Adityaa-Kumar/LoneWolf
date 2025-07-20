using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class RandomBouncingMovement2D : MonoBehaviour
{
    [Header("Movement Settings")]
    [Tooltip("Movement speed of the character")]
    [SerializeField] private float moveSpeed = 3f;
    [Tooltip("How long to move in seconds before stopping")]
    [SerializeField] private float moveDuration = 2f;
    [Tooltip("How long to stop in seconds")]
    [SerializeField] private float stopDuration = 1f;

    private Rigidbody2D rb;
    private Vector2 moveDirection;
    private bool isMoving = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(MoveAndStopRoutine());
    }

    void FixedUpdate()
    {
        rb.linearVelocity = isMoving ? moveDirection * moveSpeed : Vector2.zero;
    }

    IEnumerator MoveAndStopRoutine()
    {
        while (true)
        {
            // Start moving in a new random direction
            PickRandomDirection();
            isMoving = true;
            yield return new WaitForSeconds(moveDuration);

            // Stop moving
            isMoving = false;
            yield return new WaitForSeconds(stopDuration);
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
