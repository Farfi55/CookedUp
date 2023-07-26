using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
    class CecilUtility
    {
        public int t { get; set; }

        public void thinkengineDebugger()
        {
            t = 0;
            Debug.Log("my setter");
        }
    }

