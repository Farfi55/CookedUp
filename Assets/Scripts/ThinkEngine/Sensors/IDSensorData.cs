using Unity.Collections;
using UnityEngine;

namespace ThinkEngine.Sensors {
    public class IDSensorData : MonoBehaviour {
        
        [SerializeField] private GameObject target;
        
        public int gameObjectID;
        
        private void Start() {
            gameObjectID = target.GetInstanceID();
        }
    }
}
