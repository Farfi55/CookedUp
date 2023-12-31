using UnityEngine;

namespace CookedUp.Core.Counters.Visual
{
    [RequireComponent(typeof(ContainerCounter))]
    public class ContainerCounterVisual : MonoBehaviour {

        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Animator animator;

        private ContainerCounter containerCounter;

        private readonly int OpenCloseHash = Animator.StringToHash("OpenClose");

        private void Awake() {

            containerCounter = GetComponent<ContainerCounter>();
            spriteRenderer.sprite = containerCounter.KitchenObjectSO.Sprite;
            containerCounter.OnInteract += (sender, e) => {
                animator.SetTrigger(OpenCloseHash);
            };
        }


        [ContextMenu("set sprite")]
        private void SetSprite() {
            if (containerCounter == null)
                containerCounter = GetComponent<ContainerCounter>();
            spriteRenderer.sprite = containerCounter.KitchenObjectSO.Sprite;

#if UNITY_EDITOR
            // save unity state
            UnityEditor.EditorUtility.SetDirty(this);
#endif
        }
    }
}
