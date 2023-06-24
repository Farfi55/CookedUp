using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(KitchenObjectHolder))]
[RequireComponent(typeof(ProgressTracker))]
public class CuttingCounter : BaseCounter, IRecipeProvider {

    public ProgressTracker ProgressTracker { get; private set; }
    public KitchenObjectHolder Holder { get; private set; }

    public CuttingRecipeSO CurrentCuttingRecipe { get; private set; }
    public BaseRecipeSO CurrentRecipe => CurrentCuttingRecipe;

    [SerializeField] private CuttingRecipeSO[] cuttingRecipes;


    public event EventHandler<ValueChangedEvent<BaseRecipeSO>> OnRecipeChanged;


    private void Awake() {
        ProgressTracker = GetComponent<ProgressTracker>();

        Holder = GetComponent<KitchenObjectHolder>();
        Holder.OnKitchenObjectChanged += OnKitchenObjectChanged;
    }

    private void Start() {
        ProgressTracker.OnProgressComplete += (sender, e) => OnCuttingCompleted();
    }


    private void OnKitchenObjectChanged(object sender, KitchenObjectChangedEvent e) {
        ProgressTracker.ResetTotalWork();

        if (e.NewKitchenObject == null) {
            CurrentCuttingRecipe = null;
        }
        else {
            CurrentCuttingRecipe = GetRecipeFor(e.NewKitchenObject.KitchenObjectSO);
            if (CurrentCuttingRecipe != null)
                ProgressTracker.SetTotalWork(CurrentCuttingRecipe.TimeToCut);
        }

        var oldRecipe = GetRecipeFor(e.OldKitchenObject?.KitchenObjectSO);
        OnRecipeChanged?.Invoke(this, new ValueChangedEvent<BaseRecipeSO>(oldRecipe, CurrentCuttingRecipe));
    }


    public override void Interact(Player player) {
        if (Holder.IsEmpty()) {
            if (player.HasKitchenObject() && CanCut(player.CurrentKitchenObject.KitchenObjectSO)) {
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

    public override void InteractAlternateContinuous(Player player) {
        if (!Holder.HasKitchenObject())
            return;

        if (!CanCut()) {
            Debug.Log("Can't cut " + Holder.KitchenObject.KitchenObjectSO.Name);
            return;
        }

        ProgressTracker.AddWorkDone(Time.deltaTime);
        InvokeOnInteractAlternate(new InteractableEvent(player));
    }

    public void OnCuttingCompleted() {
        if (!CanCut())
            return;

        KitchenObjectSO outputKOSO = CurrentCuttingRecipe.Output;

        Holder.KitchenObject.DestroySelf();
        KitchenObject.CreateInstance(outputKOSO, Holder);
    }


    public CuttingRecipeSO GetRecipeFor(KitchenObjectSO kitchenObjectSO) {
        return cuttingRecipes.FirstOrDefault(recipe => recipe.Input == kitchenObjectSO);
    }

    public bool CanCut() => CurrentCuttingRecipe != null;
    public bool CanCut(KitchenObjectSO kitchenObjectSO) => GetRecipeFor(kitchenObjectSO) != null;

    public bool TryGetRecipeFor(KitchenObjectSO kitchenObjectSO, out CuttingRecipeSO recipe) {
        recipe = GetRecipeFor(kitchenObjectSO);
        return recipe != null;
    }
}
