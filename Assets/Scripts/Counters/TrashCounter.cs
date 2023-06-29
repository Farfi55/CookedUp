namespace Counters
{
    public class TrashCounter : BaseCounter {

        public override void Interact(Player.Player player) {
            if (player.HasKitchenObject()) {
                player.CurrentKitchenObject.DestroySelf();
                InvokeOnInteract(new InteractableEvent(player));
            }
        }
    }
}
