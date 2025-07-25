using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    private TMP_Text text;
    private int clicks = 0;
    private PlayerController player;
    void Start()
    {
        text = gameObject.transform.GetChild(0).GetComponent<TMP_Text>();
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
            gameObject.SetActive(false);
        }
    }
}
