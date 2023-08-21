using UnityEngine;

namespace CookedUp.ThinkEngine.Sensors {
    public class BaseSensorData : MonoBehaviour {
        
        private IDManager idManager;
        [SerializeField] private MonoBehaviour target;
        
        [Header("Sensor Data")]
        public int ID;
        public string Type;
        public string Name;

        private void Start() {
            idManager = IDManager.Instance;
            ID = idManager.GetID(target.gameObject);
            Type = target.GetType().Name;
            Name = target.name;
        }
        
        private void OnDestroy() {
            idManager.RemoveID(ID);
        }
    }
}
