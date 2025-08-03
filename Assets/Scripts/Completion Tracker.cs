using System;
using TMPro;
using TMPro.EditorUtilities;
using UnityEngine;

public class CompletionTracker : MonoBehaviour
{
    int completionCount;
    [SerializeField] LevelScreen[] levels;
    float totalTime;
    int levelCount;
    [SerializeField] TMP_Text completionText;

    private void Start()
    {
        levelCount = levels.Length;
        foreach(var level in levels)
        {
            level.UpdateScreen(this);
        }
        SetCompletionDisplay();
    }

    public void UpdateTracker(bool completed, float time)
    {
        completionCount += completed ? 1 : 0;
        totalTime += time;
    }

    void SetCompletionDisplay()
    {
        if(completionCount == levelCount)
        {
            TimeSpan time = TimeSpan.FromSeconds(totalTime);
            completionText.text = "Final Time\n" + time.ToString("mm\\:ss\\.ff");
        }
        else
        {
            completionText.text = "Exhibits\n" + completionCount + "/" + levelCount;
        }
    }
}
