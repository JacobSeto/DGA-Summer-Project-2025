using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Vector2 reflectedVector;
    RaycastHit2D ray;
    Vector2 direction;
    float currentSpeed;

    [SerializeField] public float bounceForce;
    [SerializeField] Rigidbody2D playerRb;
    Vector3 originalPos;

    [SerializeField] LayerMask bounceLayers;
    
    // Update is called once per frame
    void Update()
    {
        if(playerRb.linearVelocity.magnitude >= .05f)
        {
            direction = playerRb.linearVelocity.normalized;
            currentSpeed = playerRb.linearVelocity.magnitude;
        }
        else
        {
            // Lose Game
        }

        if (Input.GetMouseButtonDown(0))
        {
            originalPos = Input.mousePosition;
            //store initial mouse location
        }
        if (Input.GetMouseButton(0))
        {

        }
        if (Input.GetMouseButtonUp(0))
        {
            float xChange = -(Input.mousePosition.x - originalPos.x) / 10;
            float yChange = -(Input.mousePosition.y - originalPos.y) / 10;
            playerRb.linearVelocity = new Vector2(xChange, yChange);
        }
        //get if the mouse was clicked down
        //update the force based on location of mouse in comparison with original location
        //Camera.main.ScreenToWorldPoint()
        //when let go, do a calculation and apply the force
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
