using KitchenObjects;
using KitchenObjects.Container;
using Players;
using UnityEngine;

namespace Counters
{
    [RequireComponent(typeof(KitchenObjectsContainer))]
    public class ClearCounter : BaseCounter {

        public KitchenObjectsContainer Container { get; private set; }

        public KitchenObject CurrentKitchenObject => Container.KitchenObject;

        private void Awake() {
            Container = GetComponent<KitchenObjectsContainer>();
        }


        public override void Interact(Player player) {

            if (player.HasKitchenObject()) {
                if (Container.IsEmpty()) {
                    // If the counter is empty, just put the object on it.
                    player.CurrentKitchenObject.SetContainer(Container);
                    InvokeOnInteract(new InteractableEvent(player));
                }
                else if (Container.HasAny()) {
                    if (player.CurrentKitchenObject.InteractWith(CurrentKitchenObject)) {
                        InvokeOnInteract(new InteractableEvent(player));
                    }
                    else if (CurrentKitchenObject.InteractWith(player.CurrentKitchenObject)) {
                        InvokeOnInteract(new InteractableEvent(player));
                    }
                }
            }
            else if (!player.HasKitchenObject() && Container.HasAny()) {
                // If the player is not holding anything,
                // pick up the object on the counter.
                CurrentKitchenObject.SetContainer(player.Container);
            }


            base.InvokeOnInteract(new InteractableEvent(player));
        }

    }
}
