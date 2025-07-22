using System;
using System.Collections;
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
    [HideInInspector] public PlayerController player;
    private bool loss = false;
    private bool win = false;
    private bool pause = false;
    //placeholders for testing
    private int zookeeperCount = 0;
    private float timer = 0.0f;
    private bool isInAir = false;

    [Header("Game Menu")]
    [SerializeField] MenuNavigation menuNavigation;
    [SerializeField] GameObject gameMenu;
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject winScreen;
    [SerializeField] GameObject loseScreen;

    void Awake() {
        Instance = this;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    private GameObject[] zooKeepers;

    void Start()
    {
        zooKeepers = GameObject.FindGameObjectsWithTag("Zookeeper");
        zookeeperCount = zooKeepers.Length;
    }

    void Update()
    {
        if (pause == false)
        {
            if (player.slowMotion)
            {
                timer += Time.deltaTime/player.slowDownAmount;
            } else
            {
                timer += Time.deltaTime;
            }
            
            
        }
        if (Input.GetKeyDown(KeyCode.Escape) && !win && !loss)
        {
            Debug.Log("Pause");
            Pause();
        }
    }

    /// <summary>
    /// Sets up win condition
    /// </summary>
    public void WinGame()
    {
        Debug.Log("You Win");
        win = true;
        Pause();
        menuNavigation.ChangeActiveScreen(winScreen);
        //pull up menu
    }

    /// <summary>
    /// Sets up loss condition
    /// </summary>
    public void LoseGame()
    {
        Debug.Log("You Lose");
        loss = true;
        Pause();
        menuNavigation.ChangeActiveScreen(loseScreen);
    }

    /// <summary>
    /// If pause is false, Pauses the game, freezing everything and opening up a menu. Does opposite if pause is true.
    /// </summary>
    public void Pause()
    {
        pause = !pause;
        player.enabled = !pause;
        if (pause)
        {
            Time.timeScale = 0;
            menuNavigation.ChangeActiveScreen(pauseMenu);
            
        }
        else
        {
            Time.timeScale = 1;
            menuNavigation.ChangeActiveScreen(gameMenu);

        }
    }

    /// <summary>
    /// Returns current timer length
    /// </summary>
    public float GetTime()
    {
        return timer;
    }

    public void goInAir()
    {
        StartCoroutine(AirTime());
    }

    IEnumerator AirTime()
    {
        isInAir = true;
        yield return new WaitForSeconds(1);
        isInAir = false;
    }

    public bool inAir()
    {
        return isInAir;
    }

    public int numZookeepers()
    {
        return zookeeperCount;
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
