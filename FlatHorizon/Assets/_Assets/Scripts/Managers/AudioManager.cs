using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    public AudioSource bgmSource;
    public AudioSource sfxSource;

    [Header("Clips")]
    public AudioClip deathSound;

    void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // Opcional, si quieres que persista entre escenas
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayDeathSound()
    {
        bgmSource.Stop();
        sfxSource.PlayOneShot(deathSound);
    }

    public void PlayBGM()
    {
        if (!bgmSource.isPlaying)
        {
            bgmSource.Play();
        }
    }
}
