using UnityEngine;

public class ZookeeperController : MonoBehaviour
{
    [SerializeField] GameObject zooKeeper;

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
