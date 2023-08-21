using Players;
using ThinkEngine.Planning;
using UnityEngine;

namespace ThinkEngine.Actions
{
    class WaitAction : Action, IPlayerAction
    {
        private PlayersManager playersManager;
        private IDManager idManager;

        private Player player;
        
        public int PlayerID { get; set; }
        
        /// Time to wait in milliseconds
        public int TimeToWait { get; set; }


        
        private float timeToWaitFloat;
        private float timeElapsed;

        public override void Init() {
            idManager = IDManager.Instance;
            
            if (PlayerID == 0) {
                player = PlayersManager.Instance.GetPlayer();
            }
            else
                player = idManager.GetComponentFromID<Player>(PlayerID);
            
            timeToWaitFloat = TimeToWait / 1000f;
            timeElapsed = 0;
        }

        public override State Prerequisite() {
            return State.READY;
        }

        public override void Do() { }

        public override State Done() {
            timeElapsed += Time.deltaTime;
            
            return timeElapsed >= timeToWaitFloat ? State.READY : State.WAIT;
        }
    }
}
