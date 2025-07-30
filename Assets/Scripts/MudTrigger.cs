using UnityEngine;

public class MudTrigger : MonoBehaviour
{
    // [SerializeField] private Collider2D waterTileCollider;
    [SerializeField] private float mudDampeningValue;
    Rigidbody2D playerBody;
    private float defaultDampeningValue;
    private GameObject mudParticlesObj;
    private GameObject grassParticlesObj;
    private ParticleSystem grassParticles;
    private ParticleSystem mudParticles;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerBody = GameManagerScript.Instance.player.GetComponent<Rigidbody2D>();
        defaultDampeningValue = playerBody.linearDamping;
        mudParticlesObj = GameManagerScript.Instance.player.transform.Find("MudParticles").gameObject;
        grassParticlesObj = GameManagerScript.Instance.player.transform.Find("GrassParticles").gameObject;
        mudParticles = mudParticlesObj.GetComponent<ParticleSystem>();
        grassParticles = grassParticlesObj.GetComponent<ParticleSystem>();
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
            other.attachedRigidbody.linearDamping = mudDampeningValue; 
            grassParticles.Stop();
            mudParticles.Play();
        }

    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.attachedRigidbody.linearDamping = defaultDampeningValue;
            grassParticles.Play();
            mudParticles.Stop();
        }

    }
}