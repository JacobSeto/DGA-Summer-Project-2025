using UnityEngine;

public class IceTrigger : MonoBehaviour
{
    // [SerializeField] private Collider2D waterTileCollider;
    [SerializeField] private float iceDampeningValue;
    Rigidbody2D playerBody;
    private float defaultDampeningValue;
    private GameObject iceParticlesObj;
    private GameObject grassParticlesObj;
    private ParticleSystem iceParticles;
    private ParticleSystem grassParticles;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerBody = GameManagerScript.Instance.player.GetComponent<Rigidbody2D>();
        defaultDampeningValue = playerBody.linearDamping;
        iceParticlesObj = GameManagerScript.Instance.player.transform.Find("IceParticles").gameObject;
        grassParticlesObj = GameManagerScript.Instance.player.transform.Find("GrassParticles").gameObject;
        iceParticles = iceParticlesObj.GetComponent<ParticleSystem>();
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
            other.attachedRigidbody.linearDamping = iceDampeningValue;
            iceParticles.Play();
            grassParticles.Stop();
        }

    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.attachedRigidbody.linearDamping = defaultDampeningValue;
            iceParticles.Stop();
            grassParticles.Play();
        }

    }
}