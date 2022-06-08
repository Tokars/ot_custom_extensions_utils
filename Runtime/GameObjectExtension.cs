using UnityEngine;

namespace OT.Extensions
{
    public static class GameObjectExtension
    {
        /// <summary>
        /// Set layer gameobject.
        /// </summary>
        /// <param name="gameobject">GameObject</param>
        /// <param name="layer">layer num.</param>
        /// <param name="includeChildren">Toggle set for children</param>
        public static void SetLayer<T>(this GameObject gameobject, int layer, bool includeChildren = false)
            where T : Component
        {
            gameobject.layer = layer;
            if (includeChildren == false) return;

            var arr = gameobject.GetComponentsInChildren<T>(true);
            for (int i = 0; i < arr.Length; i++)
                arr[i].gameObject.layer = layer;
        }
    }
}