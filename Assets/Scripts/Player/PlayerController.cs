using JetBrains.Annotations;
using UnityEngine;
using TMPro;
using UnityEditor.Tilemaps;

public class PlayerController : MonoBehaviour
{
    [SerializeField] public Rigidbody2D playerRb;
    [SerializeField] float maxLaunchSpeed;

    /// <summary>
    /// Max speed that the armidillo can be at any moment(including boosts/powerups)
    /// </summary>
    [SerializeField] float maxSpeed;
    [SerializeField] float minSpeed;
    [SerializeField] public float bounceForce;
    [SerializeField] public float rotateForce;
    [SerializeField] int stamina;
    [SerializeField] public float slowDownAmount;
    public bool lose = false;
    public bool launched = false;
    public bool slowMotion = false;
    Vector2 reflectedVector;
    RaycastHit2D ray;
    Vector2 direction;
    float currentSpeed;
    Vector3 originalPos;
    float angle;
    private int maxStamina;
    float flip = 1;

    // Trajectory/stretching variables
    private bool stretching = false;
    float dragDistance;
    public bool IsStretching => stretching;
    public Vector3 OriginalMousePos => originalPos;
    
    // Audio
    [SerializeField] AudioManager audioManager;
    private bool played = false;

    [SerializeField] LayerMask bounceLayers;
    [SerializeField] GameObject pivot;

    // Sprites

    [SerializeField] SpriteRenderer spriteRenderer;

    [SerializeField] GameObject spriteObject;

    [SerializeField] Sprite initialSprite;

    [SerializeField] Sprite postLaunchSprite;

    // Start is called before first frame is script is active
    void Start()
    {
        spriteRenderer = spriteObject.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = initialSprite;
        audioManager = AudioManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        // initial launch with left click + drag, otherwise must activate stamina using right click
        if (!lose && (!launched || stamina > 0))
        {
            if (Input.GetMouseButtonDown(0))
            {
                originalPos = Input.mousePosition;
                //store initial mouse location
                audioManager.PlayPull();
                stretching = true;
                if (launched)
                {
                    slowMotion = true;
                    Time.timeScale = slowDownAmount;
                    Time.fixedDeltaTime = 0.02F * Time.timeScale;
                }
            }
            if (stretching)
            {
                // for trajectory UI
                Vector3 currentMousePos = Input.mousePosition;
                dragDistance = Vector3.Distance(currentMousePos, originalPos);
                Debug.Log($"Drag distance: {dragDistance}");
                if (!played)
                {
                    audioManager.PlayPull();
                    played = true;
                }
            }
            if (Input.GetMouseButtonUp(0))
            {
                played = false;
                stretching = false;
                if (slowMotion)
                {
                    Time.timeScale = 1;
                    Time.fixedDeltaTime = 0.02F;
                }
                audioManager.PlayRelease();
                float xChange = -(Input.mousePosition.x - originalPos.x) / 10;
                float yChange = -(Input.mousePosition.y - originalPos.y) / 10;
                playerRb.linearVelocity = new Vector2(xChange, yChange);
                if (launched)
                {
                    DecrementStamina();
                }
                // is this a race condition? someitmes you instalose
                if (playerRb.linearVelocity.magnitude > maxLaunchSpeed) {
                    playerRb.linearVelocity = Vector2.ClampMagnitude(playerRb.linearVelocity, maxLaunchSpeed);
                    spriteRenderer.sprite = postLaunchSprite;
                } else if (playerRb.linearVelocity.magnitude > 0.2 * maxLaunchSpeed) {
                    spriteRenderer.sprite = postLaunchSprite;
                }
                else {
                    // launch force too low, enforce minimum launch speed
                    playerRb.linearVelocity = new Vector2(xChange + maxLaunchSpeed * 0.2f, yChange + maxLaunchSpeed * 0.2f);
                    spriteRenderer.sprite = postLaunchSprite;
                }
                launched = true;

            }
        }
        if (playerRb.linearVelocity.magnitude >= minSpeed) {
                direction = playerRb.linearVelocity.normalized;
                currentSpeed = playerRb.linearVelocity.magnitude;
                angle = Mathf.Clamp01(currentSpeed / maxLaunchSpeed);
                angle = Mathf.Lerp(-90f, 90f, angle);
                pivot.transform.rotation = Quaternion.Euler(0f, 0f, -angle);
                if (playerRb.linearVelocityX < 0) {
                    spriteObject.transform.Rotate(0, 0, currentSpeed * Time.deltaTime * rotateForce * flip);
                }
                else if (playerRb.linearVelocityX > 0) {
                    spriteObject.transform.Rotate(0, 0, -currentSpeed * Time.deltaTime * rotateForce * flip);
                }
            } else if (launched) {
                lose = true;
                playerRb.linearVelocity = Vector2.zero;
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

        // Animal Controller Collisions
        if (collision.gameObject.CompareTag("Insect"))
        {
            currentSpeed *= 2f;
        }
        if (collision.gameObject.CompareTag("Tranquilizer"))
        {
            DecrementStamina();
        }
        if (collision.gameObject.CompareTag("Enemy"))
            {
                currentSpeed *= 0.5f;
            }
        ElephantController elephant = collision.gameObject.GetComponent<ElephantController>();
        elephant?.DecreaseHP();

        if (collision.gameObject.CompareTag("Cheetah"))
        {
            stamina++;
        }

        ContactPoint2D contact = collision.GetContact(0);
        Vector2 normal = contact.normal;
        if (Mathf.Abs(normal.y) > 0.5)
        {
            flip = flip * -1;
        }
        audioManager.PlayBounce();
        
    }

    /// <summary>
    /// How far from the original click has the player dragged the mouse(for trajectory UI)?
    /// </summary>
    /// <returns>float representing world distance</returns>
    public float getDragDistance()
    {
        return dragDistance;
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
    /// Set velocity given new x velocity and new y velocity
    /// playerRb is public so this prob isn't needed
    /// </summary>
    public void setVelocity(float x, float y)
    {
        playerRb.linearVelocity = new Vector2(x, y);
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
