using UnityEngine;

namespace CookedUp.Core.Counters.Visual
{
    [RequireComponent(typeof(CuttingCounter))]
    public class CuttingCounterVisual : MonoBehaviour {

        [SerializeField] private Animator[] animators;
        private CuttingCounter cuttingCounter;

        private readonly int cutHash = Animator.StringToHash("Cut");
        
        private void Start() {
            cuttingCounter = GetComponent<CuttingCounter>();
            
            cuttingCounter.OnChop += (sender, e) => {
                foreach (var animator in animators) {
                    animator.SetTrigger(cutHash);
                }
            };
        }

    }
}
