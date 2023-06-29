using System;
using System.Collections.Generic;
using System.Linq;
using KitchenObjects.Container;
using KitchenObjects.ScriptableObjects;
using UnityEngine;

namespace KitchenObjects
{
    public class PlateKitchenObject : KitchenObject {

    
        [SerializeField] private CompleteRecipeSOList recipes;
        public CompleteRecipeSOList Recipes => recipes;

        public event EventHandler OnValidIngredientsChanged;

        private List<CompleteRecipeSO> validCompleteRecipes = new();
        private readonly HashSet<KitchenObjectSO> validIngredients = new();

        public IReadOnlyList<CompleteRecipeSO> ValidCompleteRecipes => validCompleteRecipes.AsReadOnly();
        public IReadOnlyCollection<KitchenObjectSO> ValidIngredients => validIngredients;

        private KitchenObjectsContainer ingredientsContainer;

        public KitchenObjectsContainer IngredientsContainer => ingredientsContainer;


        private void Awake() {
            ingredientsContainer = GetComponent<KitchenObjectsContainer>();
            ingredientsContainer.OnKitchenObjectsChanged += OnKitchenObjectsChanged;

        }

        private void Start() {
            UpdateValidIngredients();
        }



        public bool TryAddIngredient(KitchenObject ingredient) {
            // can't add plate to a plates
            if (ingredient.IsSameType(this))
                return false;

            if (!validIngredients.Contains(ingredient.KitchenObjectSO))
                return false;

            return ingredient.SetContainer(ingredientsContainer);
        }

        public bool RemoveIngredient(KitchenObject ingredient) {
            return ingredientsContainer.Remove(ingredient);
        }


        public override bool InteractWith(KitchenObject other) {
            return TryAddIngredient(other);
        }

        private void OnKitchenObjectsChanged(object sender, KitchenObjectsChangedEvent e) {
            UpdateValidIngredients();
        }

        private void UpdateValidIngredients() {
            List<KitchenObjectSO> ingredientsSO = new();
            foreach (var ingredient in ingredientsContainer.KitchenObjects) {
                ingredientsSO.Add(ingredient.KitchenObjectSO);
            }

            validCompleteRecipes = recipes.RecipeSOList.Where(finalPlate => finalPlate.IsValidFor(ingredientsSO)).ToList();


            validIngredients.Clear();
            foreach (var finalPlate in validCompleteRecipes) {
                finalPlate.GetMissingIngredient(ingredientsSO).ForEach(ingredient => validIngredients.Add(ingredient));
            }

            OnValidIngredientsChanged?.Invoke(this, EventArgs.Empty);
        }

        private void OnDestroy() {
            ingredientsContainer.OnKitchenObjectsChanged -= OnKitchenObjectsChanged;
        }

    }
}
