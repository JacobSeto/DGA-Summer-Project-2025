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
using TMPro;

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
    private bool pause = false;
    //placeholders for testing
    private int zookeeperCount = 0;
    private int originalCount = 0;
    private int originalStamina = 0;
    private String finalTime;
    private String sceneName;
    private float fastestTime;
    private bool tutorial;


    [Header("Game Menu")]
    [SerializeField] MenuNavigation menuNavigation;

    [SerializeField] GameObject gameMenu;
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject winScreen;
    [SerializeField] GameObject winText;
    [SerializeField] GameObject loseScreen;
    [SerializeField] Image[] staminaBar;
    [Header("World Settings")]
    [SerializeField] private MusicType currentWorld;
    public MusicType CurrentWorld => currentWorld;

    [Header("UI Settings")]
    public Material greyscaleMat;

    [Header("Game Time")]
    float gameTime = 0;
    [Tooltip("If the player is actively playing the game")]
    [HideInInspector] public bool inGame = false;
    bool gameEnded = false;
    [SerializeField] TMP_Text timerText;


    void Awake()
    {
        Instance = this;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    private GameObject[] zooKeepers;
    private Transform[] zooKeeperTransforms;

    void Start()
    {
        zooKeepers = GameObject.FindGameObjectsWithTag("Zookeeper");
        zooKeeperTransforms = new Transform[zooKeepers.Length];
        for (int i = 0; i < zooKeepers.Length; i++)
        {
            if (zooKeepers[i] != null)
            {
                zooKeeperTransforms[i] = zooKeepers[i].transform;
            }
        }
        zookeeperCount = zooKeepers.Length;
        originalCount = zooKeepers.Length;
        OriginalPos = player.transform.position;
        originalStamina = player.GetStaminaCount();
        Time.timeScale = 1.0f;

        AudioManager.Instance.PlayMusic(currentWorld);
    }

    void Update()
    {
        if (inGame)
        {
            gameTime += Time.unscaledDeltaTime;
            timerText.text = TimeSpan.FromSeconds(gameTime).ToString("m\\:ss\\.ff");
        }
        if (Input.GetKeyDown(KeyCode.Escape) && !gameEnded)
        {
            Pause();
        }
        if (Input.GetKeyDown(KeyCode.R) && !gameEnded)
        {
            Reset();
        }
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
        Pause();
        sceneName = SceneManager.GetActiveScene().name;
        if (PlayerPrefs.GetFloat(sceneName, 0) == 0f || gameTime < PlayerPrefs.GetFloat(sceneName))
        {
            PlayerPrefs.SetFloat(sceneName, gameTime);
        }
        winText.GetComponent<TMP_Text>().SetText("You win!\n Time: " + TimeSpan.FromSeconds(gameTime).ToString("m\\:ss\\.ff"));
        menuNavigation.ChangeActiveScreen(winScreen);
        gameEnded = true;
    }

    /// <summary>
    /// Sets up loss condition
    /// </summary>
    public void LoseGame()
    {
        Pause();
        menuNavigation.ChangeActiveScreen(loseScreen);
        gameEnded = true;
    }

    /// <summary>
    /// If pause is false, Pauses the game, freezing everything and opening up a menu. Does opposite if pause is true.
    /// </summary>
    public void Pause()
    {
        pause = !pause;
        // inGame true when not paused and player has launched
        inGame = !pause;
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
    /// Number of active zookeepers
    /// </summary>
    public int numZookeepers()
    {
        return zookeeperCount;
    }

    /// <summary>
    /// Brings down zookeeper count.
    /// </summary>
    public void decrementZookeeper(GameObject keeper)
    {
        zookeeperCount -= 1;
        List<GameObject> tempList = new List<GameObject>(zooKeepers);
        tempList.Remove(keeper);
        zooKeepers = tempList.ToArray();
        zooKeeperTransforms = new Transform[zooKeepers.Length];
        for (int i = 0; i < zooKeepers.Length; i++)
        {
            if (zooKeepers[i] != null)
            {
                zooKeeperTransforms[i] = zooKeepers[i].transform;
            }
        }
        if (zookeeperCount == 0)
        {
            WinGame();
        }
    }

    public Transform[] GetZookeeperList()
    {
        return zooKeeperTransforms;
    }

    public GameObject getGameScreen()
    {
        return gameMenu;
    }

    public void Tutorial()
    {
        tutorial = true;
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
