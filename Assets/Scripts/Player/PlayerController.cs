using JetBrains.Annotations;
using UnityEngine;
using TMPro;
using UnityEditor.Tilemaps;

public class PlayerController : MonoBehaviour
{
    [SerializeField] public Rigidbody2D playerRb;
    [SerializeField] float maxSpeed;
    [SerializeField] float minSpeed;
    [SerializeField] public float bounceForce;
    [SerializeField] public float rotateForce;
    public bool lose = false;
    public bool launched = false;
    Vector2 reflectedVector;
    RaycastHit2D ray;
    Vector2 direction;
    float currentSpeed;
    Vector3 originalPos;
    float angle;

    private int stamina;

    private int maxStamina;

    float flip = 1;

    [SerializeField] LayerMask bounceLayers;
    [SerializeField] GameObject pivot;

    //Sprites

    [SerializeField] SpriteRenderer spriteRenderer;

    [SerializeField] GameObject spriteObject;

    [SerializeField] Sprite initialSprite;

    [SerializeField] Sprite postLaunchSprite;

    // Start is called before first frame is script is active
    void Start()
    {
        spriteRenderer = spriteObject.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = initialSprite;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerRb.linearVelocity.magnitude >= minSpeed || !launched)
        {
            direction = playerRb.linearVelocity.normalized;
            currentSpeed = playerRb.linearVelocity.magnitude;
            angle = Mathf.Clamp01(currentSpeed / maxSpeed);
            angle = Mathf.Lerp(-90f, 90f, angle);
            pivot.transform.rotation = Quaternion.Euler(0f, 0f, -angle);
        }
        else
        {
            lose = true;
            playerRb.linearVelocity = Vector2.zero;
        }

        // initial launch with left click + drag, otherwise must activate stamina using right click
        if (!launched)
        {
            if (Input.GetMouseButtonDown(0))
            {
                originalPos = Input.mousePosition;
                //store initial mouse location
            }
            // if (Input.GetMouseButton(0))
            // {

            // }
            if (Input.GetMouseButtonUp(0))
            {
                float xChange = -(Input.mousePosition.x - originalPos.x) / 10;
                float yChange = -(Input.mousePosition.y - originalPos.y) / 10;
                playerRb.linearVelocity = new Vector2(xChange, yChange);
                if (playerRb.linearVelocity.magnitude > 0.2 * maxSpeed)
                {
                    launched = true;
                    spriteRenderer.sprite = postLaunchSprite;
                }
                else
                {
                    // indicate to player that launch force was too low!
                    playerRb.linearVelocity = new Vector2(0, 0);
                }
            }
        }
        if (playerRb.linearVelocityX < 0)
        {
            spriteObject.transform.Rotate(0, 0, currentSpeed * Time.deltaTime * rotateForce * flip);
        }
        else if (playerRb.linearVelocityX > 0)
        {
            spriteObject.transform.Rotate(0, 0, -currentSpeed * Time.deltaTime * rotateForce * flip);
        }
        //get if the mouse was clicked down
        //update the force based on location of mouse in comparison with original location
        //Camera.main.ScreenToWorldPoint()
        //when let go, do a calculation and apply the force
        if (launched && playerRb.linearVelocity.magnitude == 0)
        {
            lose = true;
        }
        
    }

    private void FixedUpdate()
    {
        if (currentSpeed > maxSpeed)
        {
            playerRb.linearVelocity = Vector2.ClampMagnitude(playerRb.linearVelocity, maxSpeed);
        }
        else if (currentSpeed < 0.005 * maxSpeed)
        {
            playerRb.linearVelocity = Vector2.ClampMagnitude(playerRb.linearVelocity, 0);
        }
        else if (currentSpeed < 0.15 * maxSpeed)
        {
            float decay = Mathf.Lerp(1f, 0.94f, Mathf.Exp(currentSpeed - (0.05f * maxSpeed)));
            playerRb.linearVelocity *= decay;
            // Vector2 oppositeForce = -playerRb.linearVelocity.normalized * decelerationForce;
            // playerRb.AddForce(oppositeForce);
        }
    }
    
    void OnCollisionEnter2D(Collision2D collision)
    {
        if ((bounceLayers.value & (1 << collision.gameObject.layer)) > 0)
        {
            ray = Physics2D.Raycast(transform.position, direction, 4f, bounceLayers.value);
            if (ray)
            {
                Debug.Log(currentSpeed);
                reflectedVector = UnityEngine.Vector2.Reflect(direction * currentSpeed, ray.normal);
                playerRb.linearVelocity = reflectedVector * bounceForce;
            }
        }
        ContactPoint2D contact = collision.GetContact(0);
        Vector2 normal = contact.normal;
        if (Mathf.Abs(normal.y) > 0.5)
        {
            flip = flip * -1;
        }
    }

    /// <summary>
    /// Change the max speed (in flight)
    /// Does not affect the max speed the Armadillo is launched at
    /// </summary>
    public void ChangeMaxSpeed(float newSpeed)
    {
        maxSpeed = newSpeed;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns> Returns magnitude of player velocity </returns>
    public float GetCurrentSpeed()
    {
        return currentSpeed;
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <returns> Returns current stamina count for player </returns>
    public int GetStaminaCount()
    {
        return stamina;
    }

    /// <summary>
    /// Decrements current stamina by 1
    /// </summary>
    public void DecrementStamina()
    {
        if (stamina > 0) stamina--;
    }
    
    /// <summary>
    /// Increments current stamina by 1
    /// </summary>
    public void IncrementStamina()
    {
        if (stamina < maxStamina) stamina++;
    }
}
