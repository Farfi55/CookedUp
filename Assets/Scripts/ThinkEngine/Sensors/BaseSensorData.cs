using Unity.Collections;
using UnityEngine;

namespace ThinkEngine.Sensors {
    public class BaseSensorData : MonoBehaviour {
        
        private IDManager idManager;
        [SerializeField] private GameObject target;
        
        [Header("Sensor Data")]
        public int ID;
        public string Type;
        public string Name;

        private void Start() {
            idManager = IDManager.Instance;
            ID = idManager.GetID(target);
            Type = target.GetType().Name;
            Name = target.name;
        }
        
        private void OnDestroy() {
            idManager.RemoveGameObject(ID);
        }
    }
}
