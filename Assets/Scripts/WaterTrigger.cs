using UnityEngine;

public class WaterTrigger : MonoBehaviour
{
    // [SerializeField] private Collider2D waterTileCollider;
    [SerializeField] private float waterDampeningValue;
    Rigidbody2D playerBody;
    private float defaultDampeningValue;
    private GameObject waterParticlesObj;
    private GameObject grassParticlesObj;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerBody = GameManagerScript.Instance.player.GetComponent<Rigidbody2D>();
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
            other.attachedRigidbody.linearDamping = waterDampeningValue;
            GameManagerScript.Instance.player.SetParticles(PlayerController.ParticleTypes.Water, true);
        }
    }
 
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.attachedRigidbody.linearDamping = defaultDampeningValue; // Note: Janky if immediately entering a special tile upon exiting water 
            GameManagerScript.Instance.player.SetParticles(PlayerController.ParticleTypes.Water, false);
        }
    
    }
}
