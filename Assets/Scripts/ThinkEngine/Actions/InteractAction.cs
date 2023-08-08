using System;
using KitchenObjects.Container;
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
        protected KitchenObjectsContainer TargetContainer;
        protected bool HasTargetContainer => TargetContainer != null;
        
        protected bool HasReachedTarget = false;
        public bool IsMoving => PlayerMovement.IsMoving;

        protected bool AnyError;
        
        public override void Init() {
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
                TargetContainer = target.GetComponent<KitchenObjectsContainer>();
            }
            else {
                Debug.LogError("TargetID was not set!");
                AnyError = true;
            }
        }

        public override State Prerequisite() {
            if (AnyError)
                return State.ABORT;

            return State.READY;
        }

        
        protected float GetDistanceToTarget() => Vector3.Distance(Player.transform.position, Target.transform.position);
        
        protected bool IsTargetInRange() => GetDistanceToTarget() <= Player.InteractionDistance;
        
        protected bool IsTargetSelected() => Target == Player.GetSelectedGameObject();
        
        protected bool TryMoveToTarget() {
            HasReachedTarget = false;
            
            if (PlayerMovement.TryMoveToAndLookAt(Target.transform)) {
                PlayerMovement.OnMoveToCompleted += OnMoveToTargetComplete;
                PlayerMovement.OnMoveToCanceled += OnMoveToTargetCanceled;
                return true;
            }
            return false;
        }

        protected void OnMoveToTargetCanceled(object sender, EventArgs e) {
            Debug.Log($"{GetType().Name}: player has canceled the move to the target {Target.name}");
            HasReachedTarget = false;
            UnsubscribeMoveToEvents();
        }

        protected void OnMoveToTargetComplete(object sender, EventArgs e) {
                Debug.Log($"{GetType().Name}: player has reached the target {Target.name}");
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
