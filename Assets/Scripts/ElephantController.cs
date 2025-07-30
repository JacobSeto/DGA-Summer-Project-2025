using UnityEngine;

public class ElephantController : MonoBehaviour
{
    private Animator animator;
    private float timer;
    private bool usable;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        usable = true;
        timer = 1;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetBool("usable", usable); 
        if (!usable)
        {
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                timer = 1;
                usable = true;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("collision");
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                usable = false;
            }
        }
    }
}
