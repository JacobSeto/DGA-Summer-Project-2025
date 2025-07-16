using UnityEngine;

public class WaterTrigger : MonoBehaviour
{
    // [SerializeField] private Collider2D waterTileCollider;
    [SerializeField] private float waterDampeningValue;
    [SerializeField] private Rigidbody2D playerBody;
    private float defaultDampeningValue;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        defaultDampeningValue = playerBody.linearDamping;
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    void OnTriggerStay2D(Collider2D other)
    { // Possibly check if player is the one colliding?
        // other.attachedRigidbody.AddForce(-0.75f * other.attachedRigidbody.linearVelocity);
        if (other.CompareTag("Player"))
        {
            if (!GameManagerScript.Instance.inAir()){ 
                other.attachedRigidbody.linearDamping = waterDampeningValue;
            }
            // Debug.Log(other.attachedRigidbody.linearDamping);
        }
    }
 
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.attachedRigidbody.linearDamping = defaultDampeningValue; // Note: Janky if immediately entering a special tile upon exiting water 
            // Debug.Log(other.attachedRigidbody.linearDamping);
        }
    
    }
}
