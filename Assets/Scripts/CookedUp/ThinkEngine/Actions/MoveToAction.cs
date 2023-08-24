using CookedUp.Core;
using CookedUp.Core.Players;
using ThinkEngine.Planning;
using UnityEngine;
using Action = ThinkEngine.Planning.Action;

namespace CookedUp.ThinkEngine.Actions {
    public class MoveToAction : Action, IPlayerAction {
        public int PlayerID { get; set; }
        public int TargetID { get; set; }
        public int GridX { get; set; }
        public int GridY { get; set; }


        private GridManager gridManager;
        private PlayersManager playersManager;
        private IDManager idManager;

        private Player player;
        private PlayerMovement playerMovement;
        private GameObject target;
        private Vector2Int gridPos;

        private Vector3 worldPos;

        private bool UseTarget() => TargetID != 0;

        bool anyError;


        public override void Init() {
            idManager = IDManager.Instance;
                
            if (PlayerID == 0) {
                player = PlayersManager.Instance.GetPlayer();
            }
            else
                player = idManager.GetComponentFromID<Player>(PlayerID);

            playerMovement = player.GetComponent<PlayerMovement>();

            if (UseTarget()) {
                target = idManager.GetGameObject(TargetID);
                worldPos = target.transform.position;
                    
                Debug.Log($"[{GetType().Name}]: {player.name} moving to {target.name}");
            }
            else {
                gridManager = GridManager.Instance;
                gridPos = new Vector2Int(GridX, GridY);
                worldPos = gridManager.GetWorldPosition(gridPos);
            }
        }

        public override State Prerequisite() {
            return State.READY;
        }

        public override void Do() {
            if (UseTarget()) {
                playerMovement.LookAt(target.transform);
            }

            if (!playerMovement.TryMoveTo(worldPos)) {
                Debug.LogError($"[{GetType().Name}]: {player.name} could not move to {worldPos}");
                anyError = true;
            }
        }


        public override State Done() {
            if (anyError)
                return State.ABORT;

            if (UseTarget() && player.GetSelectedGameObject() != target)
                return State.WAIT;

            return playerMovement.IsMovingUsingNavigation ? State.WAIT : State.READY;
        }
    }
}
