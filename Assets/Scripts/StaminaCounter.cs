using TMPro;
using UnityEngine;

public class StaminaCounter : MonoBehaviour
{
    private Transform child;
    private TMP_Text text;
    [SerializeField] GameObject player;
    private PlayerController controller;
    void Start()
    {
        child = gameObject.transform.GetChild(0);
        text = child.GetComponent<TMP_Text>();
        controller = player.GetComponent<PlayerController>();
    }

    void Update()
    {
        text.SetText(controller.stamina.ToString());
    }
}
