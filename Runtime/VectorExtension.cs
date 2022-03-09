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
    }
}
