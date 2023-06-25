using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateKitchenObject : KitchenObject {

    [SerializeField] private List<FinalPlateSO> finalPlates = new();
    public List<FinalPlateSO> FinalPlates => finalPlates;

    public event EventHandler OnValidIngredientsChanged;

    private List<FinalPlateSO> validFinalPlates = new();
    private HashSet<KitchenObjectSO> validIngredients = new();

    public IReadOnlyList<FinalPlateSO> ValidFinalPlates => validFinalPlates.AsReadOnly();
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

        validFinalPlates = finalPlates.Where(finalPlate => finalPlate.IsValidFor(ingredientsSO)).ToList();


        validIngredients.Clear();
        foreach (var finalPlate in validFinalPlates) {
            finalPlate.GetMissingIngredient(ingredientsSO).ForEach(ingredient => validIngredients.Add(ingredient));
        }

        OnValidIngredientsChanged?.Invoke(this, EventArgs.Empty);
    }

    private void OnDestroy() {
        ingredientsContainer.OnKitchenObjectsChanged -= OnKitchenObjectsChanged;
    }

}
