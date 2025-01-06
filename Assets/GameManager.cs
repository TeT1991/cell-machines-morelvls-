using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private SceneSwitcher sceneSwitcher;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] public UnityEvent[] OnAllEnemiesDefeated;
    [SerializeField] private int currentLevelIndex;
   
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void CheckEnemies()
    {
        Debug.Log("check enemies");
        Collider2D[] enemies = Physics2D.OverlapCircleAll(Vector2.zero, Mathf.Infinity, enemyLayer);

        if (enemies.Length == 0)
        {
            InvokeAllEnemiesDefeatedEvents();
        }
    }

    private void InvokeAllEnemiesDefeatedEvents()
    {
        if (OnAllEnemiesDefeated == null) return;

        foreach (var eventAction in OnAllEnemiesDefeated)
        {
            eventAction.Invoke();
        }

        PlayerPrefs.SetInt("Level" + (currentLevelIndex + 1), 1);
        SoundsPlayer.Instance.PlayWinSound();
        BlockTicksProcessor.Instance.StopTicks();
    }

    public void Pause()
    {
        Time.timeScale = 0f;
    }

    public void Unpause()
    {
        Time.timeScale = 1f;
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        DOTween.KillAll();
        sceneSwitcher.SwitchToScene("Level" + currentLevelIndex);
    }

    public void Next()
    {
        Time.timeScale = 1f;
        DOTween.KillAll();
        sceneSwitcher.SwitchToScene("Level" + (currentLevelIndex + 1));
    }

    public void Exit()
    {
        Time.timeScale = 1f;
        DOTween.KillAll();
        sceneSwitcher.SwitchToScene("Menu");
    }
}
