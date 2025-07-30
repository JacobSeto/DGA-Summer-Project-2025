using UnityEngine;

public class MonkeyController : MonoBehaviour
{
    [SerializeField] float coolDownTime = 10f;
    private float timer;
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
        if (!canThrow)
        {
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                canThrow = true;

            }
        }
        animator.SetBool("usable", canThrow);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Player") && canThrow)
        {
            canThrow = false;
            timer = coolDownTime;
        }
    }
}
