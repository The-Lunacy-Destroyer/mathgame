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

        public static Vector3 RectRandomPos(float x_min, float x_max, float y_min, float y_max)
        {
            float rand_x = Random.Range(x_min, x_max);
            float rand_y = Random.Range(y_min, y_max);
            return new Vector3(rand_x, rand_y);
        }
    }
}