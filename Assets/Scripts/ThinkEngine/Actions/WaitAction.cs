using Players;
using ThinkEngine.Planning;
using UnityEngine;

namespace ThinkEngine.Actions
{
    class WaitAction : Action, IPlayerAction
    {
        private PlayersManager playersManager;
        private IDManager idManager;

        
        public int PlayerID { get; set; }
        
        /// Time to wait in milliseconds
        public int TimeToWait { get; set; }

        
        public Player Player { get; private set; }
        
        private float timeToWaitFloat;
        private float timeElapsed;

        public override State Prerequisite() {
            if (idManager == null) {
                idManager = IDManager.Instance;

                if (PlayerID == 0) {
                    Player = PlayersManager.Instance.GetPlayer();
                }
                else
                    Player = idManager.GetComponentFromID<Player>(PlayerID);
            }

            timeToWaitFloat = TimeToWait / 1000f;
            timeElapsed = 0;

            return State.READY;
        }

        public override void Do() { }

        public override bool Done() {
            timeElapsed += Time.deltaTime;
            
            return timeElapsed >= timeToWaitFloat;
        }
    }
}
