using System.Collections;
using UnityEngine;
using DG.Tweening;

public class BlockTicksProcessor : MonoBehaviour
{
    public static BlockTicksProcessor Instance;

    public delegate void TickProcessed();
    public delegate void TicksStart();
    public TicksStart OnTicksStart;    
    public delegate void TicksStop();
    public TicksStop OnTicksStop;
    public TickProcessed OnTickProcessed;

    public delegate void Tick();
    public Tick OnTick;

    public delegate void BlocksReset();
    public BlocksReset OnBlocksReset;

    private Coroutine _blockTicksCoroutine;

    public float TickDuration { get; private set; }

    private bool _isTicking;

    private void Awake()
    {
        Physics2D.simulationMode = SimulationMode2D.Script;
        Instance = this;
        TickDuration = PlayerPrefs.GetFloat("TickDuration", 0.5f);
    }

    public void StartTicks()
    {
        Physics2D.simulationMode = SimulationMode2D.FixedUpdate;
        _blockTicksCoroutine = StartCoroutine(BlockTicksCoroutine());
        OnTicksStart?.Invoke();
    }



    public void StopTicks()
    {
        if (_blockTicksCoroutine != null) StopCoroutine(_blockTicksCoroutine);
        OnTicksStop?.Invoke();
    }

    public void ToggleTicks()
    {
        _isTicking = !_isTicking;

        if (!_isTicking) StopTicks();
        else StartTicks();
    }

    public void PlayOneTick() => ProcessTick();

    private IEnumerator BlockTicksCoroutine()
    {
        while (true)
        {
            ProcessTick();
            yield return new WaitForSeconds(TickDuration);
        }
    }

    private void ProcessTick()
    {
        if (!_isTicking) Physics2D.simulationMode = SimulationMode2D.FixedUpdate;
        SoundsPlayer.Instance.PlayTickSound();
        OnTick?.Invoke();
        Invoke(nameof(InvokeOnTickProcessed), TickDuration);
    }

    private void InvokeOnTickProcessed()
    {
        OnTickProcessed?.Invoke();
        if (!_isTicking) Physics2D.simulationMode = SimulationMode2D.Script;
    }

    public void ResetBlocks()
    {
        StopTicks();
        _isTicking = false;
        DOTween.KillAll();
        Physics2D.simulationMode = SimulationMode2D.Script;
        OnBlocksReset?.Invoke();
    }
}
