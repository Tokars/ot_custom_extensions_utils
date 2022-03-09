namespace OT.Extensions
{
    public static class ArrayExtension
    {
        public static T[] Reverse<T>(this T[] array)
        {
            int i = 0;
            int j = array.Length - 1;

            while (i < j)
            {
                var tmp = array[i];
                array[i] = array[j];
                array[j] = tmp;
                i++;
                j--;
            }

            return array;
        }

        public static T[] Slice<T>(this T[] array, int start, int end)
        {
            if (end < 0)
            {
                end = array.Length + end;
            }

            int len = end - start;

            T[] res = new T[len];
            for (int i = 0; i < len; i++)
            {
                res[i] = array[i + start];
            }

            return res;
        }

        public static T[] Shuffle<T>(this T[] array)
        {
            int n = array.Length;
            while (n > 1)
            {
                n--;
                int k = UnityEngine.Random.Range(0, n + 1);
                var value = array[k];
                array[k] = array[n];
                array[n] = value;
            }

            return array;
        }
    }
}