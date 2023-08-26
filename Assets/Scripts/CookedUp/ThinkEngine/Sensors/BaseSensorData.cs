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
            if (idManager == null) {
                Debug.LogError("IDManager not found in scene, disabling gameObject");
                gameObject.SetActive(false);
                return;
            }
            
            ID = idManager.GetID(target.gameObject);
            Type = target.GetType().Name;
            Name = target.name;
        }
        
        private void OnDestroy() {
            if (idManager != null)
                idManager.RemoveID(ID);
        }
    }
}
