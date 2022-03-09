using System;
using System.Collections.Generic;
using System.Linq;

namespace OT.Extensions
{
    public static class EnumExtension
    {
        public static IEnumerable<T> GetCastValues<T>()
        {
            return Enum.GetValues(typeof(T)).Cast<T>();
        }

        public static T[] ValuesToArray<T>()
        {
            return (T[]) Enum.GetValues(typeof(T)).Cast<T>();
        }

        public static T RandomEnumValue<T>()
        {
            var v = Enum.GetValues(typeof(T));
            return (T) v.GetValue(UnityEngine.Random.Range(0, v.Length));
        }

        public static T Next<T>(this T src) where T : struct
        {
            if (!typeof(T).IsEnum) throw new ArgumentException($"Argument {typeof(T).FullName} is not an Enum");

            T[] arr = (T[]) Enum.GetValues(src.GetType());
            int j = Array.IndexOf<T>(arr, src) + 1;
            return (arr.Length == j) ? arr[0] : arr[j];
        }
    }
}