using System.Collections.Generic;
using UnityEngine;

public class RotatingBlock : Block
{
    [SerializeField] private float raycastLength;

    private List<Block> _rotatedBlocks = new List<Block>();

    private static readonly Vector2[] _directions = new Vector2[4] { Vector2.down, Vector2.up, Vector2.left, Vector2.right };

    protected override void Action()
    {
        Invoke(nameof(RotateNearBlocks), _tickDuration / 2f);
    }

    private void RotateNearBlocks()
    {
        int layerMask = LayerMask.GetMask("BlockLayer");

        foreach (Vector2 direction in _directions)
        {
            RaycastHit2D[] hitsInfo = Physics2D.RaycastAll(ThisObjectTransform.position, direction, raycastLength, layerMask);

            foreach (RaycastHit2D hitInfo in hitsInfo)
            {
                if (hitInfo.collider == null) continue;
                if (hitInfo.collider.gameObject == gameObject) continue;

                if (hitInfo.collider.TryGetComponent(out Block block))
                {
                    Debug.Log(name + " / " + block.name);

                    block.Rotate();
                    _rotatedBlocks.Add(block);
                }
            }
        }
    }
}
