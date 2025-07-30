using UnityEngine;

public class ZookeeperDirection : MonoBehaviour
{
    Transform nearestZookeeper;
    PlayerController player;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] float minSize = 0;
    [SerializeField] float maxXSize = 2;
    [SerializeField] float maxYSize = 1.75f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        nearestZookeeper = player.GetClosestZookeeper(GameManagerScript.Instance.GetZookeeperList());

        if (nearestZookeeper != null){
            var dir = nearestZookeeper.position - transform.position;

            float newX = Mathf.Abs(dir.magnitude) / 26.25f;
            float newY = Mathf.Abs(dir.magnitude) / 30;

            if (newX < minSize)
            {
                newX = minSize;
            }
            else if (newX > maxXSize)
            {
                newX = maxXSize;
            }

            if (newY < minSize)
            {
                newY = minSize;
            }
            else if (newY > maxYSize)
            {
                newY = maxYSize;
            }

            spriteRenderer.transform.localScale = new Vector3 (newX, newY, 1);

            var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }
}
