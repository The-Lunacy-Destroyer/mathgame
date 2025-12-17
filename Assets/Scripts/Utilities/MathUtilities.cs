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

        public static Vector3 RandomRectPosition(
            float xMin, float xMax, float yMin, float yMax,
            float xMinExclude = 0f, float xMaxExclude = 0f, float yMinExclude = 0f, float yMaxExclude = 0f)
        {
            bool excludeX = xMinExclude - xMaxExclude != 0;
            bool excludeY = yMinExclude - yMaxExclude != 0;
            
            float x, y;

            if (excludeX)
                x = Random.value < 0.5
                    ? Random.Range(xMin, xMinExclude)
                    : Random.Range(xMaxExclude, xMax);
            else x = Random.Range(xMin, xMax);
            
            if (excludeY)
                y = Random.value < 0.5
                    ? Random.Range(yMin, yMinExclude)
                    : Random.Range(yMaxExclude, yMax);
            else y = Random.Range(yMin, yMax);
            
            if (excludeX && excludeY)
            {
                switch (Random.Range(0, 3))
                {
                    case 0:
                        x = Random.Range(xMin, xMax);
                        break;
                    case 1:
                        y = Random.Range(yMin, yMax);
                        break;
                }
            }
            
            return new Vector3(x, y);
        }
    }
}