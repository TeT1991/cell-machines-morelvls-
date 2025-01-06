using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleScript : MonoBehaviour
{
    [SerializeField] private Sprite on;
    [SerializeField] private Sprite off;
    private Image img;

    private void Start()
    {
        img = GetComponent<Image>();
        Check();
    }

    public void Check()
    {
        if(MusicPlayer.Instance.musicVolume > 0)
            img.sprite = on;
        else img.sprite = off;
    }
}
