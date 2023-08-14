using System;
using System.Collections.Generic;
using KitchenObjects.ScriptableObjects;

namespace ThinkEngine.Models {
    [Serializable]
    public class CompleteRecipeASP {

        public string Name = String.Empty;

        public List<string> IngredientsNames = new();

        public int Value;

        public CompleteRecipeASP() {
        }

        public CompleteRecipeASP(CompleteRecipeSO cuttingRecipeSO) {
            Name = cuttingRecipeSO.name;

            IngredientsNames = cuttingRecipeSO.Ingredients.ConvertAll(ingredient => ingredient.name);

            Value = Converter.FloatToInt(cuttingRecipeSO.Value);
        }
    }
}
