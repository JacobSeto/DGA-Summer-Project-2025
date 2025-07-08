using System.Runtime.CompilerServices;
using System.Collections;
using UnityEngine;

public class JasonPlayerController : MonoBehaviour
{
    private float horizontal;
    private float speed = 4f;
    private float jumpingPower = 8f;
    private bool isTeleporting = false;
   

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Rigidbody2D indicator;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

   void Start()
    {
        Physics2D.IgnoreLayerCollision(0, 5);
        Physics2D.IgnoreLayerCollision(7, 5);
        indicator.gameObject.SetActive(false);
    }
    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpingPower);
        }

        if (Input.GetButtonUp("Jump") && rb.linearVelocity.y > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
        }
        if (Input.GetKeyDown(KeyCode.LeftShift) && isTeleporting == false)
        {
            isTeleporting = true;
            indicator.gameObject.SetActive(true);
            indicator.position = rb.position;
            indicator.linearVelocity = new Vector2(rb.linearVelocity.x * 2, rb.linearVelocity.y * 2);
            indicator.gravityScale = 0;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift) && isTeleporting == true)
        {
            isTeleporting = false;
            rb.position = indicator.position;
            indicator.gameObject.SetActive(false);
        }
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(horizontal * speed, rb.linearVelocity.y);
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }
}