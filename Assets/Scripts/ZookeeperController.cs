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
        if (Input.GetKeyUp(KeyCode.Space))
        {
            destroyZookeeper();
        }
    }

    public void destroyZookeeper()
    {
        zooKeeperSprite.enabled = false;
        zooKeeperCollider.enabled = false;
    }
}
