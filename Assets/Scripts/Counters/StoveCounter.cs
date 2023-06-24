using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(KitchenObjectHolder))]
public class StoveCounter : BaseCounter {

    [SerializeField] private CookingRecipeSO[] cookingRecipes;
    public CookingRecipeSO CurrentRecipe { get; private set; }

    public KitchenObjectHolder Holder => holder;
    private KitchenObjectHolder holder;

    public event EventHandler OnCooked;
    public event EventHandler OnCookProgressChanged;

    public float RemainingTimeToCook { get; private set; }


    private void Awake() {
        holder = GetComponent<KitchenObjectHolder>();
        holder.OnKitchenObjectChanged += Holder_OnKitchenObjectChanged;
    }

    private void Holder_OnKitchenObjectChanged(object sender, KitchenObjectChangedEvent e) {
        // Set the current recipe and remaining time to cook when the holder's kitchen object changes.
        RemainingTimeToCook = 0f;
        if (e.NewKitchenObject == null) {
            CurrentRecipe = null;
        }
        else {
            CurrentRecipe = RecipeFor(e.NewKitchenObject.KitchenObjectSO);
            if (CurrentRecipe != null) {
                RemainingTimeToCook = CurrentRecipe.TimeToCook;
            }
        }
        OnCookProgressChanged?.Invoke(this, EventArgs.Empty);
    }

    private void Update() {
        if (!CanCook())
            return;

        RemainingTimeToCook -= Time.deltaTime;
        if (RemainingTimeToCook <= 0f) {
            OnCooked?.Invoke(this, EventArgs.Empty);
            holder.KitchenObject.DestroySelf();
        }
    }


    private CookingRecipeSO RecipeFor(KitchenObjectSO koso) {
        return cookingRecipes.FirstOrDefault(x => x.Input == koso);
    }

    private bool CanCook(KitchenObjectSO koso) {
        return RecipeFor(koso) != null;
    }

    public bool CanCook() {
        return CurrentRecipe != null;
    }
    public bool IsCooking() => CanCook();

}
