using UnityEngine;
using DG.Tweening;

public class BlocksMover : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;

    private Block _movingBlock;
    private Transform _movingBlockTransform;
    private Vector2 _mainBlockPosition;
    private bool _isMovingBlock;
    private bool _canMove = true;
    private Vector3 WorldTouchPos
    {
        get => (Vector2)mainCamera.ScreenToWorldPoint(Input.mousePosition);
    }

    private void Start()
    {
        BlockTicksProcessor.Instance.OnTicksStart += () => _canMove = false;
        BlockTicksProcessor.Instance.OnBlocksReset += () => _canMove = true;
    }

    private void OnDisable()
    {
        BlockTicksProcessor.Instance.OnTicksStart -= () => _canMove = false;
        BlockTicksProcessor.Instance.OnBlocksReset -= () => _canMove = true;

    }

    private void Update()
    {
        if (_isMovingBlock)
        {
            _movingBlockTransform.position = WorldTouchPos - Vector3.forward;

            if (Input.GetMouseButtonUp(0))
            {
                if (CheckBlockClick(out _) || !CheckBlockPlacement())
                {
                    _movingBlockTransform.position = _mainBlockPosition;
                }
                else
                {
                    _movingBlockTransform.DOKill();
                    AdjustBlockPosition(_movingBlockTransform);
                    _movingBlock.UpdateMainPosition();
                }

                _movingBlock.gameObject.layer = LayerMask.NameToLayer("BlockLayer");

                _movingBlockTransform = null;
                _movingBlock = null;
                _isMovingBlock = false;
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0) && _canMove)
            {
                if (CheckBlockClick(out _movingBlock))
                {
                    _movingBlock.gameObject.layer = LayerMask.NameToLayer("Default");

                    _movingBlockTransform = _movingBlock.transform;
                    _mainBlockPosition = _movingBlockTransform.position;
                    _isMovingBlock = true;
                }
            }
        }
    }

    private bool CheckBlockPlacement()
    {
        int layerMask = LayerMask.GetMask("ValidPlacement");

        Collider2D blockCollider = Physics2D.OverlapPoint(WorldTouchPos, layerMask);
        if (blockCollider != null)
            Debug.Log($"{blockCollider.name}");
        return (blockCollider != null);

    }

    private void AdjustBlockPosition(Transform blockTransfrom)
    {
        Vector2 adjustedPosition = blockTransfrom.position;

        adjustedPosition.x = Mathf.RoundToInt(adjustedPosition.x);
        adjustedPosition.y = Mathf.RoundToInt(adjustedPosition.y);

        blockTransfrom.position = adjustedPosition;
    }

    private bool CheckBlockClick(out Block hitBlock)
    {
        int layerMask = LayerMask.GetMask("BlockLayer");

        Collider2D blockCollider = Physics2D.OverlapPoint(WorldTouchPos, layerMask);
        if (blockCollider == null)
        {
            hitBlock = null;
            return false;
        }
        Debug.Log($"{blockCollider.name}");

        hitBlock = blockCollider.GetComponent<Block>();
        if (hitBlock.CanDrag == false)
        {
            Debug.Log($"{blockCollider.name} CANT MOVE");
            hitBlock = null;
            return false;
        }
        return true;
    }
}
