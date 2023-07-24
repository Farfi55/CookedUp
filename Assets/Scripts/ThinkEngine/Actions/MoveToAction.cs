using System;
using Players;
using ThinkEngine.Planning;
using UnityEngine;
using Action = ThinkEngine.Planning.Action;

namespace ThinkEngine.Actions {
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

        bool error = false;

        public override State Prerequisite() {
            if (idManager == null) {
                idManager = IDManager.Instance;


                player = idManager.GetGameObject(PlayerID).GetComponent<Player>();
                playerMovement = player.GetComponent<PlayerMovement>();

                if (UseTarget()) {
                    target = idManager.GetGameObject(TargetID);
                    worldPos = target.transform.position;
                }
                else {
                    gridManager = GridManager.Instance;
                    gridPos = new Vector2Int(GridX, GridY);
                    worldPos = gridManager.GetWorldPosition(gridPos);
                }
            }

            return State.READY;
        }

        public override void Do() {
            if (UseTarget()) {
                playerMovement.LookAt(target.transform);
            }

            if (!playerMovement.TryMoveTo(worldPos)) {
                Debug.LogError($"player {player.name} could not move to {worldPos}");
                error = true;
            }
        }
        
        
        public override bool Done() {
            if (error)
                return true;

            if (UseTarget() && player.GetSelectedGameObject() != target)
                return false;
            
            return !playerMovement.IsMovingUsingNavigation;
        }
    }
}
