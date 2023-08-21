using System;
using KitchenObjects;
using Players;

namespace Counters {
    public class TrashCounter : BaseCounter {
        public static event EventHandler<TrashedEvent> OnAnyTrashed;

        public override void Interact(Player player) {
            if (player.HasKitchenObject()) {
                OnAnyTrashed?.Invoke(this, new TrashedEvent(player, this, player.CurrentKitchenObject));
                
                player.CurrentKitchenObject.DestroySelf();
                InvokeOnInteract(new InteractableEvent(player));
            }
        }
    }

    public class TrashedEvent {
        public Player Player { get; private set; }
        public TrashCounter TrashCounter { get; private set; }
        public KitchenObject KitchenObject { get; private set; }

        public TrashedEvent(Player player, TrashCounter trashCounter, KitchenObject kitchenObject) {
            Player = player;
            TrashCounter = trashCounter;
            KitchenObject = kitchenObject;
        }
    }
}
