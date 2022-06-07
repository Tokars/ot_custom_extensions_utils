using UnityEngine;

namespace CustomExtension
{
    public static class VectorExtension
    {
        public static Vector3 RandomRange(this Vector3 vector)
        {
            return new Vector3(Random.Range(-vector.x, vector.x), Random.Range(-vector.y, vector.y), Random.Range(-vector.z, vector.z));
        }

        public static Vector2 RandomRange(this Vector2 vector)
        {
            return new Vector3(Random.Range(-vector.x, vector.x), Random.Range(-vector.y, vector.y));
        }

        public static Vector3 Clamp(this Vector3 vector, float minValue, float maxValue)
        {
            return new Vector3(Mathf.Clamp(vector.x, minValue, maxValue), Mathf.Clamp(vector.y, minValue, maxValue), Mathf.Clamp(vector.z, minValue, maxValue));
        }

        public static Vector2 Clamp(this Vector2 vector, float minValue, float maxValue)
        {
            return new Vector2(Mathf.Clamp(vector.x, minValue, maxValue), Mathf.Clamp(vector.y, minValue, maxValue));
        }
        
        public static Vector3 BezierInterpolateV3(Vector3 p0, Vector3 c0, Vector3 p1, float t)
        {
            Vector3 p0c0 = Vector3.LerpUnclamped(p0, c0, t);
            Vector3 c0p1 = Vector3.LerpUnclamped(c0, p1, t);

            return Vector3.LerpUnclamped(p0c0, c0p1, t);
        }

        public static Vector2 BezierInterpolateV2(Vector2 p0, Vector2 c0, Vector2 p1, float t)
        {
            Vector3 p0c0 = Vector2.LerpUnclamped(p0, c0, t);
            Vector3 c0p1 = Vector2.LerpUnclamped(c0, p1, t);

            return Vector2.LerpUnclamped(p0c0, c0p1, t);
        }


        public static Vector3 BezierInterpolate4(Vector3 p0, Vector3 c0, Vector3 c1, Vector3 p1, float t)
        {
            Vector3 p0c0 = Vector3.LerpUnclamped(p0, c0, t);
            Vector3 c0c1 = Vector3.LerpUnclamped(c0, c1, t);
            Vector3 c1p1 = Vector3.LerpUnclamped(c1, p1, t);

            Vector3 x = Vector3.LerpUnclamped(p0c0, c0c1, t);
            Vector3 y = Vector3.LerpUnclamped(c0c1, c1p1, t);

            return Vector3.LerpUnclamped(x, y, t);
        }

    }
}
