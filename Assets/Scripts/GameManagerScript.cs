using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

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
        for (int i = 0; i < zooKeepers.Count; i++)
        {
            if (!zooKeepers[i].activeSelf)
            {
                zooKeepers.Remove(zooKeepers[i]);
                zookeeperCount--;
            }
        }
        if (zookeeperCount == 0)
        {
            WinGame();
        }
        timer += Time.deltaTime;

    }
    
    /// <summary>
    /// Sets up win condition
    /// </summary>
    public void WinGame()
    {
        Debug.Log("You Win");
        Time.timeScale = 0;
        win = true;
        //pull up menu
    }

    /// <summary>
    /// Sets up loss condition
    /// </summary>
    public void LoseGame()
    {
        Debug.Log("You Lose");
        Time.timeScale = 0;
        loss = true;
        //pull up loss menu
    }
    /// <summary>
    /// Returns current timer length
    /// </summary>
    /// <returns></returns>
    public float GetTime()
    {
        return timer;
    }
}
