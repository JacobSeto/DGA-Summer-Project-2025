using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
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
    private List<GameObject> zooKeepers;

    void Start()
    {
        zooKeepers = GameObject.FindGameObjectsWithTag("Zookeeper").ToList();
        ZookeeperCount = zooKeepers.Count;
    }

    void Update()
    {
        isFrozen();
        for (int i = 0; i < zooKeepers.Count; i++)
        {
            if (zooKeepers[i].IsDestroyed())
            {
                zooKeepers.Remove(zooKeepers[i]);
                ZookeeperCount--;
            }
        }
        if (ZookeeperCount == 0)
        {
            Debug.Log("No More Keepers");
            win = true;
            Debug.Log(win);
        }
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
    //Resets key variables during level reloads or changes.
    public void managerReset()
    {
        loss = false;
        win = false;
    }
}


