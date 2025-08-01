using UnityEngine;

public class MudTrigger : MonoBehaviour
{
    // [SerializeField] private Collider2D waterTileCollider;
    [SerializeField] private float mudDampeningValue;
    Rigidbody2D playerBody;
    private float defaultDampeningValue;
    private GameObject mudParticlesObj;
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

    void OnTriggerEnter2D(Collider2D other)
    { // Possibly check if player is the one colliding?
        // other.attachedRigidbody.AddForce(-0.75f * other.attachedRigidbody.linearVelocity);
        if (other.CompareTag("Player"))
        {
            other.attachedRigidbody.linearDamping = mudDampeningValue;
            GameManagerScript.Instance.player.SetParticles(PlayerController.ParticleTypes.Mud, true);
            GameManagerScript.Instance.player.SetParticles(PlayerController.ParticleTypes.Grass, false);
        }

    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.attachedRigidbody.linearDamping = defaultDampeningValue;
            GameManagerScript.Instance.player.SetParticles(PlayerController.ParticleTypes.Mud, false);
            GameManagerScript.Instance.player.SetParticles(PlayerController.ParticleTypes.Grass, true);
        }

    }
}