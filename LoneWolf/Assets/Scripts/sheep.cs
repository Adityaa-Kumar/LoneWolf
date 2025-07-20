using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class RandomBouncingMovement2D : MonoBehaviour
{
    [Header("Movement Settings")]
    [Tooltip("Movement speed of the character")]
    [SerializeField] private float moveSpeed = 3f;

    private Rigidbody2D rb;
    private Vector2 moveDirection;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        PickRandomDirection();
    }

    void FixedUpdate()
    {
        rb.linearVelocity = moveDirection * moveSpeed;  // Updated line
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Reflect movement direction on collision
        Vector2 normal = collision.contacts[0].normal;
        moveDirection = Vector2.Reflect(moveDirection, normal).normalized;
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
