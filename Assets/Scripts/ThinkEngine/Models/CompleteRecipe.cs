using System;
using System.Collections.Generic;
using KitchenObjects.ScriptableObjects;

namespace ThinkEngine.Models {
    
    [Serializable]
    public class CompleteRecipe {
        
        public string Name;
        public List<string> IngredientsNames;

        public int Value;
        
        public CompleteRecipe(CompleteRecipeSO cuttingRecipeSO) {
            Name = cuttingRecipeSO.name;

            IngredientsNames = cuttingRecipeSO.Ingredients.ConvertAll(ingredient => ingredient.name);
            
            Value = Converter.FloatToInt(cuttingRecipeSO.Value);
        }
    }
}
