using Counters;
using KitchenObjects.ScriptableObjects;
using ThinkEngine.Planning;
using UnityEngine;

namespace ThinkEngine.Actions
{
    public class CutAction : InteractAction
    {
        
        private CuttingCounter cuttingCounter;
        private CuttingRecipeSO cuttingRecipe;

        private bool hasCutSuccessfully;
        private bool isDone;
        
        protected override void Setup() {
            base.Setup();
            cuttingCounter = Target.GetComponent<CuttingCounter>();
            if (cuttingCounter == null) {
                Debug.LogError($"Target {Target.name} does not have a CuttingCounter component!", cuttingCounter);
                AnyError = true;
            }
            
            cuttingRecipe = cuttingCounter.CurrentCuttingRecipe;
        }

        public override State Prerequisite() {
            var state = base.Prerequisite();
            if (state != State.READY)
                return state;

            if (!cuttingCounter.CanCut()) {
                Debug.LogError($"{Player.name} cannot cut on {Target.name}", cuttingCounter);
                return State.ABORT;
            }
            return State.READY;
        }

        public override void Do() {
            cuttingCounter.OnRecipeChanged += OnRecipeChanged;
            Player.StartAlternateInteract();
        }

        private void OnRecipeChanged(object sender, ValueChangedEvent<BaseRecipeSO> e) {
            isDone = true;
            
            // if the recipe has changed and the result is what we expected as output of the recipe
            // then we are successful
            var outputKO = cuttingCounter.Container.KitchenObject;
            hasCutSuccessfully = outputKO != null && outputKO.IsSameType(cuttingRecipe.Output);
        }

        public override State Done() {
            if (AnyError)
                return State.ABORT;
            
            if (isDone) {
                OnDone();
                return hasCutSuccessfully ? State.READY : State.ABORT;
            }

            return State.WAIT;
        }
        
        
        private void OnDone() {
            cuttingCounter.OnRecipeChanged -= OnRecipeChanged;
            
            Player.StopAlternateInteract();
        }
    }
}
