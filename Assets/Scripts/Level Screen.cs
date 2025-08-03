using System;
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

    public void UpdateScreen(CompletionTracker completionTracker)
    {
        levelNameText.text = levelName;
        if(PlayerPrefs.GetFloat(sceneName, 0) != 0)
        {
            completionText.color = completeColor;
            TimeSpan time = TimeSpan.FromSeconds(PlayerPrefs.GetFloat(sceneName));
            completionText.text = "Time: " + time.ToString("m\\:ss\\.ff");
            completionTracker.UpdateTracker(true, PlayerPrefs.GetFloat(sceneName));
        }
        else
        {
            completionText.color = incompleteColor;
            completionTracker.UpdateTracker(false, 0);
            completionText.text = "Incomplete";
        }
    }

    public void LoadLevel()
    {
        MenuNavigation.Instance.LoadScene(sceneName);
    }

}
