using UnityEngine;

public class TutorialTwo : MonoBehaviour
{
    private PlayerController player;
    [SerializeField] private GameObject background;
    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        player.startingStamina = 1;
    }

}
