using KitchenObjects.ScriptableObjects;
using Players;
using ThinkEngine.Models;
using ThinkEngine.Planning;
using UnityEngine;

namespace ThinkEngine.Actions {
    class PickUpIngredientAction : PickUpAction {
        protected RecipesMapperManager RecipesMapperManager;

        public string IngredientName { get; set; }
        public string RecipeName { get; set; } = "";

        protected PlayerBot playerBot;
        protected CompleteRecipeSO recipe;
        protected KitchenObjectSO ingredient;

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

            if (!missingIngredients.Contains(ingredient)) {
                Debug.LogError($"{Player.name} does not need {IngredientName}");
                return State.ABORT;
            }

            return State.READY;
        }
    }
}
