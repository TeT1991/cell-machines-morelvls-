using UnityEngine;

public class OneAxisBlock : Block
{
    private bool _needToChangeConstraints;

    protected override void OnEnable()
    {
        base.OnEnable();
        BlockTicksProcessor.Instance.OnTickProcessed += ChangeConstaints;
        Debug.Log(transform.forward);
    }

    public override void Rotate()
    {
        base.Rotate();
        _needToChangeConstraints = true;
    }

    private void ChangeConstaints()
    {
        if (!_needToChangeConstraints) return;
        _needToChangeConstraints = false;

        RigidbodyConstraints2D nextContstraint = RigidbodyConstraints2D.FreezePositionX;

        if (BlockRb.constraints == (RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation))
            nextContstraint = RigidbodyConstraints2D.FreezePositionY;

        BlockRb.constraints = nextContstraint | RigidbodyConstraints2D.FreezeRotation;
    }

    private void OnDisable()
    {
        BlockTicksProcessor.Instance.OnTickProcessed += ChangeConstaints;
    }
}
