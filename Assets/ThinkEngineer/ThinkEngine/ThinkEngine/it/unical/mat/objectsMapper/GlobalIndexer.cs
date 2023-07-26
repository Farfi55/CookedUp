using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ThinkEngine
{
    class GlobalIndexer
    {
        private static object toLock = new object();
        static internal int maxIndexUsed;

        internal static int assignIndex()
        {
            lock (toLock)
            {
                return ++maxIndexUsed;
            }
        }
        internal static GameObject find(int index)
        {
            foreach (IndexTracker tracker in Resources.FindObjectsOfTypeAll<IndexTracker>())
            {
                if (tracker.CurrentIndex == index)
                {
                    return tracker.gameObject;
                }
            }
            return null;
        }
    }
}