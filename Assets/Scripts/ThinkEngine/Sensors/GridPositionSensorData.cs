using UnityEngine;

namespace ThinkEngine.Sensors {
    public class GridPositionSensorData : MonoBehaviour {
        
        [SerializeField] private Transform target;
        private GridManager gridManager;
        
        [Header("Sensor Data")]
        public Vector2Int Pos;
        
        private void Start() {
            gridManager = GridManager.Instance;
            Pos = gridManager.GetGridPosition(target.position);
        }
        
        private void Update() {
            Pos = gridManager.GetGridPosition(target.position);
        }
        
    }
}
