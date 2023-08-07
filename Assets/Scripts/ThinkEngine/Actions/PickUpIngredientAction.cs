using System;
using KitchenObjects.ScriptableObjects;
using Players;
using ThinkEngine.Models;
using ThinkEngine.Planning;
using UnityEngine;

namespace ThinkEngine.Actions {
    class PickUpIngredientAction : PickUpAction {
        protected RecipesMapperManager RecipesMapperManager;

        /// <summary>
        /// The name of the ingredient to pick up
        /// </summary>
        public string IngredientName { get; set; }
        
        public string RecipeName { get; set; } = "";
        
        public bool RequiresCooking { get; set; } = false;
        public bool RequiresCutting { get; set; } = false;
        
        protected PlayerBot playerBot;
        protected CompleteRecipeSO recipe;
        protected KitchenObjectSO ingredient;

        protected bool RequiresAnyWork => RequiresCooking || RequiresCutting;

        protected override void Setup() {
            base.Setup();
            RecipesMapperManager = RecipesMapperManager.Instance;
            playerBot = Player.GetComponent<PlayerBot>();
            ingredient = RecipesMapperManager.KitchenObjectNameMap[IngredientName];
            if (RecipeName == "") {
                recipe = playerBot.CurrentRecipe;
            }
            else {
                recipe = RecipesMapperManager.CompleteRecipeNameMap[RecipeName];
            }
        }

        public override State Prerequisite() {
            var state = base.Prerequisite();
            if (state != State.READY)
                return state;

            if (!playerBot.HasPlate) {
                Debug.LogError($"{Player.name} does not have a plate");
                return State.ABORT;
            }

            if (!playerBot.HasRecipe || recipe == null) {
                Debug.LogError($"{Player.name} does not have a recipe");
                return State.ABORT;
            }

            var ingredients = playerBot.Plate.IngredientsContainer.AsKitchenObjectSOs();
            var missingIngredients = recipe.GetMissingIngredient(ingredients);
            
            KitchenObjectSO resultingIngredient;

            if (!RequiresAnyWork) {
                resultingIngredient = ingredient;
            }
            else {
                BaseRecipeSO recipeSO;
                if (RequiresCooking)
                    recipeSO = RecipesMapperManager.GetCookingRecipeFromInput(ingredient);
                else if (RequiresCutting)
                    recipeSO = RecipesMapperManager.GetCuttingRecipeFromInput(ingredient);
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
