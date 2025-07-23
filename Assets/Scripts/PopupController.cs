using UnityEngine;
using UnityEngine.UI;

public class PopupController : MonoBehaviour
{
    [SerializeField] private GameObject text1;
    [SerializeField] private GameObject text2;
    [SerializeField] private Button nextButton;
    [SerializeField] private Button doneButton;

    void Start()
    {
        text1.SetActive(true);
        text2.SetActive(false);
        doneButton.gameObject.SetActive(false); 

        nextButton.onClick.AddListener(OnNextClicked);
        doneButton.onClick.AddListener(OnDoneClicked); 
    }

    void OnNextClicked()
    {
        text1.SetActive(false);
        text2.SetActive(true);

        nextButton.gameObject.SetActive(false);
        doneButton.gameObject.SetActive(true); 
    }

    void OnDoneClicked()
    {
        gameObject.SetActive(false);
        GameManagerScript.Instance.DonePopup();
    }
}
