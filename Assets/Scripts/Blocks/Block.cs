using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Rigidbody2D))]
public class Block : MonoBehaviour
{
    public bool IsDuplicated { private get; set; }

    protected Rigidbody2D BlockRb { get; private set; }

    protected Transform ThisObjectTransform { get; private set; }

    private Vector3 _firstRotation;
    private Vector2 _firstPosition;
    private bool _canAdjust = true;
    protected float _tickDuration;

    protected virtual void Awake()
    {
        BlockRb = GetComponent<Rigidbody2D>();

        ThisObjectTransform = transform;
        UpdateMainPosition();
    }

    public void UpdateMainPosition()
    {
        _firstPosition = ThisObjectTransform.position;
        _firstRotation = ThisObjectTransform.eulerAngles;

        BlockRb.position = _firstPosition;
    }

    protected virtual void OnEnable()
    {
        _tickDuration = BlockTicksProcessor.Instance.TickDuration;

        BlockTicksProcessor.Instance.OnTick += Action;
        BlockTicksProcessor.Instance.OnTickProcessed += AdjustPosition;
        BlockTicksProcessor.Instance.OnBlocksReset += ResetPosition;
    }

    protected virtual void Action() { }

    private void ResetPosition()
    {
        if (IsDuplicated)
        {
            Destroy(gameObject);
            return;
        }

        ThisObjectTransform.DOKill();
        _canAdjust = false;
        ThisObjectTransform.DOMove(_firstPosition, _tickDuration / 2f);
        ThisObjectTransform.DORotate(_firstRotation, _tickDuration / 2f);
    }

    public virtual void Rotate()
    {
        ThisObjectTransform.DORotate(ThisObjectTransform.eulerAngles - (Vector3.forward * 90f), _tickDuration / 2f);
    }

    public void AdjustPosition()
    {
        if (_canAdjust)
        {
            Vector2 adjustedPos = VectorUtility.RoundVector(ThisObjectTransform.position);
            ThisObjectTransform.position = adjustedPos;
        }
        _canAdjust = true;
    }

    private void OnDisable()
    {
        BlockTicksProcessor.Instance.OnTickProcessed -= AdjustPosition;
        BlockTicksProcessor.Instance.OnTickProcessed -= AdjustPosition;
        BlockTicksProcessor.Instance.OnBlocksReset -= ResetPosition;
    }

    public void Move(Vector2 direction)
    {
        Vector2 nextPos = VectorUtility.RoundVector((Vector2)ThisObjectTransform.position + direction);
        BlockRb.DOMove(nextPos, _tickDuration);

        Collider2D enemy = GetCollidingEnemy(nextPos);
        if (enemy != null)
        {
            ParticlesSpawner.Instance.SpawnParticles(enemy.transform.position);
            SoundsPlayer.Instance.PlayKillSound();
            Destroy(enemy.gameObject);
            Destroy(gameObject);
            GameManager.Instance.CheckEnemies();
            return;
        }
    }

    private Collider2D GetCollidingEnemy(Vector2 position)
    {
        return Physics2D.OverlapCircle(position, 0.1f, LayerMask.GetMask("EnemyLayer"));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Finish")
        {
            ParticlesSpawner.Instance.SpawnParticles(collision.transform.position);
            SoundsPlayer.Instance.PlayKillSound();
            Destroy(collision.gameObject);
            Destroy(gameObject);

            GameManager.Instance.CheckEnemies();
        }
    }
}
