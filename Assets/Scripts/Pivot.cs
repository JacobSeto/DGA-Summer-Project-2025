using UnityEngine;

public class Pivot : MonoBehaviour
{
    private GameObject player;
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        player.GetComponent<PlayerController>().addPivot(gameObject);
    }
}
