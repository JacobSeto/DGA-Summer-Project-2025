using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/*
 * The Game Manager handles win and loss conditions alongside tracking the time elapsed in each level.
 * Variables:
 * Loss: boolean that tracks if the player has lost the game by losing momentum.
 * Win: boolean that tracks if the player has beaten the level.
 * Timer: float showing time elapsed in a level.
 */
public class GameManagerScript : MonoBehaviour
{
    public static GameManagerScript Instance;
    [HideInInspector] public PlayerController player;
    private Vector3 OriginalPos;
    private bool loss = false;
    private bool win = false;
    private bool pause = false;
    //placeholders for testing
    private int zookeeperCount = 0;
    private int originalCount = 0;
    private int originalStamina = 0;
    private float timer = 0.0f;

    public bool isPopupOpen => uiPopupScreen.activeSelf;


    [Header("Game Menu")]
    [SerializeField] MenuNavigation menuNavigation;

    [SerializeField] GameObject uiPopupScreen;

    [SerializeField] GameObject gameMenu;
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject winScreen;
    [SerializeField] GameObject loseScreen;
    [SerializeField] Image[] staminaBar;

    public Material greyscaleMat;

    void Awake()
    {
        Instance = this;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    private GameObject[] zooKeepers;

    void Start()
    {
        zooKeepers = GameObject.FindGameObjectsWithTag("Zookeeper");
        zookeeperCount = zooKeepers.Length;
        originalCount = zooKeepers.Length;
        OriginalPos = player.transform.position;
        originalStamina = player.GetStaminaCount();
        Time.timeScale = 1.0f;
    }

    void Update()
    {
        if (pause == false)
        {
            if (player.slowMotion)
            {
                timer += Time.deltaTime / player.slowDownAmount;
            }
            else
            {
                timer += Time.deltaTime;
            }


        }
        if (Input.GetKeyDown(KeyCode.Escape) && !win && !loss)
        {
            Pause();
        }
        if (Input.GetKeyDown(KeyCode.R) && !win && !loss)
        {
            Reset();
        }
    }

    public void HideGameMenu()
    {
        menuNavigation.ChangeActiveScreen(uiPopupScreen);
    }

    public void DonePopup()
    {
        menuNavigation.ChangeActiveScreen(gameMenu);
    }

    /// <summary>
    /// Sets up win condition
    /// </summary>
    public void WinGame()
    {
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
    /// Resets the game, setting all states back to original positions.
    /// </summary>
    public void Reset()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }

    /// <summary>
    /// Returns current timer length
    /// </summary>
    public float GetTime()
    {
        return timer;
    }

    /// <summary>
    /// Number of active zookeepers
    /// </summary>
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

    public GameObject getGameScreen()
    {
        return gameMenu;
    }
    
    public void UpdateStaminaBar(int stamina)
    {
        for (int i = 0; i < stamina; i++)
        {
            staminaBar[i].material = Canvas.GetDefaultCanvasMaterial();
        }

        for (int i = stamina; i < player.GetMaxStamina(); i++)
        {
            staminaBar[i].material = greyscaleMat;
        }
    }
}
