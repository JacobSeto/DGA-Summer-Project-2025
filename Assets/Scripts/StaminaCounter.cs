using TMPro;
using UnityEngine;

public class StaminaCounter : MonoBehaviour
{

    public GameObject bugJuicePrefab;
    private int maxStamina;
    private int lastStamina = -1;
    private Transform staminaContainer;
    private GameObject player;
    private PlayerController controller;
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        controller = player.GetComponent<PlayerController>();
        staminaContainer = transform.GetChild(0);
        maxStamina = controller.GetMaxStamina();

        for (int i = 0; i < maxStamina; i++)
        {
            GameObject juice = Instantiate(bugJuicePrefab, staminaContainer);
            juice.SetActive(false);
        }

        UpdateStamina();
    }

    void Update()
    {
        if (controller.GetStaminaCount() != lastStamina)
        {
            UpdateStamina();
        }
    }

    void UpdateStamina()
    {
        int currentStamina = controller.GetStaminaCount();
        lastStamina = currentStamina;

        for (int i = 0; i < staminaContainer.childCount; i++)
        {
            staminaContainer.GetChild(i).gameObject.SetActive(i < currentStamina);
        }
    }
}
