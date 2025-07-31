using TMPro;
using UnityEngine;
/// <summary>
/// Scrolls between the different screens and loops beginning and end
/// </summary>
public class ScreenScrolling : MonoBehaviour
{
    [SerializeField] GameObject[] screens;
    [SerializeField] TMP_Text LEDText;
    [SerializeField] string[] LEDTexts;

    int index = 0;

    private void Start()
    {
        screens[index].gameObject.SetActive(true);
        LEDText.text = LEDTexts[index];
    }
    public void ScrollRight()
    {
        screens[index].gameObject.SetActive(false);
        index++;
        if (index >= screens.Length)
        {
            index = 0;
        }
        screens[index].gameObject.SetActive(true);
        LEDText.text = LEDTexts[index];
    }

    public void ScrollLeft()
    {
        screens[index].gameObject.SetActive(false);
        index--;
        if (index < 0)
        {
            index = screens.Length - 1;
        }
        screens[index].gameObject.SetActive(true);
        LEDText.text = LEDTexts[index];
    }
}
