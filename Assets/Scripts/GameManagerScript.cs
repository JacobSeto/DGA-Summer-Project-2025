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
 * Loss: boolean that tracks if the player has lost the game by losing momentum.
 * Win: boolean that tracks if the player has beaten the level.
 * Timer: float showing time elapsed in a level.
 */
public class GameManagerScript: MonoBehaviour
{
    public static GameManagerScript Instance;
    private bool loss = false;
    private bool win = false;
    //placeholders for testing
    private int zookeeperCount = 0;
    private float Velocity = 0.0f;
    private bool isLaunched = true;
    private float timer = 0.0f;

    void Awake() => Instance = this;

    private List<GameObject> zooKeepers;

    void Start()
    {
        zooKeepers = GameObject.FindGameObjectsWithTag("Zookeeper").ToList();
        zookeeperCount = zooKeepers.Count;
    }

    void Update()
    {
        isFrozen();
        for (int i = 0; i < zooKeepers.Count; i++)
        {
            if (!zooKeepers[i].activeSelf)
            {
                zooKeepers.Remove(zooKeepers[i]);
                zookeeperCount--;
            }
            Debug.Log(zookeeperCount);
        }
        if (zookeeperCount == 0)
        {
            Debug.Log("win");
            win = true;
        }
        timer += Time.deltaTime;

    }
    //Tracks if the player has lost momentum as of now
    private bool isFrozen()
    {
        if (Velocity == 0 & isLaunched == true)
        {
            loss = true;
            return true;
        }
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
