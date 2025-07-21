using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] public Rigidbody2D playerRb;
    [SerializeField] float maxLaunchSpeed;

    /// <summary>
    /// Max speed that the armidillo can be at any moment(including boosts/powerups)
    /// </summary>
    [SerializeField] float maxSpeed;
    [SerializeField] float minSpeed;

    /// <summary>
    /// The percentage of inertia kept after each collision with a wall
    /// </summary>
    [SerializeField] public float bounceForce;

    /// <summary>
    /// The amount of time that must pass before another collision can reduce the momentum of the player.
    /// </summary>
    const float bounceCooldown = 1.5f;

    /// <summary>
    /// Time that has passed since last bounce applied some force to the player 
    /// </summary>
    private float bounceTimer;

    /// <summary>
    /// Whether or not the player should get an impulse on a bounce
    /// </summary>
    private bool bounceImpulseActive;

    [SerializeField] public float rotateForce;
    [SerializeField] public int stamina;
    [SerializeField] public float slowDownAmount;
    public bool launched;
    public bool slowMotion;
    Vector2 reflectedVector;
    RaycastHit2D ray;
    Vector2 direction;
    float currentSpeed;
    Vector3 originalPos;
    Vector3 originalPlayerPos;
    float angle;
    private int maxStamina;
    float flip = 1;

    // acceleration variables 
    const int accelerationWindow = 10;
    private bool isAccelerating;

    // Trajectory/stretching variables
    private bool stretching;
    float dragDistance;
    public bool IsStretching => stretching;
    public Vector3 OriginalMousePos => originalPos;
    public Vector3 OriginalPlayerPos => originalPlayerPos;
    

    [SerializeField] LayerMask wallLayer;
    [SerializeField] LayerMask boundaryLayer;

    private LayerMask bounceLayers; 
    private GameObject pivot;

    // Sprites

    [SerializeField] SpriteRenderer spriteRenderer;

    [SerializeField] GameObject spriteObject;

    [SerializeField] Sprite initialSprite;

    [SerializeField] Sprite postLaunchSprite;

    [SerializeField] Animator animator;

    // Start is called before first frame is script is active
    void Start()
    {
        spriteRenderer = spriteObject.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = initialSprite;
        launched = false;
        slowMotion = false;
        stretching = false;
        bounceImpulseActive = true;
        bounceLayers = wallLayer.value | boundaryLayer.value;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(launched);
        if (!bounceImpulseActive)
        {
            bounceTimer += Time.deltaTime;
            if (bounceTimer > bounceCooldown) bounceImpulseActive = true;
        }
        // initial launch with left click + drag, otherwise must activate stamina using right click
        if ((!launched || stamina > 0))
        {
            HandleLaunch();
        }
        if (playerRb.linearVelocity.magnitude >= minSpeed) {
            direction = playerRb.linearVelocity.normalized;
            currentSpeed = playerRb.linearVelocity.magnitude;
            angle = Mathf.Clamp01(currentSpeed / maxSpeed);
            angle = Mathf.Lerp(-90f, 90f, angle);
            pivot.transform.rotation = Quaternion.Euler(0f, 0f, -angle);
            
            if (playerRb.linearVelocityX < 0)
            {
                spriteObject.transform.Rotate(0, 0, currentSpeed * Time.deltaTime * rotateForce * flip);
            }
            else if (playerRb.linearVelocityX > 0)
            {
                spriteObject.transform.Rotate(0, 0, -currentSpeed * Time.deltaTime * rotateForce * flip);
            }
        }
        else if (launched)
        {
            GameManagerScript.Instance.LoseGame();
            playerRb.linearVelocity = Vector2.zero;
        }
    }

    private void HandleLaunch()
    {
        if (Input.GetMouseButtonDown(0))
        {
            originalPos = Input.mousePosition;
            originalPlayerPos = playerRb.transform.position;
            //store initial mouse location
            AudioManager.Instance.PlayPull();
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
        }
        if (Input.GetMouseButtonUp(0))
        {
            stretching = false;
            if (slowMotion)
            {
                Time.timeScale = 1;
                Time.fixedDeltaTime = 0.02F;
            }
            AudioManager.Instance.PlayRelease();
            float xChange = -(Input.mousePosition.x - originalPos.x) / 10;
            float yChange = -(Input.mousePosition.y - originalPos.y) / 10;
            playerRb.linearVelocity = new Vector2(xChange, yChange);
            if (launched)
            {
                DecrementStamina();
            }
            
            
            if (playerRb.linearVelocity.magnitude > maxLaunchSpeed)
            {
                playerRb.linearVelocity = Vector2.ClampMagnitude(playerRb.linearVelocity, maxLaunchSpeed);
                spriteRenderer.sprite = postLaunchSprite;
            }
            else if (playerRb.linearVelocity.magnitude > 0.2 * maxLaunchSpeed)
            {
                spriteRenderer.sprite = postLaunchSprite;
            }
            else
            {
                // launch force too low, enforce minimum launch speed
                playerRb.linearVelocity = new Vector2(xChange + maxLaunchSpeed * 0.2f, yChange + maxLaunchSpeed * 0.2f);
                spriteRenderer.sprite = postLaunchSprite;
            }
            launched = true;
            animator.SetBool("Launch", launched);
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
        else if (currentSpeed < 0.33 * maxSpeed)
        {
            float decay = Mathf.Lerp(1f, 0.975f, ((0.33f * maxSpeed) - currentSpeed) / (0.33f * maxSpeed));
            playerRb.linearVelocity *= decay;
            Debug.Log(decay);
            Debug.Log((0.33f * maxSpeed) - currentSpeed);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        ContactPoint2D contact = collision.GetContact(0);

        Debug.Log(bounceLayers.value);

        // As it works right now, everything in LayerMask bounceLayers will act as a physical object the player can ricochet off of
        // As such, the impulse we apply stars its cooldown in here so player can't lose from bouncing too much in a short period of time
        if ((bounceLayers.value & (1 << collision.gameObject.layer)) > 0)
        {
            ray = Physics2D.Raycast(transform.position, direction, 4f, bounceLayers.value);
            if (ray)
            {
                Debug.Log(currentSpeed);
                reflectedVector = UnityEngine.Vector2.Reflect(direction * currentSpeed, ray.normal);

                if (bounceImpulseActive)
                {
                    // give impulse to player, reset timer
                    reflectedVector *= bounceForce;
                    bounceTimer = 0f;
                    bounceImpulseActive = false;
                }
                playerRb.linearVelocity = reflectedVector;
            }
        }

        // Animal Controller Collisions
        //if (collision.gameObject.CompareTag("Insect"))
        //{
        //    currentSpeed *= 2f;
        //}
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

        //if (collision.gameObject.CompareTag("Cheetah"))
        //{
        //    stamina++;
        //}

        // Rotation logic
        Vector2 normal = contact.normal;
        if (Mathf.Abs(normal.y) > 0.5)
        {
            flip = flip * -1;
        }

        AudioManager.Instance.PlayBounce();
    }

    // Animal Triggers
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Cheetah"))
        {
            stamina++;
        }
        if (collision.gameObject.CompareTag("Insect"))
        {
            // Debug.Log("Old Speed: " + currentSpeed);
            currentSpeed *= 2f;
            playerRb.linearVelocity *= 2f;
            // Debug.Log("New Speed: " + currentSpeed);
        }
    }

    public void addPivot(GameObject add)
    {
        pivot = add;
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
