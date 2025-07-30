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

    [SerializeField] private Transform fovTriangle;
    private Quaternion targetFOVRotation;

    // ADD: Reference to Animator
    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        StartCoroutine(MoveAndWaitRoutine());

        float initialAngle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
        targetFOVRotation = Quaternion.Euler(0f, 0f, initialAngle);
    }

    void FixedUpdate()
    {
        rb.linearVelocity = isMoving ? moveDirection * moveSpeed : Vector2.zero;

        if (isMoving && fovTriangle != null)
        {
            float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
            targetFOVRotation = Quaternion.Euler(0f, 0f, angle);
            float smoothSpeed = 5f;
            fovTriangle.localRotation = Quaternion.Lerp(fovTriangle.localRotation, targetFOVRotation, Time.fixedDeltaTime * smoothSpeed);
        }

        // Animation control
        if (animator != null)
        {
            if (isMoving)
            {
                animator.speed = 1f; // Play the walk animation normally
            }
            else
            {
                animator.speed = 0f; // Pause animation
                animator.Play("SheepWalk", 0, 0f); // Hold on first frame of walk animation ("SheepWalk" is your animation's name)
            }
        }
    }

    IEnumerator MoveAndWaitRoutine()
    {
        while (true)
        {
            PickRandomDirection();
            isMoving = true;
            yield return new WaitForSeconds(moveDuration);

            isMoving = false;
            float waitTime = Random.Range(7f, 10f);
            yield return new WaitForSeconds(waitTime);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
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
