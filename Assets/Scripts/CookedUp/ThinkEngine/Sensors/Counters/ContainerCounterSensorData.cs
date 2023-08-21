using CookedUp.Core.Counters;
using UnityEngine;

namespace CookedUp.ThinkEngine.Sensors.Counters {
    public class ContainerCounterSensorData : MonoBehaviour {
        [SerializeField] private ContainerCounter containerCounter;
        
        [Header("Sensor Data")]
        public string KOType;

        private void Start() {
            KOType = containerCounter.KitchenObjectSO.name;
        }
    }
}
