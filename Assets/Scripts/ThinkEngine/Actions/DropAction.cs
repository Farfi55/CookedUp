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

        public override bool Done() {

            if (hasInteracted) {
                OnDone();
                return true;
            }
            
            if (Player.GetSelectedGameObject() != Target) {
                Debug.LogError("player is not looking at the target anymore");
                return false;
            }

            if(Player.TryInteract()) {
                if (hasInteracted) {
                    OnDone();
                    return true;
                }
            }
            return false;
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
