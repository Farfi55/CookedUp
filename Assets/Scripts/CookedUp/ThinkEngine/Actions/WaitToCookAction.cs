using CookedUp.Core.Counters;
using CookedUp.Core.KitchenObjects;
using Shared;
using ThinkEngine.Planning;
using UnityEngine;

namespace CookedUp.ThinkEngine.Actions
{
    public class WaitToCookAction : InteractAction
    {
        private RecipesMapperManager recipesMapperManager;
        
        private StoveCounter stoveCounter;
        private CookingRecipeSO cookingRecipe;

        public string FinalIngredient { get; set; }
        
        private bool hasCookedSuccessfully;
        private bool isDone;
        
        public override void Init() {
            base.Init();
            recipesMapperManager = RecipesMapperManager.Instance;
            
            stoveCounter = Target.GetComponent<StoveCounter>();
            if (stoveCounter == null) {
                Debug.LogWarning($"[{GetType().Name}]: Target {Target.name} does not have a StoveCounter component!", stoveCounter);
                AnyError = true;
                return;
            }
            
            cookingRecipe = stoveCounter.CurrentCookingRecipe;

            if(cookingRecipe == null) {
                Debug.LogWarning($"[{GetType().Name}]: Target {Target.name} does not have a cooking recipe!", stoveCounter);
                AnyError = true;
                return;
            }

            if (cookingRecipe.Output.name != FinalIngredient) {
                Debug.LogWarning($"[{GetType().Name}]: Target {Target.name} does not have the expected output {FinalIngredient}!", stoveCounter);
                AnyError = true;
                return;
            }

            var koPlayer = stoveCounter.Container.KitchenObject.GetComponent<KitchenObjectPlayer>();
            if(koPlayer.Player != Player) {
                Debug.LogWarning($"[{GetType().Name}]: Target {Target.name} is not being used by {Player.name}!", stoveCounter);
                AnyError = true;
                return;
            }
            
            Debug.Log($"[{GetType().Name}]: {Player.name} waiting to cook {cookingRecipe.name}");
        }


        public override State Prerequisite() {
            var state = base.Prerequisite();
            if (state != State.READY)
                return state;

            if (!stoveCounter.CanCook()) {
                Debug.LogWarning($"[{GetType().Name}]: {Player.name} cannot cook on {Target.name}", stoveCounter);
                return State.ABORT;
            }
            return State.READY;
        }

        public override void Do() {
            stoveCounter.OnRecipeChanged += OnRecipeChanged;
        }

        private void OnRecipeChanged(object sender, ValueChangedEvent<BaseRecipeSO> e) {
            var newKO = stoveCounter.Container.KitchenObject;
            Debug.Log($"[{GetType().Name}]: {Player.name} recipe changed to {e.NewValue?.name ?? "null"}");
            if(newKO != null && newKO.IsSameType(cookingRecipe.Input))
                return;
            
            isDone = true;
            
            // if the recipe has changed and the result is what we expected as output of the recipe
            // then we are successful
            hasCookedSuccessfully = newKO != null && newKO.IsSameType(cookingRecipe.Output);
        }

        public override State Done() {
            if (AnyError)
                return State.ABORT;
            
            if (isDone)
                return hasCookedSuccessfully ? State.READY : State.ABORT;

            return State.WAIT;
        }

        public override void Clean() {
            base.Clean();
            stoveCounter.OnRecipeChanged -= OnRecipeChanged;
        }
    }
}
