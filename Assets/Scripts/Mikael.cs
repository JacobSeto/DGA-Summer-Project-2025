using UnityEngine;

public class Mikael : MonoBehaviour
{
    private float horizontal;
    private float speed = 8f;
    private float jumpingPower = 16f;
    private bool isFacingRight = true;
    private float dash = 0f;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        Flip();

        if (Input.GetButtonDown("Fire3"))
        {
            if (!isFacingRight)
            {
                dash = -10f;
            }
            else
            {
                dash = 10f;
            }
        }

        if (Input.GetButtonDown("Jump") && IsGrounded()) {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpingPower);
        } else if (Input.GetButtonDown("Jump")){
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, -jumpingPower);
        }

        if (Input.GetButtonUp("Jump") && rb.linearVelocity.y > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
        }

        
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(horizontal * speed + dash * speed, rb.linearVelocity.y);
        horizontal = 0;
        dash = 0;
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }
}
