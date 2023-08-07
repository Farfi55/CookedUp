using System;
using ThinkEngine.Planning;
using UnityEngine;

namespace ThinkEngine.Actions
{
    public class PickUpAction : InteractAction
    {
        protected bool HasInteracted;
        protected bool HasPickedUp;
        

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
            if (!TryMoveToTarget()) {
                Debug.LogError($"{Player.name} cannot move to {Target.name}");
                AnyError = true;
            }
            
            Interactable.OnInteract += OnInteract;
        }

        private void OnInteract(object sender, InteractableEvent e) {
            HasInteracted = true;
            HasPickedUp = Player.HasKitchenObject();
        }

        public override State Done() {
            if (AnyError)
                return State.ABORT;
            
            if (HasInteracted) {
                OnDone();
                return State.READY;
            }
            
            if (Player.GetSelectedGameObject() != Target)
                return State.WAIT;
            
            // if (!HasReachedTarget) {
            //     return State.WAIT;
            // }

            if(Player.TryInteract()) {
                Debug.Log($"player interacted with the target {Target.name}");
                if(HasInteracted) {
                    OnDone();
                    return State.READY;
                }
            }

            return State.WAIT;
        }

        private void OnDone()
        {
            if (HasPickedUp)
                Debug.Log($"{Player.name} picked up a kitchen object {Target.name}");
            else
                Debug.LogError($"{Player.name} did not pick up anything {Target.name}");

            Interactable.OnInteract -= OnInteract;
            UnsubscribeMoveToEvents();
        }
    }
}
