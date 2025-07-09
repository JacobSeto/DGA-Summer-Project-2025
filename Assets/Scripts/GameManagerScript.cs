using System;
using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

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
    public bool isWin()
    {
        if (win == true)
        {
            return true;
        }
        return false;
    }
    public bool isLose()
    {
        if (loss == true)
        {
            return true;
        }
        return false;
    }
}


