using UnityEngine;

public class Pivot : MonoBehaviour
{
    private PlayerController player;
    public float angle;
    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
    }

    void Update()
    {
        angle = Mathf.Clamp01(player.currentSpeed / player.maxSpeed);
        angle = Mathf.Lerp(-90f, 90f, angle) * -1;
        gameObject.transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }
}
