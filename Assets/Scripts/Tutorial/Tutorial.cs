using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Tutorial : MonoBehaviour
{
    private PlayerController player;
    private GameObject background;
    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        player.startingStamina = 1;
        background = gameObject.transform.parent.gameObject;
        player.tutorial = true;
    }
}
