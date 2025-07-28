using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Sound Effects")]
    [SerializeField] private AudioSource bounceAudio;
    [SerializeField] private AudioSource releaseAudio;
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
}
