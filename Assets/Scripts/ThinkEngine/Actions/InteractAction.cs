using Players;
using ThinkEngine.Planning;
using UnityEngine;

namespace ThinkEngine.Actions {
    public abstract class InteractAction : Action, IPlayerAction {
        public int PlayerID { get; set; }
        public int TargetID { get; set; }

        protected PlayersManager PlayersManager;
        protected IDManager IDManager;

        public Player Player { get; private set; }

        protected GameObject Target => target;
        protected IInteractable Interactable => interactable;

        private GameObject target;
        private IInteractable interactable;
        
        protected bool AnyError;

        protected virtual void Setup() {
            IDManager = IDManager.Instance;

            if (PlayerID == 0) {
                Player = PlayersManager.Instance.GetPlayer();
            }
            else
                Player = IDManager.GetComponentFromID<Player>(PlayerID);

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

            if (Player.GetSelectedGameObject() != target) {
                Debug.Log($"player is not looking at the target anymore");
                return State.WAIT;
            }

            return State.READY;
        }
    }
}
