using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Random = UnityEngine.Random;

namespace Shared.Extensions {
    public static class CollectionExtensions {
        public static T GetRandomElement<T>(this List<T> list) {
            return list[Random.Range(0, list.Count)];
        }

        public static T GetRandomElement<T>(this T[] list) {
            return list[Random.Range(0, list.Length)];
        }

        public static void RemoveAll<T>(this Collection<T> collection, Func<T, bool> condition) {
            for (int i = collection.Count - 1; i >= 0; i--) {
                if (condition(collection[i])) {
                    collection.RemoveAt(i);
                }
            }
        }
    }
}
