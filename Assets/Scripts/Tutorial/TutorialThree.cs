using UnityEngine;

public class TutorialThree : MonoBehaviour
{
    private PlayerController player;
    private GameObject gameScreen;
    private GameObject speedometer;
    private GameObject background;
    private int clicks = 0;
    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        player.startingStamina = 3;
        background = gameObject.transform.parent.gameObject;
    }

    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            clicks = clicks + 1;
        }
        if (clicks == 1)
        {
            background.SetActive(false);
        }
    }

}
