using System;
using UnityEngine;

public class VolumeSetter : MonoBehaviour
{
    [SerializeField] private bool isInGame;

    [SerializeField] private CustomToggle soundToggle;
    [SerializeField] private CustomToggle musicToggle;

    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource soundSource;

    private void Start()
    {
        bool musicEnabled = PlayerPrefs.GetInt("MusicEnabled", 1) == 1;
        bool soundEnabled = PlayerPrefs.GetInt("SoundEnabled", 1) == 1;

        if (!isInGame)
        {
            if (!musicEnabled) musicToggle.Switch();
            if (!soundEnabled) soundToggle.Switch();
        }
        
        musicSource.mute = !musicEnabled;
        if (soundSource != null) soundSource.mute = !soundEnabled;
    }

    public void UpdateMusic(bool value)
    {
        musicSource.mute = !value;
        PlayerPrefs.SetInt("MusicEnabled", Convert.ToInt32(value));
    }

    public void UpdateSound(bool value)
    {
        PlayerPrefs.SetInt("SoundEnabled", Convert.ToInt32(value));
    }
}
