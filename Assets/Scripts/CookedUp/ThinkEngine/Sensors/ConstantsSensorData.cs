using System.Collections.Generic;
using KitchenObjects.ScriptableObjects;
using ThinkEngine.Models;
using UnityEngine;

namespace ThinkEngine.Sensors {
    public class ConstantsSensorData : MonoBehaviour {
        [SerializeField] private CompleteRecipeSOList completeRecipesSos;
        [SerializeField] private List<CookingRecipeSO> cookingRecipesSos;
        [SerializeField] private List<CuttingRecipeSO> cuttingRecipesSos;
        [SerializeField] private List<KitchenObjectSO> kitchenObjectsSos;


        public List<CompleteRecipeASP> CompleteRecipes = new();
        public List<CookingRecipe> CookingRecipes = new();
        public List<CuttingRecipe> CuttingRecipes = new();
        public List<string> KitchenObjectsNames = new();
        
        
        private void Awake() {
            CompleteRecipes = completeRecipesSos.RecipeSOList.ConvertAll(completeRecipeSO => new CompleteRecipeASP(completeRecipeSO));
            CookingRecipes = cookingRecipesSos.ConvertAll(cookingRecipeSO => new CookingRecipe(cookingRecipeSO));
            CuttingRecipes = cuttingRecipesSos.ConvertAll(cuttingRecipeSO => new CuttingRecipe(cuttingRecipeSO));
            KitchenObjectsNames = kitchenObjectsSos.ConvertAll(kitchenObjectSO => kitchenObjectSO.name);
        }
        
    }
}
