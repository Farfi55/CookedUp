using UnityEngine;

namespace CookedUp.Core
{
    public class GridManager : MonoBehaviour
    {
        public static GridManager Instance { get; private set; }
    
        public Grid Grid { get; private set; }


        private void Awake() {
            if (Instance == null) {
                Instance = this;
            }
            else {
                Debug.LogError("Multiple GridManagers in scene");
                Destroy(gameObject);
            }
        
            Grid = GetComponent<Grid>();
            if (Grid == null) {
                Debug.LogError("GridManager has no Grid component");
                return;
            }


            if (Grid.cellSwizzle != GridLayout.CellSwizzle.XZY) {
                Debug.LogError($"GridManager's Grid component has wrong cellSwizzle, should be XZY, is {Grid.cellSwizzle}.\n" +
                               "automatically setting cellSwizzle to XZY...");
                Grid.cellSwizzle = GridLayout.CellSwizzle.XZY;
            }
            
        }
    
    
        public Vector2Int GetGridPosition(Vector3 worldPosition) {
            var pos = Grid.WorldToCell(worldPosition);
            return new Vector2Int(pos.x, pos.y);
        }
    
        public Vector3 GetWorldPosition(Vector2Int gridPosition) {
            var pos = new Vector3Int(gridPosition.x, gridPosition.y, 0);
            return Grid.CellToWorld(pos);
        }
    }
}
