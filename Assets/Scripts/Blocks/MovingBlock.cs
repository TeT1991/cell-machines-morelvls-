using UnityEditor;
using UnityEngine;

public class MovingBlock : Block
{
    protected override void Action()
    {
        if (ThisObjectTransform != null)
        {
            if (CalculateEmptyBlocksCount() > 0)
            {

                Move(ThisObjectTransform.right);
            }
        }

    }

    private int CalculateEmptyBlocksCount()
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

        OneAxisBlock oneAxisBlock = HasMovableOneAxisBlock(blocksHitsInfo);

        Debug.Log(transform.localRotation.z);
        //Debug.Log(oneAxisBlock.transform.rotation.z);


        if (oneAxisBlock != null && transform.right == Vector3.forward)
        {
            Debug.Log(oneAxisBlock.transform.rotation.z);
            Destroy(oneAxisBlock.gameObject);
            //return (int)Vector3.Distance(transform.position, oneAxisBlock.transform.position);
        }

        Debug.Log("Blocks Count - " + blocksCount);

        int walllayerMask = LayerMask.GetMask("WallLayer");
        RaycastHit2D[] wallsHitsInfo = Physics2D.RaycastAll(ThisObjectTransform.position, ThisObjectTransform.right, lenght, walllayerMask);

        if (gameObject.transform != null)
        {
            int result = (int)Vector3.Distance(transform.position, wallsHitsInfo[0].transform.position) - 1 - blocksCount;
            Debug.Log(result);
            return (int)Vector3.Distance(transform.position, wallsHitsInfo[0].transform.position) - 1 - blocksCount;
        }



        return 0;
    }

    private OneAxisBlock HasMovableOneAxisBlock(RaycastHit2D[] blocksHitsInfo)
    {
        foreach (var item in blocksHitsInfo)
        {
            if (item.collider.gameObject.TryGetComponent<OneAxisBlock>(out OneAxisBlock collidedBlock))
            {
                if (collidedBlock.gameObject.transform.forward != transform.forward && -collidedBlock.gameObject.transform.forward != transform.forward)
                    return collidedBlock;
            }
        }

        return null;
    }
}
