using ThinkEngine.Planning;
using UnityEngine;

namespace ThinkEngine.Actions
{
    public class PickUpAction : InteractAction
    {
        private bool hasInteracted;
        private bool hasPickedUp;


        public override State Prerequisite() {
            var state = base.Prerequisite();
            if (state != State.READY)
                return state;

            if (Player.HasKitchenObject()) {
                Debug.LogError($"{Player.name} already has a kitchen object {Target.name}");
                return State.ABORT;
            }

            return State.READY;
        }

        public override void Do() {
            Interactable.OnInteract += OnInteract;
        }

        private void OnInteract(object sender, InteractableEvent e) {
            hasInteracted = true;
            hasPickedUp = Player.HasKitchenObject();
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
                Debug.Log($"player interacted with the target {Target.name}");
                if(hasInteracted) {
                    OnDone();
                    return true;
                }
            }

            return false;
        }

        private void OnDone()
        {
            if (hasPickedUp)
                Debug.Log($"{Player.name} picked up a kitchen object {Target.name}");
            else
                Debug.LogError($"{Player.name} did not pick up anything {Target.name}");

            Interactable.OnInteract -= OnInteract;
        }
    }
}
