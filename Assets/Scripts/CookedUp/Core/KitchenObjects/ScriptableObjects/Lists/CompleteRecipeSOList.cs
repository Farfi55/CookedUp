using System.Collections.Generic;
using UnityEngine;

namespace KitchenObjects.ScriptableObjects {
    [CreateAssetMenu(fileName = "new FinalRecipeSOList", menuName = "CookedUp/FinalRecipeSOList", order = 100)]
    public class CompleteRecipeSOList : ScriptableObject {
        [SerializeField] private List<CompleteRecipeSO> recipeSOList;
        public List<CompleteRecipeSO> RecipeSOList => recipeSOList;
    }
}
