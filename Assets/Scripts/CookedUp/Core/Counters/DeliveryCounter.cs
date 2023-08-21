using CookedUp.Core.KitchenObjects.Container;
using CookedUp.Core.Players;

namespace CookedUp.Core.Counters
{
    public class DeliveryCounter : BaseCounter
    {
        public KitchenObjectsContainer Container { get; private set;  }
    
        private void Awake() {
            Container = GetComponent<KitchenObjectsContainer>();
        }

        public override void Interact(Player player) {
        
            if (player.HasKitchenObject() && player.CurrentKitchenObject.TryGetPlate(out var plate)) {
                plate.SetContainer(Container);
                DeliveryManager.Instance.DeliverRecipe(plate, player, this);
            
                InvokeOnInteract(new InteractableEvent(player));
            }
            else if (player.Container.IsEmpty() && Container.HasAny()) {
                Container.KitchenObject.SetContainer(player.Container);
            }
        }
    }
}
