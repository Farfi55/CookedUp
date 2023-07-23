using System;
using KitchenObjects.ScriptableObjects;

namespace ThinkEngine.Models {
    
    [Serializable]
    public class CookingRecipe {
        
        public string Name;
        public string InputKOName;
        public string OutputKOName;
        
        public int TimeToCook;
        
        public bool IsBurningRecipe;
        
        public CookingRecipe(CookingRecipeSO cookingRecipeSO) {
            Name = cookingRecipeSO.name;
            
            InputKOName = cookingRecipeSO.Input.name;
            OutputKOName = cookingRecipeSO.Output.name;
            
            TimeToCook = Converter.FloatToInt(cookingRecipeSO.TimeToCook);
            
            IsBurningRecipe = cookingRecipeSO.IsBurningRecipe;
        }
    }
}
