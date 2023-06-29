using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

namespace Extensions {
    public static class CollectionExtensions {

        public static T GetRandomElement<T>(this List<T> list) {
            return list[Random.Range(0, list.Count)];
        }
        
        public static T GetRandomElement<T>(this T[] list) {
            return list[Random.Range(0, list.Length)];
        }
        
    }
}
