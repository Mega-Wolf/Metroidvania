using System.Collections.Generic;

namespace Helpers {

    public static class CollectionHelper {

        public static bool IsNullOrEmpty<T>(this ICollection<T> collection) {
            return collection == null || collection.Count == 0;
        }
        
    }

}