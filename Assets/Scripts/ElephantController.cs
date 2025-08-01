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
        
        if (!usable)
        {
            timer -= Time.deltaTime * 2;
            if (timer <= 0f)
            {
                timer = 1;
                usable = true;
            }
        }
        animator.SetBool("usable", usable); 
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("collision");
        if (collision.gameObject.CompareTag("Player"))
        {
            AudioManager.Instance.PlayElephant();
        
            if (animator != null)
            {
                usable = false;
            }
        }
    }
}
