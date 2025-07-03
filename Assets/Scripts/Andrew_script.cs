using UnityEngine;

// Short script for the burger that enables fast falling and a dash in the direction last moved by pressing LShift
public class Andrew_script : MonoBehaviour
{
    private float horizontal;
    private float speed = 8f;
    private float jumpingPower = 10f;
    private bool isFacingRight = true;

    private float originalGravity;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    [SerializeField] private float dashSpeed = 20f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float dashCooldown = 1f;

    private bool isDashing;
    private float dashTimer;
    private float dashCooldownTimer;
    private float lastDashDirection = 1f;

    void Update()
    {
        // skip movement updates if dashing
        if (isDashing)
        {
            return;
        }

        horizontal = Input.GetAxisRaw("Horizontal");

        if (horizontal != 0)
        {
            lastDashDirection = horizontal;
        }

        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpingPower);
        }

        if (Input.GetButtonUp("Jump") && rb.linearVelocity.y > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
        }

        FastFall();

        Flip();

        if (Input.GetButtonDown("Fire3") && dashCooldownTimer <= 0f)
        {
            StartDash();
        }

        if (dashCooldownTimer > 0f)
        {
            dashCooldownTimer -= Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        if (isDashing)
        {
            rb.linearVelocity = new Vector2(lastDashDirection * dashSpeed, 0f);
            dashTimer -= Time.fixedDeltaTime;
            if (dashTimer <= 0f)
            {
                EndDash();
            }
        }
        else
        {
            rb.linearVelocity = new Vector2(horizontal * speed, rb.linearVelocity.y);
        }
    }

    // Checks if the burger currently is moving down, and if so, triples the gravity scale temporarily to simulate fast falling
    private void FastFall()
    {
        if (rb.linearVelocity.y < 0)
        {
            rb.gravityScale = 3;
        }
        else
        {
            rb.gravityScale = 1;
        }
    }

    // Checks if burger is currently touching any collider labeled as a ground
    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    // Flips orientation based on movement
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

    // Reset dash timer variables to begin dash movement, actually update is handled in fixed update
    private void StartDash()
    {
        isDashing = true;
        dashTimer = dashDuration;
        dashCooldownTimer = dashCooldown;
        rb.gravityScale = 0;
        rb.linearVelocity = new Vector2(lastDashDirection * dashSpeed, 0f);
    }

    private void EndDash()
    {
        isDashing = false;
        rb.gravityScale = 1;
    }
}
