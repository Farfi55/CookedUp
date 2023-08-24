using CookedUp.Core;
using CookedUp.Core.KitchenObjects;
using CookedUp.Core.Players;
using ThinkEngine.Planning;
using UnityEngine;

namespace CookedUp.ThinkEngine.Actions
{
    class SetPlateAction : Action, IPlayerAction
    {
        private IDManager idManager;
        
        public int PlayerID { get; set; }
        
        public int PlateID { get; set; }

        private Player player;
        private PlayerBot playerBot;
        
        public PlateKitchenObject plate;

        public override void Init() {

            
            idManager = IDManager.Instance;
            
            
            player = idManager.GetComponentFromID<Player>(PlayerID);
            playerBot = player.GetComponent<PlayerBot>();

            plate = PlateID == 0 ? null : idManager.GetComponentFromID<PlateKitchenObject>(PlateID);
        }

        public override State Prerequisite() {
            return State.READY;
        }

        public override void Do() {
            
            playerBot.SetPlate(plate);
            
            if(playerBot.Plate != plate)
                Debug.Log($"[{GetType().Name}]: {player.name} failed to set plate to {PlateID}");
        }

        public override State Done() {
            return State.READY;
        }

        public override void Clean() {
            Debug.Log($"[{GetType().Name}]: Clean, PlayerID: {PlayerID}, PlateID: {PlateID}");
        }
    }
}
