using System;
using ThinkEngine.Planning;
using UnityEngine;

namespace ThinkEngine.Actions {
    public class DropAction : InteractAction {
        private bool hasInteracted;
        private bool hasDropped;


        public override State Prerequisite() {
            var state = base.Prerequisite();
            if (state != State.READY)
                return state;

            if (!Player.HasKitchenObject()) {
                Debug.LogError($"{Player.name} does not have a kitchen object, to drop on {Target.name}");
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
            hasInteracted = true;
            hasDropped = !Player.HasKitchenObject();
            PlayerMovement.StopAll();
        }

        public override State Done() {
            if (AnyError) {
                OnDone();
                return State.ABORT;
            }

            if (hasInteracted) {
                OnDone();
                return State.READY;
            }
            
            if(HasReachedTarget && !IsTargetInRange())
                HasReachedTarget = false;
            
            if (!HasReachedTarget && !IsMoving) {
                TryMoveToTarget();
            }
            
            if (IsTargetSelected() && Player.TryInteract()) {
                if (hasInteracted || (HasTargetContainer && !TargetContainer.HasSpace())) {
                    OnDone();
                    return hasDropped ? State.READY : State.ABORT;
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
            UnsubscribeMoveToEvents();
        }
        
    }
}
