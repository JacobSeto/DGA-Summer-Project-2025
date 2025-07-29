using UnityEngine;

public class TutorialTwo : MonoBehaviour
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
        gameScreen = GameManagerScript.Instance.getGameScreen();
        speedometer = gameScreen.transform.GetChild(1).gameObject;
        speedometer.SetActive(false);
        player.speedometerExists = false;
        background = gameObject.transform.parent.gameObject;
        player.tutorialTwo = true;
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
