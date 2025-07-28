using UnityEngine;
using UnityEngine.UI;

public class SlowVisual : MonoBehaviour
{
    private PlayerController player;
    private Image image;
    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        image = gameObject.GetComponent<Image>();
    }

    void Update()
    {
        if (player.slowMotion)
        {
            Color color = image.color;
            color.a = 0.5f;
            image.color = color;
        }
        else
        {
            Color color = image.color;
            color.a = 0f;
            image.color = color;
        }
    }
}
