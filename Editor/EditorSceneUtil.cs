using System.Collections.Generic;
using UnityEngine;

namespace OT.Extensions
{
    public static class EditorSceneUtil
    {
        public static T[] FindTypesInScene<T>() where T : class
        {
            List<T> result = new List<T>();
            var arr = GameObject.FindObjectsOfType<MonoBehaviour>();
            for (int i = 0; i < arr.Length; i++)
            {
                var monoBehaviour = arr[i];
                if (monoBehaviour is T type)
                    result.Add(type);
            }

            return result.ToArray();
        }

        public static T FindTypeInScene<T>() where T : class
        {
            var arr = GameObject.FindObjectsOfType<MonoBehaviour>();
            for (int i = 0; i < arr.Length; i++)
            {
                var monoBehaviour = arr[i];
                if (monoBehaviour is T type) return type;
            }

            return default;
        }
    }
}