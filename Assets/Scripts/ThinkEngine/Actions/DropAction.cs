using ThinkEngine.Planning;
using UnityEngine;

namespace ThinkEngine.Actions
{
    public class DropAction : InteractAction
    {
        private bool hasInteracted;
        private bool hasDropped;
        
        
        public override State Prerequisite() {
            var state = base.Prerequisite();
            if (state != State.READY)
                return state;

            if (!Player.HasKitchenObject()) {
                Debug.LogError($"{Player.name} does not have a kitchen object {Target.name}");
                return State.ABORT;
            }

            return State.READY;
        }
        
        public override void Do() {
            Interactable.OnInteract += OnInteract;
        }

        private void OnInteract(object sender, InteractableEvent e) {
            hasInteracted = true;
            hasDropped = !Player.HasKitchenObject();
        }

        public override State Done() {

            if (hasInteracted) {
                OnDone();
                return State.READY;
            }
            
            if (Player.GetSelectedGameObject() != Target) {
                Debug.LogError("player is not looking at the target anymore");
                return State.WAIT;
            }

            if(Player.TryInteract()) {
                if (hasInteracted) {
                    OnDone();
                    return State.READY;
                }
            }
            return State.WAIT;
        }

        private void OnDone() {
            if (hasDropped)
                Debug.Log($"{Player.name} dropped a kitchen object {Target.name}");
            else
                Debug.LogError($"{Player.name} did not drop a kitchen object {Target.name}");
                
            Interactable.OnInteract -= OnInteract;
        }
    }
}
