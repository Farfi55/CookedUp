using System.Collections.Generic;
using System.Collections.ObjectModel;
using KitchenObjects.ScriptableObjects;
using UnityEngine;

namespace ThinkEngine {
    public class RecipesMapperManager : MonoBehaviour {
        public static RecipesMapperManager Instance { get; private set; }
        
        [SerializeField] private CompleteRecipeSOList recipesList;
        [SerializeField] private List<KitchenObjectSO> kitchenObjectList;
        [SerializeField] private List<CookingRecipeSO> cookingRecipeList;
        [SerializeField] private List<CuttingRecipeSO> cuttingRecipeList;
        
        private readonly Dictionary<string, CompleteRecipeSO> completeRecipeNameMap = new();
        private readonly Dictionary<string, KitchenObjectSO> kitchenObjectNameMap = new();
        private readonly Dictionary<string, CookingRecipeSO> cookingRecipeNameMap = new();
        private readonly Dictionary<string, CuttingRecipeSO> cuttingRecipeNameMap = new();

        public ReadOnlyDictionary<string, CompleteRecipeSO> CompleteRecipeNameMap;
        public ReadOnlyDictionary<string, KitchenObjectSO> KitchenObjectNameMap;
        public ReadOnlyDictionary<string, CookingRecipeSO> CookingRecipeNameMap;
        public ReadOnlyDictionary<string, CuttingRecipeSO> CuttingRecipeNameMap;

        private void Awake() {
            if (Instance == null) {
                Instance = this;
            }
            else {
                Debug.LogError("Multiple SOManagers in scene");
                Destroy(gameObject);
            }
            
            foreach (var recipe in recipesList.RecipeSOList) 
                completeRecipeNameMap[recipe.name] = recipe;

            foreach (var kitchenObject in kitchenObjectList) 
                kitchenObjectNameMap[kitchenObject.name] = kitchenObject;
            
            foreach (var cookingRecipe in cookingRecipeList)
                cookingRecipeNameMap[cookingRecipe.name] = cookingRecipe;
            
            foreach (var cuttingRecipe in cuttingRecipeList)
                cuttingRecipeNameMap[cuttingRecipe.name] = cuttingRecipe;
            
            CompleteRecipeNameMap = new(completeRecipeNameMap);
            KitchenObjectNameMap = new(kitchenObjectNameMap);
            CookingRecipeNameMap = new(cookingRecipeNameMap);
            CuttingRecipeNameMap = new(cuttingRecipeNameMap);
        }

    }
}
