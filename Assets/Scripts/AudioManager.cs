using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    [Header("Sound Effects")]
    [SerializeField] private AudioSource bounceAudio;
    [SerializeField] private AudioSource releaseAudio;

    // speed audio
    [SerializeField] private AudioSource fastAudio;
    [SerializeField] private AudioSource slowerFastAudio;
    [SerializeField] private AudioSource mediumAudio;
    [SerializeField] private AudioSource slowAudio;
    [SerializeField] private AudioSource slowerSlowAudio;

    public static AudioManager Instance;
    private PlayerController player;

    private bool start = false;
    private float playerSpeed = 0;

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

    public void PlayPull() {
        //pullAudio.pitch = 0.5f;
        //pullAudio.PlayOneShot(pullAudio.clip);
    }

    public void PlayRelease() {
        releaseAudio.pitch = 0.5f;
        releaseAudio.PlayOneShot(releaseAudio.clip);
    }

    private IEnumerator Start() {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        yield return new WaitForSeconds(0.1f);
        start = true;
    }

    private void Update() {
        if (start) {
            if (!player.launched && !fastAudio.isPlaying) {
                fastAudio.PlayOneShot(fastAudio.clip);
            } else {
                playerSpeed = player.GetCurrentSpeed() / player.maxSpeed;
                if (!(slowerSlowAudio.isPlaying || slowAudio.isPlaying || mediumAudio.isPlaying
                    || slowerFastAudio.isPlaying || fastAudio.isPlaying)) {
                    if (playerSpeed < 0.1f) {
                        slowerSlowAudio.PlayOneShot(slowerSlowAudio.clip);
                    } else if (playerSpeed < 0.3f) {
                        slowAudio.PlayOneShot(slowAudio.clip);
                    } else if (playerSpeed < 0.5f) {
                        mediumAudio.PlayOneShot(mediumAudio.clip);
                    } else if (playerSpeed < 0.75f) {
                        slowerFastAudio.PlayOneShot(slowerFastAudio.clip);
                    } else {
                        fastAudio.PlayOneShot(fastAudio.clip);
                    }
                }
            }
        }
    }
}
