using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Vector2 reflectedVector;
    Vector2 normal;
    [SerializeField] public float bounceForce;
    [SerializeField] Rigidbody2D playerRb;
    Vector3 originalPos;
    
    // Update is called once per frame
    void Update()
    {
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
        if (collision.gameObject.CompareTag("Wall"))
        {
            normal = collision.contacts[0].normal;
            reflectedVector = UnityEngine.Vector2.Reflect(playerRb.linearVelocity, normal);
            playerRb.linearVelocity = reflectedVector * bounceForce;
        }
    }
}
