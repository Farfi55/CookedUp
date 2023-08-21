using KitchenObjects;
using KitchenObjects.Container;
using KitchenObjects.ScriptableObjects;
using Players;
using UnityEngine;

namespace Counters
{
    public class ContainerCounter : BaseCounter {
        [SerializeField] private KitchenObjectSO kitchenObjectSO;
        public KitchenObjectSO KitchenObjectSO => kitchenObjectSO;

        private KitchenObjectsContainer container;

        private void Awake() {
            container = GetComponent<KitchenObjectsContainer>();
        }

        private void Start() {
            container.OnKitchenObjectRemoved += ContainerOnOnKitchenObjectRemoved;
        
            if (container.IsEmpty()) {
                var kitchenObject = KitchenObject.CreateInstance(kitchenObjectSO, container);
                kitchenObject.SetVisible(false);
            }
        }

        private void ContainerOnOnKitchenObjectRemoved(object sender, KitchenObject e) {
            if (container.IsEmpty()) {
                var kitchenObject = KitchenObject.CreateInstance(kitchenObjectSO, container);
                kitchenObject.SetVisible(false);
            }
        }


        public override void Interact(Player player) {
            if (player.HasKitchenObject()) {
                if (player.CurrentKitchenObject.IsSameType(kitchenObjectSO)) {
                    player.CurrentKitchenObject.DestroySelf();
                    InvokeOnInteract(new InteractableEvent(player));
                }
                else if (player.CurrentKitchenObject.InteractWith(container.KitchenObject)) {
                    InvokeOnInteract(new InteractableEvent(player));
                }
            }
            else {
                container.KitchenObject.SetContainer(player.Container);
                InvokeOnInteract(new InteractableEvent(player));
            }
        }
    }
}
