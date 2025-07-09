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
    private float Velocity = 0;
    private bool isLaunched = true;

    void Update()
    {
        isFrozen();
        if (ZookeeperCount == 0)
        {
            Console.WriteLine("No More Keepers");
            win = true;
            Console.WriteLine(win);
        }
    }
    //Tracks if the player has lost momentum as of now
    private bool isFrozen()
    {
        if (Velocity == 0 & isLaunched == true)
        {
            Console.WriteLine("loss");
            loss = true;

            return true;
        }
        Console.WriteLine("not loss");
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
    //Resets key variables during level reloads or changes.
    public void managerReset()
    {
        loss = false;
        win = false;
    }
}


