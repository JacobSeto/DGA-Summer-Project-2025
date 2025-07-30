using System;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    private float timer;
    private float timerGap;
    private float minutes;
    private float seconds;
    private String secString;
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
            seconds = timer % 60;
            secString = seconds.ToString();
            if (secString.Length != 2)
            {
                secString = "0" + secString;
            }
            text.SetText(minutes.ToString() + ":" + secString);
        }
    }
}