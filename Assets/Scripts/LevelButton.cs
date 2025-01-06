using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelButton : MonoBehaviour
{
    [SerializeField] private GameObject levelIndexObject;
    [SerializeField] private GameObject lockObject;

    [SerializeField] private TextMeshProUGUI levelIndexText;
    [SerializeField] private int levelIndex;

    private bool _isUnlocked;

    private void Start()
    {
        if (PlayerPrefs.GetInt("Level" + levelIndex, 0) == 1 || levelIndex == 1)
        {
            levelIndexObject.SetActive(true);
            lockObject.SetActive(false);
            levelIndexText.text = levelIndex.ToString();

            _isUnlocked = true;
        }
    }

    public void Play()
    {
        if (!_isUnlocked) return;

        SceneSwitcher.Instance.SwitchToScene("Level" + levelIndex);
    }
}
