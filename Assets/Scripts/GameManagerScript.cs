using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

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
            if (!zooKeepers[i].activeSelf)
            {
                zooKeepers.Remove(zooKeepers[i]);
                ZookeeperCount--;
            }
        }
<<<<<<< Updated upstream
        if (ZookeeperCount == 0)
=======
        Debug.Log(zookeeperCount);
        if (zookeeperCount == 0)
>>>>>>> Stashed changes
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


