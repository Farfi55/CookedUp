using Players;
using ThinkEngine.Planning;
using UnityEngine;

namespace ThinkEngine.Actions
{
    class SetRecipeRequestAction : Action, IPlayerAction
    {
        private IDManager idManager;
        private DeliveryManager deliveryManager;
        
        public int PlayerID { get; set; }
        
        public int RecipeRequestID { get; set; }

        private Player player;
        private PlayerBot playerBot;
        
        private RecipeRequest recipeRequest;


        public override void Init() {
            
            idManager = IDManager.Instance;
            deliveryManager =  DeliveryManager.Instance;
            recipeRequest = deliveryManager.GetRecipeRequestFromID(RecipeRequestID);
            
            player = idManager.GetComponentFromID<Player>(PlayerID);
            playerBot = player.GetComponent<PlayerBot>();

            // todo: remove this debug code
            var playerBots = FindObjectsByType<PlayerBot>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            foreach (var bot in playerBots) {
                if (bot.HasRecipeRequest && bot.CurrentRecipeRequest.ID == RecipeRequestID) {
                    if (bot != playerBot) {
                        Debug.LogWarning($"SetRecipeRequestAction: Recipe request {recipeRequest.Recipe.name} is already assigned to {bot.name}");
                    }
                    else {
                        Debug.Log($"SetRecipeRequestAction: {player.name} already has recipe request {recipeRequest.Recipe.name}");
                    }
                }
            }
        }

        public override State Prerequisite() {
            return State.READY;
        }

        public override void Do() {
            
            playerBot.SetRecipeRequest(recipeRequest);
            
            if(playerBot.CurrentRecipeRequest != recipeRequest)
                Debug.Log($"SetRecipeRequestAction: {player.name} failed to set recipe request to {recipeRequest.Recipe.name}");
        }

        public override State Done() {
            return State.READY;
        }
    }
}
