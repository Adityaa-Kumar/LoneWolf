using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5;
    public Rigidbody2D rb;
    private Animator animator;

    void Start()
    {
        if (rb == null)
            rb = GetComponent<Rigidbody2D>();

        animator = GetComponent<Animator>();
        if (animator == null)
            Debug.LogWarning("Animator not found on Player!");
    }

    void FixedUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector2 moveInput = new Vector2(horizontal, vertical);
        rb.linearVelocity = moveInput * speed;

        // Animation control
        bool isMoving = moveInput.magnitude > 0.01f;

        if (animator != null)
        {
            if (isMoving)
            {
                animator.speed = 1f; // Play walking animation
            }
            else
            {
                animator.speed = 0f; // Pause animation on first frame
                animator.Play("PlayerWalk", 0, 0f); // Hold on first frame
            }
        }
    }
}
