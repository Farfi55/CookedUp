using System;
using Counters;
using UnityEngine;

namespace ThinkEngine.Sensors.Counters {
    public class CuttingCounterSensorData : MonoBehaviour {
        [SerializeField] private ContainerCounter container;
        
        public bool hasAny;
        public bool isCutting;
        public bool isDoneCutting;
        
        
        
        public string koType;

        private void Start() {
            koType = container.KitchenObjectSO.name;
        }
    }
}
