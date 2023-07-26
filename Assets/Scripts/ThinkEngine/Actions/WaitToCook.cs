using Counters;
using KitchenObjects.ScriptableObjects;
using ThinkEngine.Planning;
using UnityEngine;

namespace ThinkEngine.Actions
{
    public class WaitToCookAction : InteractAction
    {
        
        private StoveCounter stoveCounter;
        private CookingRecipeSO cookingRecipe;

        private bool hasCookedSuccessfully;
        private bool isDone;
        
        protected override void Setup() {
            base.Setup();
            stoveCounter = Target.GetComponent<StoveCounter>();
            if (stoveCounter == null) {
                Debug.LogError($"Target {Target.name} does not have a StoveCounter component!", stoveCounter);
                AnyError = true;
            }
            
            cookingRecipe = stoveCounter.CurrentCookingRecipe;
        }

        public override State Prerequisite() {
            var state = base.Prerequisite();
            if (state != State.READY)
                return state;

            if (!stoveCounter.CanCook()) {
                Debug.LogError($"{Player.name} cannot cook on {Target.name}", stoveCounter);
                return State.ABORT;
            }
            return State.READY;
        }

        public override void Do() {
            stoveCounter.OnRecipeChanged += OnRecipeChanged;
        }

        private void OnRecipeChanged(object sender, ValueChangedEvent<BaseRecipeSO> e) {
            isDone = true;
            
            // if the recipe has changed and the result is what we expected as output of the recipe
            // then we are successful
            var outputKO = stoveCounter.Container.KitchenObject;
            hasCookedSuccessfully = outputKO != null && outputKO.IsSameType(cookingRecipe.Output);
        }

        public override bool Done() {
            if (AnyError)
                return true;
            
            if (isDone) {
                OnDone();
                return true;
            }

            return false;
        }
        
        
        private void OnDone() {
            stoveCounter.OnRecipeChanged -= OnRecipeChanged;
            
        }
    }
}
