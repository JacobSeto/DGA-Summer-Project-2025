using UnityEngine;

public class ZookeeperController : MonoBehaviour
{
    [SerializeField] GameObject zooKeeper;
    private PlayerController player;
    [SerializeField] Animator animator;

    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
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
    }

    private void destroyZookeeper()
    {
        zooKeeper.SetActive(false);
        GameManagerScript.Instance.decrementZookeeper();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            destroyZookeeper();
        }
    }
}
