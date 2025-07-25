using UnityEngine;

public class TutorialTwo : MonoBehaviour
{
    private PlayerController player;
    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        player.startingStamina = 4;
        Debug.Log(player.startingStamina);
    }

}
