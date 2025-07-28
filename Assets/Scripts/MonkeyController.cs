using UnityEngine;

public class MonkeyController : MonoBehaviour
{
    [SerializeField] GameObject monkey;
    private Animator animator;
    private bool canThrow;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        canThrow = true;
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetBool("usable", canThrow);
    }

    private void destroyMonkey()
    {
        monkey.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Player") && canThrow)
        {
            canThrow = false;
            animator.SetBool("usable", false);
            // destroyMonkey();
        }
    }
}
