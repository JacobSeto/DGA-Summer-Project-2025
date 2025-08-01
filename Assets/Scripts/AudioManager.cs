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


    public static AudioManager Instance;

    private void Awake() {
        Instance = this;
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

    public void PlayRelease() {
        releaseAudio.pitch = 0.5f;
        releaseAudio.PlayOneShot(releaseAudio.clip);
    }

    public void PlayPull()
    {
        pullAudio.pitch = 1;
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
