using Unity.Collections;
using UnityEngine;

namespace ThinkEngine.Sensors {
    public class BaseSensorData : MonoBehaviour {
        
        [SerializeField] private GameObject target;
        
        public int id;
        public string type;
        public string name;

        private void Start() {
            id = target.GetInstanceID();
            type = target.GetType().Name;
            name = target.name;
        }
    }
}
