using System;
using ThinkEngine.Planning;
using UnityEngine;

namespace ThinkEngine.Actions
{
    public class PickUpAction : InteractAction
    {
        protected bool HasInteracted;
        protected bool HasPickedUp;
        
        private float interactionDelay = 0.2f;
        private float remainingInteractionDelay;


        public override void Init() {
            base.Init();
            remainingInteractionDelay = interactionDelay;
        }

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
            PlayerMovement.StopAll();
        }

        public override State Done() {
            if (AnyError) {
                return State.ABORT;
            }
            
            if(HasReachedTarget && !IsTargetSelected() && !IsTargetInRange())
                HasReachedTarget = false;
            
            if (!IsTargetSelected()) {
                remainingInteractionDelay = interactionDelay;
                if (!HasReachedTarget && !IsMoving) {
                    TryMoveToTarget();
                }
            }
            else {
                remainingInteractionDelay -= Time.deltaTime;
            }
            
            if (remainingInteractionDelay <= 0f && IsTargetSelected() && Player.TryInteract()) {
                Debug.Log($"player interacted with the target {Target.name}");
                if(HasInteracted) {
                    return HasPickedUp ? State.READY : State.ABORT;
                }
            }

            return State.WAIT;
        }

        public override void Clean() {
            base.Clean();
            if (HasPickedUp)
                Debug.Log($"{Player.name} picked up a kitchen object {Target.name}");
            else
                Debug.LogError($"{Player.name} did not pick up anything {Target.name}");

            if (Interactable != null) {
                Interactable.OnInteract -= OnInteract;
            }
            UnsubscribeMoveToEvents();
            PlayerMovement.StopAll();
        }
    }
}
