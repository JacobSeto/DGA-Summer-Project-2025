using UnityEngine;

public class WaterTrigger : MonoBehaviour
{
    [SerializeField] private Collider2D waterTileCollider;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
     
    }

    void OnTriggerStay2D(Collider2D other)
    { // Possibly check if player is the one colliding?
        other.attachedRigidbody.AddForce(-0.75f * other.attachedRigidbody.linearVelocity);
    }
}
