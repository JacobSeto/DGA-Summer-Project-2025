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

    public bool inCooldown()
    {
        return !canThrow;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Player") && canThrow)
        {
            AudioManager.Instance.PlayMonkey();
            
            if(gameObject.transform.position.x > collision.transform.position.x){
                animator.SetBool("throwRight", true);
                Debug.Log("true");
            }else {
                animator.SetBool("throwRight", false);
                Debug.Log("false");
            }
            canThrow = false;
            timer = coolDownTime;
            collision.gameObject.GetComponent<PlayerController>().goInAir();
        }
    }
}
