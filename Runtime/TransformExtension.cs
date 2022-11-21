using UnityEngine;

namespace OT.Extensions
{
    public static class TransformExtension
    {
        public static void Clear(this Transform transform)
        {
            var count = transform.childCount - 1;
            for (var i = count; i >= 0; i--)
            {
#if UNITY_EDITOR
                UnityEditor.Undo.DestroyObjectImmediate(transform.GetChild(i).gameObject);
#else
                Object.Destroy(transform.GetChild(i).gameObject);
#endif
            }
        }
    }
}