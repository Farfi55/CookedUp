using Unity.Collections;
using UnityEngine;

namespace ThinkEngine.Sensors {
    public class BaseSensorData : MonoBehaviour {
        
        [SerializeField] private GameObject target;
        
        [Header("Sensor Data")]
        public int ID;
        public string Type;
        public string Name;

        private void Start() {
            ID = target.GetInstanceID();
            Type = target.GetType().Name;
            Name = target.name;
        }
    }
}
