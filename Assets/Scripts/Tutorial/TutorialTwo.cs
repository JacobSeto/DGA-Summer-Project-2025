using UnityEngine;

public class TutorialTwo : MonoBehaviour
{
    private PlayerController player;
    private GameObject gameScreen;
    private GameObject speedometer;
    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        player.startingStamina = 3;
        gameScreen = GameManagerScript.Instance.getGameScreen();
        speedometer = gameScreen.transform.GetChild(1).gameObject;
        speedometer.SetActive(false);
    }

}
