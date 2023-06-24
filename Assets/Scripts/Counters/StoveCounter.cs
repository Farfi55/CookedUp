using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(KitchenObjectHolder))]
[RequireComponent(typeof(ProgressTracker))]
public class StoveCounter : BaseCounter, IRecipeProvider {
    public ProgressTracker ProgressTracker { get; private set; }
    public KitchenObjectHolder Holder { get; private set; }


    public CookingRecipeSO CurrentCookingRecipe { get; private set; }
    public BaseRecipeSO CurrentRecipe => CurrentCookingRecipe;
    [SerializeField] private CookingRecipeSO[] cookingRecipes;


    public event EventHandler<ValueChangedEvent<BaseRecipeSO>> OnRecipeChanged;


    private void Awake() {
        ProgressTracker = GetComponent<ProgressTracker>();
        Holder = GetComponent<KitchenObjectHolder>();
        Holder.OnKitchenObjectChanged += OnKitchenObjectChanged;
    }

    private void Start() {
        ProgressTracker.OnProgressComplete += (sender, e) => OnCookingCompleted();
    }


    private void OnKitchenObjectChanged(object sender, KitchenObjectChangedEvent e) {
        // Set the current recipe and remaining time to cook when the holder's kitchen object changes.

        ProgressTracker.ResetTotalWork();
        if (e.NewKitchenObject == null) {
            CurrentCookingRecipe = null;
        }
        else {
            CurrentCookingRecipe = GetRecipeFor(e.NewKitchenObject.KitchenObjectSO);
            if (CurrentCookingRecipe != null) {
                ProgressTracker.SetTotalWork(CurrentCookingRecipe.TimeToCook);
            }
        }

        var oldRecipe = GetRecipeFor(e.OldKitchenObject?.KitchenObjectSO);
        OnRecipeChanged?.Invoke(this, new(oldRecipe, CurrentCookingRecipe));
    }

    public override void Interact(Player player) {
        if (Holder.IsEmpty()) {
            if (player.HasKitchenObject() && CanCook(player.CurrentKitchenObject.KitchenObjectSO)) {
                player.CurrentKitchenObject.SetHolder(Holder);
            }
        }
        else {
            if (!player.HasKitchenObject()) {
                Holder.KitchenObject.SetHolder(player.Holder);
            }
        }

        InvokeOnInteract(new InteractableEvent(player));
    }

    private void Update() {
        if (!CanCook())
            return;

        ProgressTracker.AddWorkDone(Time.deltaTime);
    }

    private void OnCookingCompleted() {
        if (!CanCook())
            return;

        var outputKOSO = CurrentCookingRecipe.Output;
        Holder.KitchenObject.DestroySelf();
        KitchenObject.CreateInstance(outputKOSO, Holder);
    }


    public CookingRecipeSO GetRecipeFor(KitchenObjectSO kitchenObjectSO) {
        return cookingRecipes.FirstOrDefault(x => x.Input == kitchenObjectSO);
    }

    public bool TryGetRecipeFor(KitchenObjectSO kitchenObjectSO, out CookingRecipeSO recipe) {
        recipe = GetRecipeFor(kitchenObjectSO);
        return recipe != null;
    }

    private bool CanCook(KitchenObjectSO kitchenObjectSO) {
        return GetRecipeFor(kitchenObjectSO) != null;
    }

    public bool CanCook() {
        return CurrentCookingRecipe != null;
    }
    public bool IsCooking() => CanCook();



}
