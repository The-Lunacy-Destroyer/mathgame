using UnityEngine;

public static class Utilities
{
    public static Vector2 RotateVector(Vector2 vector, float angle)
    {
        angle *= Mathf.Deg2Rad;
        
        return new Vector2
        (
            vector.x * Mathf.Cos(angle) + vector.y * -Mathf.Sin(angle),
            vector.x * Mathf.Sin(angle) + vector.y * Mathf.Cos(angle)
        );
    }
}