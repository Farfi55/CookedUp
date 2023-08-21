using Shared;
using UnityEngine;

namespace CookedUp.UI {
    
    public class LoadingSceneProgressBarUI : MonoBehaviour {
        [SerializeField] private ProgressTracker progressTracker;

        private void Start() {
            progressTracker.SetTotalWork(1f);
        }
        
        private void Update() {
            var loadingProgress = SceneLoader.GetLoadingProgress();
            progressTracker.SetProgress(loadingProgress);
        }
    }
}
