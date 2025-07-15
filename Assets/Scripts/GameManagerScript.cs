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
    private GameObject player;
    private bool loss = false;
    private bool win = false;
    private bool pause = false;
    //placeholders for testing
    private int zookeeperCount = 0;
    private float timer = 0.0f;

    void Awake() => Instance = this;

    private GameObject[] zooKeepers;
    private bool playerFreeze;

    void Start()
    {
        zooKeepers = GameObject.FindGameObjectsWithTag("Zookeeper");
        zookeeperCount = zooKeepers.Length;
        player = GameObject.FindGameObjectWithTag("Player");
        playerFreeze = player.GetComponent<PlayerController>().enabled;
    }

    void Update()
    {
        if (pause == false)
        {
            timer += Time.deltaTime;
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Pause");
            Pause();
        }
        if (player.GetComponent<PlayerController>().lose)
        {
            LoseGame();
        }
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
    /// If pause is false, Pauses the game, freezing everything and opening up a menu. Does opposite if pause is true.
    /// </summary>
    public void Pause()
    {
        if (!pause) {
            pause = true;
            playerFreeze = false;
            Time.timeScale = 0;
            //pull up pause menu
            }
        else
        {
            pause = false;
            playerFreeze = true;
            Time.timeScale = 1;
            //close pause menu
        }
    }

    /// <summary>
    /// Returns current timer length
    /// </summary>
    public float GetTime()
    {
        return timer;
    }

    /// <summary>
    /// Brings down zookeeper count.
    /// </summary>
    public void decrementZookeeper()
    {
        zookeeperCount -= 1;
        if (zookeeperCount == 0)
        {
            WinGame();
        }
    }
}
