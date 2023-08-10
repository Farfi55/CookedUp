using System;
using KitchenObjects.ScriptableObjects;
using Players;
using ThinkEngine.Models;
using ThinkEngine.Planning;
using UnityEngine;

namespace ThinkEngine.Actions {
    class PickUpIngredientAction : PickUpAction {
        protected RecipesMapperManager RecipesMapperManager;
        protected DeliveryManager DeliveryManager;

        /// <summary>
        /// The name of the ingredient to pick up
        /// </summary>
        public string IngredientName { get; set; }
        
        public string RecipeName { get; set; } = "";
        public int RecipeRequestID { get; set; } = -1;
        
        public bool RequiresCooking { get; set; } = false;
        public bool RequiresCutting { get; set; } = false;
        
        protected PlayerBot PlayerBot;
        protected RecipeRequest RecipeRequest;
        protected KitchenObjectSO Ingredient;

        protected bool RequiresAnyWork => RequiresCooking || RequiresCutting;

        public override void Init() {
            base.Init();
            RecipesMapperManager = RecipesMapperManager.Instance;
            DeliveryManager = DeliveryManager.Instance;
            PlayerBot = Player.GetComponent<PlayerBot>();
            Ingredient = RecipesMapperManager.KitchenObjectNameMap[IngredientName];
            if (RecipeName == "") {
                RecipeRequest = PlayerBot.CurrentRecipeRequest;
            }
            else {
                RecipeRequest = DeliveryManager.GetRecipeRequestFromID(RecipeRequestID); 
            }
        }


        public override State Prerequisite() {
            var state = base.Prerequisite();
            if (state != State.READY)
                return state;

            if (!PlayerBot.HasPlate) {
                Debug.LogError($"{Player.name} does not have a plate");
                return State.ABORT;
            }

            if (!PlayerBot.HasRecipeRequest || RecipeRequest == null) {
                Debug.LogError($"{Player.name} does not have a recipe");
                return State.ABORT;
            }

            var ingredients = PlayerBot.Plate.IngredientsContainer.AsKitchenObjectSOs();
            var missingIngredients = RecipeRequest.Recipe.GetMissingIngredient(ingredients);
            
            KitchenObjectSO resultingIngredient;

            if (!RequiresAnyWork) {
                resultingIngredient = Ingredient;
            }
            else {
                BaseRecipeSO recipeSO;
                if (RequiresCooking)
                    recipeSO = RecipesMapperManager.GetCookingRecipeFromInput(Ingredient);
                else if (RequiresCutting)
                    recipeSO = RecipesMapperManager.GetCuttingRecipeFromInput(Ingredient);
                else throw new ArgumentException(); // should never happen

                if (recipeSO == null) {
                    Debug.LogError($"{Player.name} cannot cook or cut {IngredientName}");
                    return State.ABORT;
                }

                resultingIngredient = recipeSO.Output;
            }
            
            if (!missingIngredients.Contains(resultingIngredient)) {
                Debug.LogError($"{Player.name} does not need {IngredientName}");
                return State.ABORT;
            }

            return State.READY;
        }
    }
}
