using System;
using Counters;
using UnityEngine;

namespace ThinkEngine.Sensors.Counters {
    public class ContainerCounterSensorData : MonoBehaviour {
        [SerializeField] private ContainerCounter container;
        
        public string koType;

        private void Start() {
            koType = container.KitchenObjectSO.name;
        }
    }
}
