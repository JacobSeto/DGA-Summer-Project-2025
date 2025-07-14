using JetBrains.Annotations;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    [SerializeField] public Rigidbody2D playerRb;
    [SerializeField] float maxSpeed;
    [SerializeField] public float bounceForce;
    public bool lose = false;
    public bool launched = false;
    Vector2 reflectedVector;
    RaycastHit2D ray;
    Vector2 direction;
    float currentSpeed;
    Vector3 originalPos;

    [SerializeField] LayerMask bounceLayers;

    [SerializeField] TextMeshProUGUI speedText;

    //Sprites

    [SerializeField] SpriteRenderer spriteRenderer;

    [SerializeField] Sprite initialSprite;

    [SerializeField] Sprite postLaunchSprite;

    // Start is called before first frame is script is active
    void Start()
    {
        spriteRenderer.sprite = initialSprite;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerRb.linearVelocity.magnitude >= .05f)
        {
            direction = playerRb.linearVelocity.normalized;
            currentSpeed = playerRb.linearVelocity.magnitude;
            speedText.text = $"Speed: {currentSpeed:F2}";
        }
        else
        {
            // Lose Game
        }

        // initial launch with left click + drag, otherwise must activate stanima using right click
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
                    playerRb.linearVelocity = new Vector2(0,0);
                }
            }
        }

        //get if the mouse was clicked down
        //update the force based on location of mouse in comparison with original location
        //Camera.main.ScreenToWorldPoint()
        //when let go, do a calculation and apply the force
        
        // game status updates
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
    }
}
