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
            text.SetText("Launch Josephine towards the zookeeper by clicking " +
                "and dragging your cursor until the arrow is pointing the " +
                "direction you want to go. Then, release! Try it out now!");
            clicks = clicks + 1;
        }
        if (clicks == 2)
        {
            player.Unfreeze();
        }
        if (clicks == 3)
        {
            text.gameObject.SetActive(false);
        }
    }
}
