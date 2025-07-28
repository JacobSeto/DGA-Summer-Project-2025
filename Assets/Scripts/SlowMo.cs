using UnityEngine;

public class SlowMo : MonoBehaviour
{
    private GameObject player;
    private bool slowMo;
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        slowMo = player.GetComponent<PlayerController>().slowMotion;
    }
}
