using UnityEngine;

public class CheetahController : MonoBehaviour
{
    [SerializeField] float coolDownTime = 10f;
    private float timer;
    private bool canIncreaseStamina = true;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!canIncreaseStamina)
        {
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                canIncreaseStamina = true;
            }
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && canIncreaseStamina)
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                player.IncrementStamina();
                canIncreaseStamina = false;
                timer = coolDownTime;
                Debug.Log("Stamina added. Cooldown started.");
            }
        }
    }
}
