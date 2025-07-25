using UnityEngine;

public class CheetahController : MonoBehaviour
{

  [SerializeField] float coolDownTime = 10f;
    private float timer;
    private bool canIncreaseSpeed = true;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!canIncreaseSpeed)
        {
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                canIncreaseSpeed = true;
            }
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && canIncreaseSpeed)
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                player.playerRb.linearVelocity *= 2f;
                canIncreaseSpeed = false;
                timer = coolDownTime;
            }
        }
    }

    

}
