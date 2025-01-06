using UnityEngine;

public class DuplicatingBlock : Block
{
    [SerializeField] private float raycastLength;

    private Collider2D[] _blockColliders;
    private Collider2D[] _ignoringBlockColliders;

    private Transform _newBlockTransform;

    protected override void Awake()
    {
        base.Awake();
        _blockColliders = GetComponents<Collider2D>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        BlockTicksProcessor.Instance.OnTickProcessed += DisableIgnoreCollision;
    }

    protected override void Action()
    {
        DuplicateBlock();
    }

    private void DuplicateBlock()
    {
        if (CalculateEmptyBlocksCount() > 0)
        {
            int layerMask = LayerMask.GetMask("BlockLayer");

            RaycastHit2D[] hitsInfo = Physics2D.RaycastAll(ThisObjectTransform.position, -ThisObjectTransform.right, raycastLength, layerMask);

            foreach (RaycastHit2D hitInfo in hitsInfo)
            {
                if (hitInfo.collider == null) continue;
                if (hitInfo.collider.gameObject == gameObject) continue;

                if (hitInfo.collider.TryGetComponent(out Block block))
                {
                    GameObject newBlock = Instantiate(block.gameObject, ThisObjectTransform.position + Vector3.forward, Quaternion.identity);
                    _newBlockTransform = newBlock.transform;

                    Block newBlockComponent = newBlock.GetComponent<Block>();
                    newBlockComponent.Move(ThisObjectTransform.right);
                    newBlockComponent.IsDuplicated = true;

                    _ignoringBlockColliders = newBlock.GetComponents<Collider2D>();

                    foreach (Collider2D collider in _ignoringBlockColliders)
                    {
                        foreach (Collider2D blockCollider in _blockColliders)
                        {
                            Physics2D.IgnoreCollision(blockCollider, collider, true);
                        }
                    }
                }
            }
        }
    }

    private void DisableIgnoreCollision()
    {
        if (_ignoringBlockColliders != null && _ignoringBlockColliders.Length != 0)
        {
            _newBlockTransform.position -= Vector3.forward;

            foreach (Collider2D collider in _ignoringBlockColliders)
            {
                foreach (Collider2D blockCollider in _blockColliders)
                {
                    Physics2D.IgnoreCollision(blockCollider, collider, false);
                }
            }

            _ignoringBlockColliders = new Collider2D[0];
        }
    }

    private int CalculateEmptyBlocksCount()
    {

        if (gameObject.transform != null)
        {
            int lenght = 20;

            int blockLayerMask = LayerMask.GetMask("BlockLayer");
            RaycastHit2D[] blocksHitsInfo = Physics2D.RaycastAll(gameObject.transform.position, gameObject.transform.right, lenght, blockLayerMask);
            int blocksCount = 0;

            foreach (var block in blocksHitsInfo)
            {
                if (block.collider.gameObject != gameObject && block.collider.TryGetComponent<Block>(out Block collidedBlock))
                {
                    blocksCount++;
                }
            }

            Debug.Log("Blocks Count - " +  blocksCount);

            int walllayerMask = LayerMask.GetMask("WallLayer");
            RaycastHit2D[] wallsHitsInfo = Physics2D.RaycastAll(ThisObjectTransform.position, ThisObjectTransform.right, lenght, walllayerMask);


            int result = (int)Vector3.Distance(transform.position, wallsHitsInfo[0].transform.position) - 1 - blocksCount;
            Debug.Log(result);
            return (int)Vector3.Distance(transform.position, wallsHitsInfo[0].transform.position) - 1 - blocksCount;  
        }

        return 0;
    }
}
