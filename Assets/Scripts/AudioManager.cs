using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Sound Effects")]
    [SerializeField] private AudioSource bounceAudio;
    [SerializeField] private AudioSource releaseAudio;
    [SerializeField] private AudioSource pullAudio;
    [SerializeField] private AudioSource maxPullAudio;
    [SerializeField] private AudioSource fillArrowAudio;
    [SerializeField] private AudioSource BugAudio;
    [SerializeField] private AudioSource CheetahAudio;
    [SerializeField] private AudioSource ElephantAudio;
    [SerializeField] private AudioSource MonkeyAudio;
    [SerializeField] private AudioSource ZookeeperAudio;

    [SerializeField] private AudioSource MainMenu;

    [SerializeField] private AudioSource GrasslandsMusic;

    [SerializeField] private AudioSource TropicMusic;

    [SerializeField] private AudioSource JungleMusic;

    [SerializeField] private AudioSource IceMusic;

    private AudioSource[] songs;

    private MusicType currentWorld;

    public static AudioManager Instance;
    
    private bool ignoreNextMusicChange = false;

    public void SetIgnoreNextMusicChange()
    {
        ignoreNextMusicChange = true;
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this.gameObject);

        songs = new AudioSource[] { GrasslandsMusic, IceMusic, TropicMusic, JungleMusic, MainMenu };
        foreach (AudioSource song in songs)
        {
            if (song != null)
            {
                song.loop = true;
            }
        }
        Debug.Log("Audio Manager Set Up");
        currentWorld = MusicType.MainMenu;
        PlayMainMenu();
    }

    public void PlayMainMenu()
    {
        songs[4].Play();
    }

    public void PlayMusic(MusicType world)
    {
        Debug.Log($"PlayMusic called: {world}, currentWorld: {currentWorld}");
        Debug.Log($"Stack trace: {System.Environment.StackTrace}");
        if (ignoreNextMusicChange)
        {
            ignoreNextMusicChange = false;
            Debug.Log("Ignoring music change due to reset");
            return;
        }
        if (world != currentWorld && !songs[(int)world].isPlaying)
        {
            StopAllMusic();
            if (songs[(int)world] != null)
            {
                songs[(int)world].Play();
                currentWorld = world;
            }
        }
    }

    public void StopAllMusic()
    {
        foreach (AudioSource song in songs)
        {
            if (song != null && song.isPlaying)
            {
                song.Stop();
            }
        }
    }

    public void PlayBounce()
    {
        // Play audio on bounce, randomized semitones
        bounceAudio.pitch = 1;
        int[] semitones = new[] { 0, 2, 4, 7, 9 };
        int x = Random.Range(0, 5);
        for (int i = 0; i < x; i++)
        {
            bounceAudio.pitch *= 1.059463f;
        }
        bounceAudio.PlayOneShot(bounceAudio.clip);
    }

    public void PlayRelease()
    {
        releaseAudio.pitch = 0.5f;
        int[] semitones = new[] { 0, 2, 4, 7, 9 };
        int x = Random.Range(0, 5);
        for (int i = 0; i < x; i++)
        {
            bounceAudio.pitch *= 1.059463f;
        }
        releaseAudio.PlayOneShot(releaseAudio.clip);
    }

    public void PlayPull()
    {
        pullAudio.pitch = 1;
        int[] semitones = new[] { 0, 2, 4, 7, 9 };
        int x = Random.Range(0, 5);
        for (int i = 0; i < x; i++)
        {
            bounceAudio.pitch *= 1.059463f;
        }
        pullAudio.PlayOneShot(pullAudio.clip);
    }

    public void StopPull()
    {
        pullAudio.Stop();
    }

    //max stretch audio
    public void PlayMaxPull()
    {
        if (!maxPullAudio.isPlaying)
        {
            maxPullAudio.pitch = 1;
            maxPullAudio.PlayOneShot(maxPullAudio.clip);
        }


    }
    public void StopMaxPull()
    {
        maxPullAudio.Stop();
    }

    public void PlayFillArrow()
    {
        fillArrowAudio.PlayOneShot(fillArrowAudio.clip);
    }
    //topped a bar audio

    public void PlayBug()
    {
        BugAudio.PlayOneShot(BugAudio.clip);
    }
    public void PlayCheetah()
    {
        CheetahAudio.PlayOneShot(CheetahAudio.clip);
    }
    public void PlayElephant()
    {
        ElephantAudio.PlayOneShot(ElephantAudio.clip);
    }
    public void PlayMonkey()
    {
        MonkeyAudio.PlayOneShot(MonkeyAudio.clip);
    }
    public void PlayZookeeper()
    {
        ZookeeperAudio.PlayOneShot(ZookeeperAudio.clip);
    }
}
