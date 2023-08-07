using System;
using Players;
using ThinkEngine.Planning;
using UnityEngine;
using Action = ThinkEngine.Planning.Action;

namespace ThinkEngine.Actions {
    public abstract class InteractAction : Action, IPlayerAction {
        public int PlayerID { get; set; }
        public int TargetID { get; set; }

        protected PlayersManager PlayersManager;
        protected IDManager IDManager;

        public Player Player { get; private set; }
        protected PlayerMovement PlayerMovement;


        protected GameObject Target => target;
        protected IInteractable Interactable => interactable;

        private GameObject target;
        private IInteractable interactable;
        
        protected bool HasReachedTarget = false;

        protected bool AnyError;

        protected virtual void Setup() {
            IDManager = IDManager.Instance;
            PlayersManager = PlayersManager.Instance;

            if (PlayerID == 0) {
                Debug.LogError("PlayerID was not set!");
                AnyError = true;
            }
            else {
                Player = IDManager.GetComponentFromID<Player>(PlayerID);
                PlayerMovement = Player.GetComponent<PlayerMovement>();
                
                if (PlayerMovement == null) {
                    Debug.LogError($"Player {Player.name} does not have a PlayerMovement component!");
                    AnyError = true;
                }
                else if (!PlayerMovement.HasAgent) {
                    Debug.LogError($"Player {Player.name} does not have an agent!");
                    AnyError = true;
                }
            }

            if (TargetID != 0) {
                target = IDManager.GetGameObject(TargetID);

                if (!target.TryGetComponent(out interactable)) {
                    Debug.LogError($"Target {target.name} does not have an IInteractable component!");
                    AnyError = true;
                }
            }
            else {
                Debug.LogError("TargetID was not set!");
                AnyError = true;
            }
        }

        public override State Prerequisite() {
            if (IDManager == null) {
                Setup();
            }

            if (AnyError)
                return State.ABORT;

            return State.READY;
        }

        protected virtual bool TryMoveToTarget() {
            HasReachedTarget = false;
            
            if (PlayerMovement.TryMoveToAndLookAt(Target.transform)) {
                PlayerMovement.OnMoveToCompleted += OnMoveToTargetComplete;
                PlayerMovement.OnMoveToCanceled += OnMoveToTargetCanceled;
                return true;
            }
            return false;
        }

        protected virtual void OnMoveToTargetCanceled(object sender, EventArgs e) {
            Debug.Log($"player has canceled the move to the target {Target.name}");
            HasReachedTarget = false;
            UnsubscribeMoveToEvents();
        }

        protected virtual void OnMoveToTargetComplete(object sender, EventArgs e) {
            Debug.Log($"player has reached the target {Target.name}");
            HasReachedTarget = true;
            UnsubscribeMoveToEvents();
        }
        
        protected void UnsubscribeMoveToEvents()
        {
            PlayerMovement.OnMoveToCompleted -= OnMoveToTargetComplete;
            PlayerMovement.OnMoveToCanceled -= OnMoveToTargetCanceled;
        }
    }
}
