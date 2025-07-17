using TMPro;
using UnityEngine;

public class StaminaCounter : MonoBehaviour
{
    private Transform child;
    private TMP_Text text;
    private GameObject player;
    private PlayerController controller;
    void Start()
    {
        child = gameObject.transform.GetChild(0);
        text = child.GetComponent<TMP_Text>();
        player = GameObject.FindWithTag("Player");
        controller = player.GetComponent<PlayerController>();
    }

    void Update()
    {
        text.SetText(controller.stamina.ToString());
    }
}
