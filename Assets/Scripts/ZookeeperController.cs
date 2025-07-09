using UnityEngine;

public class ZookeeperController : MonoBehaviour
{
    [SerializeField] SpriteRenderer zooKeeperSprite;
    [SerializeField] BoxCollider2D zooKeeperCollider;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void destroyZookeeper()
    {
        zooKeeperSprite.enabled = false;
        zooKeeperCollider.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            destroyZookeeper();
        }
    }
}
