using System.Collections;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    public static MusicPlayer Instance { get; private set; }

    [Header("Audio Settings")]
    [SerializeField] private AudioSource audioSource; 
    [SerializeField] private AudioClip[] musicTracks; 
    [SerializeField] public float musicVolume = 1.0f;

    private int currentTrackIndex = 0;
    private void Awake()
    {
        musicVolume = PlayerPrefs.GetFloat("MusicVolume");
        if (PlayerPrefs.GetFloat("FirstStart") == 0)
        {
            musicVolume = 1f;
            PlayerPrefs.SetFloat("FirstStart", 1);
        }
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        audioSource.loop = false;
        audioSource.volume = musicVolume;
    }

    private void Start()
    {
        PlayTrack(Random.Range((int)0,musicTracks.Length));
    }

    private void Update()
    {
        if (!audioSource.isPlaying)
            PlayNextTrack();
    }

    public static void SetVolume(float volume)
    {
        if (Instance != null && Instance.audioSource != null)
        {
            Instance.musicVolume = Mathf.Clamp01(volume); 
            Instance.audioSource.volume = Instance.musicVolume;
            PlayerPrefs.SetFloat("MusicVolume", volume);
        }
    }

    public static void LowerVolume(float decrement)
    {
        if (Instance != null)
        {
            SetVolume(Instance.musicVolume - decrement);
        }
    }

    public static void ToggleMusic()
    {
        if (Instance != null)
        {
            if (Instance.musicVolume == 0f)
                Instance.musicVolume = 1f;
            else Instance.musicVolume = 0f;
            SetVolume(Instance.musicVolume);
        }
    }


    public void PlayNextTrack()
    {
        if (musicTracks.Length == 0) return;

        currentTrackIndex = (currentTrackIndex + 1) % musicTracks.Length;
        PlayTrack(currentTrackIndex);
    }

    public void PlayTrack(int trackIndex)
    {
        if (trackIndex < 0 || trackIndex >= musicTracks.Length) return;

        currentTrackIndex = trackIndex;
        audioSource.clip = musicTracks[trackIndex];
        audioSource.Play();
    }
}


