using System.Collections.Generic;
using Shared.Extensions;
using UnityEngine;
using UnityEngine.Serialization;

namespace CookedUp.Core.KitchenObjects
{
    [CreateAssetMenu(fileName = "new CompleteRecipeSO", menuName = "CookedUp/Complete Recipe", order = 3)]
    public class CompleteRecipeSO : ScriptableObject {

        [SerializeField] private string displayName;
        [SerializeField] private List<KitchenObjectSO> ingredients = new();
        [SerializeField, Range(0, 150)] private int value = 10;

        [FormerlySerializedAs("finalPlateArrangmentPrefab")] [SerializeField] private RecipeArrangement recipeArrangementPrefab;


        public string DisplayName => displayName;
        public List<KitchenObjectSO> Ingredients => ingredients;
        public float Value => value;

        public RecipeArrangement RecipeArrangementPrefab => recipeArrangementPrefab;



        public bool IsValidFor(IEnumerable<KitchenObjectSO> otherIngredients) {
            foreach (var otherIngredient in otherIngredients) {
                if (!ingredients.Contains(otherIngredient))
                    return false;
            }

            return true;
        }

        public List<KitchenObjectSO> GetMissingIngredient(List<KitchenObjectSO> otherIngredients) {
            var missingIngredients = new List<KitchenObjectSO>();
            foreach (var ingredient in ingredients) {
                if (!otherIngredients.Contains(ingredient))
                    missingIngredients.Add(ingredient);
            }

            return missingIngredients;
        }
    
        public bool MatchesCompletely(List<KitchenObjectSO> otherIngredients) {
            if(otherIngredients.Count != ingredients.Count)
                return false;
        
            foreach (var ingredient in ingredients) {
                if (!otherIngredients.Contains(ingredient))
                    return false;
            }

            return true;
        }


#if UNITY_EDITOR
        [ContextMenu("load from file name")]
        private void LoadFromName() {
            displayName = name.CamelCaseToSentence();

            string path = $"Assets/Prefabs/FinalPlateArrangements/{name}Arrangement.prefab";
            recipeArrangementPrefab = UnityEditor.AssetDatabase.LoadAssetAtPath<RecipeArrangement>(path);
            if (recipeArrangementPrefab == null) {
                Debug.LogError($"Couldn't find prefab for {displayName} at {path}", this);
            }


            UnityEditor.EditorUtility.SetDirty(this);
        }
#endif

    }
}
