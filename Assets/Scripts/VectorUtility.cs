using UnityEngine;

public static class VectorUtility
{
    public static Vector2 RoundVector(Vector2 vector)
    {
        vector.x = Mathf.RoundToInt(vector.x);
        vector.y = Mathf.RoundToInt(vector.y);
        return vector;
    }
}
