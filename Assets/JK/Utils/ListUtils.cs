using JK.Interaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JK.Utils
{
    public static class ListUtils
    {
        /// <summary>
        /// Randomly shuffles a list in place
        /// https://stackoverflow.com/questions/273313/randomize-a-listt/1262619#1262619
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        public static void ShuffleInPlace<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = UnityEngine.Random.Range(0, n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        private static readonly List<int> indexes = new List<int>(32);

        public static IEnumerable<T> EnumeraterRandomly<T>(this IList<T> list)
        {
            indexes.Clear();
            indexes.AddRange(EnumerateUpTo(list.Count));
            indexes.ShuffleInPlace();

            for (int i = 0; i < indexes.Count; i++)
                yield return list[i];
        }

        public static IEnumerable<int> EnumerateUpTo(int n)
        {
            for (int i = 0; i < n; i++)
                yield return i;
        }
    }
}
