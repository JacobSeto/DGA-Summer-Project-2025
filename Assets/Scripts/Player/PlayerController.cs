using JetBrains.Annotations;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    [SerializeField] public Rigidbody2D playerRb;
    [SerializeField] float maxSpeed;
    [SerializeField] float minSpeed;
    [SerializeField] public float bounceForce;
    [SerializeField] public float rotateForce;
    public bool lose = false;
    public bool launched = false;
    public bool slowMotion = false;
    Vector2 reflectedVector;
    RaycastHit2D ray;
    Vector2 direction;
    float currentSpeed;
    Vector3 originalPos;

    [SerializeField] LayerMask bounceLayers;

    [SerializeField] TextMeshProUGUI speedText;

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
            speedText.text = $"Speed: {currentSpeed:F2}";
        }
        else if(launched && playerRb.linearVelocity.magnitude == 0)
        {
            lose = true;
            playerRb.linearVelocity = Vector2.zero;
        }

        if (Input.GetMouseButton(1))
        {
            slowMotion = !slowMotion;
        }


        if (!lose && (!launched || slowMotion))
        {
            if (Input.GetMouseButtonDown(0))
            {
                originalPos = Input.mousePosition;
                //store initial mouse location
            }
            if (Input.GetMouseButtonUp(0))
            {
                float xChange = -(Input.mousePosition.x - originalPos.x) / 10;
                float yChange = -(Input.mousePosition.y - originalPos.y) / 10;
                playerRb.linearVelocity = new Vector2(xChange, yChange);
                launched = true;
                spriteRenderer.sprite = postLaunchSprite;
            }
            spriteObject.transform.Rotate(0, 0, currentSpeed * Time.deltaTime * rotateForce);
        }
    }

    private void FixedUpdate()
    {
        if (playerRb.linearVelocity.magnitude > maxSpeed)
        {
            playerRb.linearVelocity = Vector2.ClampMagnitude(playerRb.linearVelocity, maxSpeed);
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
