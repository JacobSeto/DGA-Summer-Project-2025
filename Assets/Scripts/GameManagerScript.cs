using System;
using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

/*
 * The Game Manager handles win and loss conditions alongside tracking the time elapsed in each level.
 * Variables:
 * Loss: boolean that tracks if the player has lost the game by losing momentum
 * Win: boolean that tracks if the player has beaten the level.
 */
public class GameManagerScript
{
    private bool loss = false;
    private bool win = false;
    //placeholders for testing
    private int ZookeeperCount = 0;
    private float Velocity = 0.0f;
    private bool isLaunched = true;
    private float timer = 0.0f;

    void Update()
    {
        isFrozen();
        if (ZookeeperCount == 0)
        {
            Debug.Log("No More Keepers");
            win = true;
            Debug.Log(win);
        }
        timer += Time.deltaTime;

    }
    //Tracks if the player has lost momentum as of now
    private bool isFrozen()
    {
        if (Velocity == 0 & isLaunched == true)
        {
            Debug.Log("loss");
            loss = true;

            return true;
        }
        Debug.Log("not loss");
        return false;
    }
    //Returns if the player has won.
    public bool isWin()
    {
        if (win == true)
        {
            return true;
        }
        return false;
    }
   
    //Returns if the player has lost.
    public bool isLose()
    {
        if (loss == true)
        {
            return true;
        }
        return false;
    }
    //Returns current timer length
    public float getTime()
    {
        return timer;
    }
}


