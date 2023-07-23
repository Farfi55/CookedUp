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


        [HideInInspector] public List<CompleteRecipe> CompleteRecipes;
        [HideInInspector] public List<CookingRecipe> CookingRecipes;
        [HideInInspector] public List<CuttingRecipe> CuttingRecipes;
        [HideInInspector] public List<string> KitchenObjectsNames;
        
        
        private void Awake() {
            CompleteRecipes = completeRecipesSos.RecipeSOList.ConvertAll(completeRecipeSO => new CompleteRecipe(completeRecipeSO));
            CookingRecipes = cookingRecipesSos.ConvertAll(cookingRecipeSO => new CookingRecipe(cookingRecipeSO));
            CuttingRecipes = cuttingRecipesSos.ConvertAll(cuttingRecipeSO => new CuttingRecipe(cuttingRecipeSO));
            KitchenObjectsNames = kitchenObjectsSos.ConvertAll(kitchenObjectSO => kitchenObjectSO.name);
        }
        
    }
}
