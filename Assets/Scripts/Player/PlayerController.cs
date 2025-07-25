using System.Collections;
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
    [SerializeField] public int maxStamina;

    [SerializeField] public int startingStamina;
    [SerializeField] public float slowDownAmount;
    public bool launched;
    public bool slowMotion;
    public float angle;
    public int stamina;
    Vector2 reflectedVector;
    RaycastHit2D ray;
    Vector2 direction;
    float currentSpeed;
    Vector3 originalPos;
    Vector3 originalPlayerPos;

    float flip = 1;
    private bool thrown;
    private bool isInAir;
    private bool aboveWall;
    private Vector3 inAirScale = new Vector3(2, 2, 2);
    private Vector3 defaultScale;
    float slowTime = 0.5f;
    float timeLeft;

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
    LayerMask bounceLayers;
    //[SerializeField] GameObject slowVisual;
    private GameObject pivot;

    // Sprites

    [SerializeField] SpriteRenderer spriteRenderer;

    [SerializeField] GameObject spriteObject;

    [SerializeField] Sprite initialSprite;

    [SerializeField] Sprite postLaunchSprite;

    [SerializeField] Animator animator;

    private bool wallBounce;

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
        defaultScale = spriteRenderer.transform.localScale;
        timeLeft = slowTime;
        //slowVisual.gameObject.SetActive(false);
        stamina = startingStamina;
        if(stamina == 0)
        {
            throw new System.Exception("Stamina is 0");
        }

        GameManagerScript.Instance.UpdateStaminaBar(stamina);
    }

    // Update is called once per frame
    void Update()
    {
        if (!bounceImpulseActive)
        {
            bounceTimer += Time.deltaTime;
            if (bounceTimer > bounceCooldown) bounceImpulseActive = true;
        }
        // initial launch with left click + drag, otherwise must activate stamina using right click
        if (!launched)
        {
            HandleLaunch();
        }
        else if (stamina > 0)
        {
            HandleLaunch();
            if (Input.GetButtonDown("Slow"))
            {
                SlowMotion();
            }
            if (Input.GetButtonUp("Slow"))
            {
                EndSlowMotion();
            }
        }
        if (slowMotion)
        {
            timeLeft = timeLeft - Time.deltaTime;
            if (timeLeft <= 0)
            {
                EndSlowMotion();
            }
        }
        if (playerRb.linearVelocity.magnitude >= minSpeed)
        {
            direction = playerRb.linearVelocity.normalized;
            currentSpeed = playerRb.linearVelocity.magnitude;
            angle = Mathf.Clamp01(currentSpeed / maxSpeed);
            angle = Mathf.Lerp(-90f, 90f, angle) * -1;
            pivot.transform.rotation = Quaternion.Euler(0f, 0f, angle);

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

        if (isInAir && !thrown)
        {
            thrown = true;
            StartCoroutine(MonkeyThrow());
        }
        else if (isInAir && thrown)
        {
            thrown = false;
        }

        if (aboveWall && !Physics2D.CircleCast(transform.position, 1f, Vector2.zero, Mathf.Infinity, wallLayer))
        {
            SetWallBounceActive(true);
            aboveWall = false;
        }
    }

    IEnumerator MonkeyThrow()
    {
        float timer = 0f;

        while (timer < 1)
        {
            spriteRenderer.transform.localScale = Vector3.Lerp(defaultScale, inAirScale, timer);
            timer += Time.deltaTime;
            yield return null;
        }

        timer = 0f;

        while (timer < 1)
        {
            spriteRenderer.transform.localScale = Vector3.Lerp(inAirScale, defaultScale, timer);
            timer += Time.deltaTime;
            yield return null;
        }
        spriteRenderer.transform.localScale = new Vector3(1, 1, 1);
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
        }
        if (stretching)
        {
            // for trajectory UI
            Vector3 currentMousePos = Input.mousePosition;
            dragDistance = Vector3.Distance(currentMousePos, originalPos);
        }
        if (Input.GetMouseButtonUp(0))
        {
            stretching = false;
            AudioManager.Instance.PlayRelease();
            float xChange = -(Input.mousePosition.x - originalPos.x) / 10;
            float yChange = -(Input.mousePosition.y - originalPos.y) / 10;
            playerRb.linearVelocity = new Vector2(xChange, yChange);
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
            DecrementStamina();
            animator.SetBool("Launch", true);
            launched = true;
        }
    }

    private void SlowMotion()
    {
        //slowVisual.SetActive(true);
        slowMotion = true;
        Time.timeScale = slowDownAmount;
        Time.fixedDeltaTime = 0.02F * Time.timeScale;
    }

    private void EndSlowMotion()
    {
       // slowVisual.SetActive(false);
        Time.timeScale = 1;
        Time.fixedDeltaTime = 0.02F;
        slowMotion = false;
        timeLeft = slowTime;
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
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        ContactPoint2D contact = collision.GetContact(0);

        // As it works right now, everything in LayerMask bounceLayers will act as a physical object the player can ricochet off of
        // As such, the impulse we apply stars its cooldown in here so player can't lose from bouncing too much in a short period of time
        if ((bounceLayers.value & (1 << collision.gameObject.layer)) > 0)
        {
            ray = Physics2D.Raycast(transform.position, direction, 4f, bounceLayers.value);
            if (ray)
            {
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

        if (collision.gameObject.CompareTag("Elephant"))
        {         
            ray = Physics2D.Raycast(transform.position, direction, 4f, bounceLayers.value);
            if (ray)
            {
                reflectedVector = UnityEngine.Vector2.Reflect(direction * currentSpeed, ray.normal);

                    if (bounceImpulseActive)
                    {
                        // give impulse to player, reset timer
                        reflectedVector *= bounceForce;
                        bounceTimer = 0f;
                        bounceImpulseActive = false;
                    }
                    playerRb.linearVelocity = reflectedVector * 8f;
            }
        }
        

        // Animal Controller Collisions
        //if (collision.gameObject.CompareTag("Insect"))
        //{
        //    currentSpeed *= 2f;
        //}

        // ElephantController elephant = collision.gameObject.GetComponent<ElephantController>();
        // elephant?.DecreaseHP();
        if (collision.gameObject.CompareTag("Tranquilizer"))
        {
            DecrementStamina();
        }
        if (collision.gameObject.CompareTag("Enemy"))
        {
            currentSpeed *= 0.5f;
        }
        // ElephantController elephant = collision.gameObject.GetComponent<ElephantController>();
        // elephant?.DecreaseHP();

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

    /// <summary>
    /// Activates player in air state after running into monkey
    /// </summary>
    public void goInAir()
    {
        SetWallBounceActive(false);
        StartCoroutine(AirTime());
    }

    /// <summary>
    /// Timer for player in air state after running into monkey
    /// </summary>
    IEnumerator AirTime()
    {
        isInAir = true;
        Physics2D.IgnoreLayerCollision(gameObject.layer, 7, true);
        yield return new WaitForSeconds(2);
        isInAir = false;
        Physics2D.IgnoreLayerCollision(gameObject.layer, 7, false);
        if (Physics2D.CircleCast(transform.position, 1f, Vector2.zero, Mathf.Infinity, wallLayer))
        {
            aboveWall = true;
        }
        else
        {
            SetWallBounceActive(true);
        }
    }

    /// <summary>
    /// Whether player is in the air or not
    /// </summary>
    public bool inAir()
    {
        return isInAir;
    }

    /// <summary>
    /// Set whether or not inner walls(not boundaries) can bounce player
    /// Set to false when player is airborne
    /// </summary>
    /// <param name="flag">True to allow wall bounce, false to turn off wall bounce</param>
    public void SetWallBounceActive(bool flag)
    {
        // wallBounce = flag;
        int playerLayer = gameObject.layer;
        int wallLayerIndex = Mathf.RoundToInt(Mathf.Log(wallLayer, 2));

        Physics2D.IgnoreLayerCollision(playerLayer, wallLayerIndex, !flag);
    }

    // Animal Triggers
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Insect"))
        {
            IncrementStamina();
        }

         if (collision.gameObject.CompareTag("Elephant"))
        {  
            ray = Physics2D.Raycast(transform.position, direction, 4f, bounceLayers.value);
            if (ray)
            {
                reflectedVector = UnityEngine.Vector2.Reflect(direction * currentSpeed, ray.normal);

                    if (bounceImpulseActive)
                    {
                        // give impulse to player, reset timer
                        reflectedVector *= bounceForce;
                        bounceTimer = 0f;
                        bounceImpulseActive = false;
                    }
                    playerRb.linearVelocity = reflectedVector * 8f;
            }
        }
        if (collision.gameObject.CompareTag("Monkey"))
        {
            goInAir();
        }
    }

       public void Freeze()
    {
        enabled = false;
    }

    public void Unfreeze()
    {
        enabled = true;
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
    /// 
    /// </summary>
    /// <returns>Max stamina the player can have</returns>
    public int GetMaxStamina()
    {
        return maxStamina;
    }

    /// <summary>
    /// Decrements current stamina by 1
    /// </summary>
    public void DecrementStamina()
    {
        if (stamina > 0) stamina--;
        GameManagerScript.Instance.UpdateStaminaBar(stamina);
    }

    /// <summary>
    /// Increments current stamina by 1
    /// </summary>
    public void IncrementStamina()
    {
        if (stamina < maxStamina) stamina++;
        GameManagerScript.Instance.UpdateStaminaBar(stamina);
    }
}
