using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    private TMP_Text text;
    private int clicks = 0;
    private PlayerController player;
    void Start()
    {
        text = gameObject.transform.GetChild(0).GetComponent<TMP_Text>();
        player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
    }

    void Update()
    {
        if (clicks == 0)
        {
            player.Freeze();
        }
        if (Input.GetMouseButtonUp(0))
        {
            clicks = clicks + 1;
        }
        if (clicks == 1)
        {
            text.SetText("Launch her at the zookeeper by clicking " +
                "& dragging until the arrow points " +
                "where you want to go. Then, let go!");
            clicks = clicks + 1;
        }
        if (clicks == 2)
        {
            player.Unfreeze();
        }
        if (clicks == 3)
        {
            text.gameObject.SetActive(false);
            gameObject.SetActive(false);
        }
    }
}
