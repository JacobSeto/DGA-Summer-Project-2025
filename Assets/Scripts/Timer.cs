using System;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    private float timer;
    private float timerGap;
    private float minutes;
    private float seconds;
    private String timeString;
    private TMP_Text text;
    private PlayerController player;
    private bool firstLaunch = true;
    void Start()
    {
        text = gameObject.transform.GetChild(0).GetComponent<TMP_Text>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    void Update()
    {
        if (player.launched && firstLaunch)
        {
            timerGap = (float)Math.Round(GameManagerScript.Instance.timer, 0);
            firstLaunch = false;
        }
        else if (player.launched && !firstLaunch)
        {
            timer = (float)Math.Round(GameManagerScript.Instance.timer, 0) - timerGap;
            if (timer / 60 >= 1)
            {
                minutes = timer / 60;
            }
            timeString = minutes.ToString();
            seconds = timer % 60;
            if (seconds.ToString().Length != 2)
            {
                timeString = timeString + ":" + "0" + seconds.ToString();
            }
            else
            {
                timeString = timeString + ":" + seconds.ToString();
            }
            text.SetText(timeString);
        }
    }

   public String GetFinalTime()
    {
        return timeString;
    }
}