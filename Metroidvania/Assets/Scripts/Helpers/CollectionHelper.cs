using System.Collections.Generic;
using UnityEngine;

namespace Helpers {

    public static class CollectionHelper {

        public static bool IsNullOrEmpty<T>(this ICollection<T> collection) {
            return collection == null || collection.Count == 0;
        }

        public static void Shuffle<T>(this List<T> list) {
            for (var i = list.Count - 1; i > 0; i--) {
                var j = (int)Mathf.Floor(Random.value * (i + 1));
                var temp = list[i];
                list[i] = list[j];
                list[j] = temp;
            }

        }

    }

}