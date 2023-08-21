using UnityEngine;

namespace Shared
{
    public class FrameRateManager : MonoBehaviour {
 
        public static FrameRateManager Instance { get; private set; }

        [SerializeField] private int targetFrameRate = 60;
    
        private void Awake() {
            if (Instance == null) {
                Instance = this;
            }
            else {
                Debug.LogError("DebugManager already exists!");
                Destroy(gameObject);
            }
        }
    
    
        private void Start() {
            Application.targetFrameRate = targetFrameRate;
        }

#if UNITY_EDITOR
        private void OnValidate() {
            Application.targetFrameRate = targetFrameRate;
        }
#endif
    }
}
