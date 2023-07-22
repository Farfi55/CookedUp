using UnityEngine;

namespace ThinkEngine.Sensors {
    public class GridPositionSensorData : MonoBehaviour {
        
        [SerializeField] private Transform target;
        
        private GridManager gridManager;
        public Vector2Int pos;
        
        private void Start() {
            gridManager = GridManager.Instance;
            pos = gridManager.GetGridPosition(target.position);
        }
        
        private void Update() {
            pos = gridManager.GetGridPosition(target.position);
        }
        
    }
}
