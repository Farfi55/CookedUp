using System.Collections.Generic;
using UnityEngine;

namespace ThinkEngine
{
    [ExecuteInEditMode]
    class IndexTracker : MonoBehaviour
    {
        private int _currentIndex = -1;
        internal int CurrentIndex
        {
            get
            {
                if (_currentIndex == -1)
                {
                    _currentIndex = GlobalIndexer.assignIndex();
                }
                return _currentIndex;
            }
        }

    }
}