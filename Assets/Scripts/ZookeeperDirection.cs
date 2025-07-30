using UnityEngine;

public class ZookeeperDirection : MonoBehaviour
{
    Transform nearestZookeeper;
    PlayerController player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        nearestZookeeper = player.GetClosestZookeeper(GameManagerScript.Instance.GetZookeeperList());

        var dir = nearestZookeeper.position - transform.position;

        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
}
