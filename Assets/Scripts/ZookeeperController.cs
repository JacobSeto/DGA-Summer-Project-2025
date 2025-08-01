using UnityEngine;

public class ZookeeperController : MonoBehaviour
{
    [SerializeField] GameObject zooKeeper;
    private PlayerController player;
    [SerializeField] Animator animator;

    private bool isFalling = false;
    private float fallSpeed = 5f;

    private BoxCollider2D boxCollider;

    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        if (player.slowMotion)
        {
            animator.SetBool("Highlight", true);
        }
        else
        {
            animator.SetBool("Highlight", false);
        }
        if (isFalling)
        {
            transform.position += Vector3.down * fallSpeed * Time.deltaTime;

            if (transform.position.y < -10f)
            {
                gameObject.SetActive(false);
            }
        }
        else
        {
            animator.SetBool("Fallen", false);

        }
    }

    private void destroyZookeeper()
    {
        GameManagerScript.Instance.decrementZookeeper();
        isFalling = true;
        animator.SetBool("Fallen", true);
        if (boxCollider != null)
        {
            boxCollider.enabled = false;
        }
        


    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            AudioManager.Instance.PlayZookeeper();
            destroyZookeeper();
        }
    }
}
