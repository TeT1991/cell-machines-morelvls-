using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block2 : MonoBehaviour
{
    [SerializeField] private List<Vector3> _allowedDirections;

    private void Start()
    {
    }

    public void TryMove(Vector3 direction)
    {
        if (TryGetAllowedDirection(direction))
        {

        }
    }

    private bool TryGetAllowedDirection(Vector3 direction)
    {
        foreach (var dir in _allowedDirections)
        {
            if(dir.x == direction.x && dir.y == direction.y && dir.z == direction.z)
            {
                return true;
            }
        }

        return false;
    }

    
}
