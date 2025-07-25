using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Tutorial : MonoBehaviour
{
    private int clicks = 0;
    private PlayerController player;
    [SerializeField] private GameObject background;
    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        player.startingStamina = 1;
    }

    void Update()
    {
        if (clicks == 0)
        {
            GameManagerScript.Instance.HideGameMenu();
        }
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
