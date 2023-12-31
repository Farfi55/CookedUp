﻿using CookedUp.Core;
using ThinkEngine.Planning;
using UnityEngine;

namespace CookedUp.ThinkEngine.Actions {
    public class DropAction : InteractAction {
        protected bool HasInteracted;
        protected bool HasDropped;


        private float interactionDelay = 0.2f;
        private float remainingInteractionDelay;

        protected bool AbortOnInteractableFull = true;


        public override void Init() {
            base.Init();
            remainingInteractionDelay = interactionDelay;
        }

        public override State Prerequisite() {
            var state = base.Prerequisite();
            if (state != State.READY)
                return state;

            if (!Player.HasKitchenObject()) {
                Debug.LogWarning($"[{GetType().Name}]: {Player.name} does not have a kitchen object, to drop on {Target.name}");
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
            HasDropped = !Player.HasKitchenObject();
            PlayerMovement.StopAll();
        }

        public override State Done() {
            if (AnyError) {
                return State.ABORT;
            }

            if (HasInteracted) {
                return State.READY;
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
            
            if ((remainingInteractionDelay <= 0f || HasReachedTarget) && IsTargetSelected() && Player.TryInteract()) {
                if (HasInteracted) {
                    return HasDropped ? State.READY : State.ABORT;
                }
                else if (AbortOnInteractableFull && HasTargetContainer && !TargetContainer.HasSpace()) {
                    return State.ABORT;
                }
            }

            return State.WAIT;
        }

        public override void Clean() {
            base.Clean();
            
            if (HasDropped)
                Debug.Log($"[{GetType().Name}]: {Player.name} dropped a kitchen object {Target.name}");
            else
                Debug.LogWarning($"[{GetType().Name}]: {Player.name} did not drop a kitchen object {Target.name}");

            if (Interactable != null) {
                Interactable.OnInteract -= OnInteract;
            }
            UnsubscribeMoveToEvents();
        
        }
        
    }
}
