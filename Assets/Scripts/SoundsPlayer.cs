using UnityEngine;

public class SoundsPlayer : MonoBehaviour
{
    public static SoundsPlayer Instance;

    [SerializeField] private AudioSource soundSource;

    [SerializeField] private AudioClip enemyKillSound;
    [SerializeField] private AudioClip winSound;
    [SerializeField] private AudioClip tickSound;

    private void Awake() => Instance = this;

    public void PlayKillSound()
    {
        soundSource.PlayOneShot(enemyKillSound);
    }

    public void PlayWinSound()
    {
        soundSource.PlayOneShot(winSound);
    }

    public void PlayTickSound()
    {
        soundSource.PlayOneShot(tickSound);
    }
}


