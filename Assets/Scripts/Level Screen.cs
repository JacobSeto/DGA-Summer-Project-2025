using TMPro;
using UnityEngine;

/// <summary>
/// The screen that represents a level in the level select
/// </summary>
public class LevelScreen : MonoBehaviour
{
    [SerializeField] string levelName;
    [SerializeField] string sceneName;
    [SerializeField] TMP_Text levelNameText;
    [SerializeField] TMP_Text completionText;
    [SerializeField] Color incompleteColor;
    [SerializeField] Color completeColor;

    private void Start()
    {
        levelNameText.text = levelName;
        if(PlayerPrefs.GetFloat(sceneName, 0) != 0)
        {
            completionText.color = completeColor;
            completionText.text = "Time: " + PlayerPrefs.GetFloat(sceneName).ToString("mm\\:ss\\.ff");
        }
        else
        {
            completionText.color = incompleteColor;
            completionText.text = "Incomplete";
        }
    }

    public void LoadLevel()
    {
        MenuNavigation.Instance.LoadScene(sceneName);
    }

}
