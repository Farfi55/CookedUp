using System;

namespace ThinkEngine.Models
{
    [Serializable]
    public class RecipeRequestASP : CompleteRecipeASP {
        public int ID;
        public int TimeToComplete;
        public int RemainingTimeToComplete;
        
        public RecipeRequestASP (RecipeRequest recipeRequest) : base(recipeRequest.Recipe) {
            ID = recipeRequest.ID;
            TimeToComplete = Converter.FloatToInt(recipeRequest.TimeToComplete);
            RemainingTimeToComplete = Converter.FloatToInt(recipeRequest.RemainingTimeToComplete);
        }
        
        
    }
}
