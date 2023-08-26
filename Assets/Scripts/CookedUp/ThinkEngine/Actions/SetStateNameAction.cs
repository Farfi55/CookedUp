using CookedUp.Core;
using CookedUp.Core.KitchenObjects;
using CookedUp.Core.Players;
using ThinkEngine.Planning;
using UnityEngine;

namespace CookedUp.ThinkEngine.Actions
{
    class SetStateNameAction : Action, IPlayerAction
    {
        private IDManager idManager;
        
        public int PlayerID { get; set; }
        
        public string StateName { get; set; }
        
        private Player player;
        private PlayerBot playerBot;

        public override void Init() {
            idManager = IDManager.Instance;
            
            player = idManager.GetComponentFromID<Player>(PlayerID);
            playerBot = player.GetComponent<PlayerBot>();
        }

        public override State Prerequisite() {
            return State.READY;
        }

        public override void Do() {
            playerBot.SetStateName(StateName);
        }

        public override State Done() {
            return State.READY;
        }

        public override void Clean() {
        }
    }
}
