using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5;
    public Rigidbody2D rb;

    // FixedUpdate is called 50 times per second
    void FixedUpdate()
    {
        float horizantal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        rb.linearVelocity = new Vector2 (horizantal, vertical) * speed;
    }
}
