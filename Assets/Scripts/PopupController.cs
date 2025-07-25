using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PopupController : MonoBehaviour
{
    [SerializeField] TMP_Text dialogueText;
    [SerializeField] Button nextButton;

    [SerializeField] string[] text;
    private PlayerController player;

    private int currentIndex = 0;

    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();

        dialogueText.text = text[currentIndex];
        nextButton.onClick.AddListener(NextMessage);
    }

    void Update()
    {
        
    }

    void NextMessage()
    {
        currentIndex++;

        if (currentIndex < text.Length)
        {
            player.Freeze();
            dialogueText.text = text[currentIndex];
        }
        else
        {
            player.Unfreeze();
            gameObject.SetActive(false); 
        }
    }
}
