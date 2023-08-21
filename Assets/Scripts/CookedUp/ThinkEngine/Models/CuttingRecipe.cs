using System;
using CookedUp.Core.KitchenObjects;

namespace CookedUp.ThinkEngine.Models {
    
    [Serializable]
    public class CuttingRecipe {
        
        public string Name;
        public string InputKOName;
        public string OutputKOName;
        
        public int TimeToCut;
        
        public CuttingRecipe(CuttingRecipeSO cuttingRecipeSO) {
            Name = cuttingRecipeSO.name;
            
            InputKOName = cuttingRecipeSO.Input.name;
            OutputKOName = cuttingRecipeSO.Output.name;
            
            TimeToCut = Converter.FloatToInt(cuttingRecipeSO.TimeToCut);
        }
    }
}
