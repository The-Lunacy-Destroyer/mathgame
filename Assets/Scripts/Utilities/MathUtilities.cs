using UnityEngine;

namespace Utilities
{
    public static class MathUtilities
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

        public static float RoundTo(float value, int digits)
        {
            float scale = Mathf.Pow(10, digits);
            return Mathf.Round(value * scale) / scale;
        }

        public static Vector3 RandomRectPosition(float xMin, float xMax, float yMin, float yMax)
        {
            return new Vector3(Random.Range(xMin, xMax), Random.Range(yMin, yMax));
        }
    }
}