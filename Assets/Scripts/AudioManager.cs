using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Sound Effects")]
    [SerializeField] private AudioSource bounceAudio;
    [SerializeField] private AudioSource pullAudio;
    [SerializeField] private AudioSource stretchAudio;
    private static AudioManager instance;

    public static AudioManager Instance {

        get {
            if (instance==null) {
                Debug.LogError("Audio manager is null");
            }
            return instance;
        }
    }

    private void Awake() {
        instance = this;
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

    public void PlayPull() {
        pullAudio.pitch = 1;
        pullAudio.PlayOneShot(pullAudio.clip);
    }

    public void PlayStretch() {
        stretchAudio.pitch = 1;
        stretchAudio.Play();
    }
}
