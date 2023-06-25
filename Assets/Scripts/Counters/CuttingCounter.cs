using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(KitchenObjectsContainer))]
[RequireComponent(typeof(ProgressTracker))]
public class CuttingCounter : BaseCounter, IRecipeProvider {

    public ProgressTracker ProgressTracker { get; private set; }
    public KitchenObjectsContainer Container { get; private set; }

    public CuttingRecipeSO CurrentCuttingRecipe { get; private set; }
    public BaseRecipeSO CurrentRecipe => CurrentCuttingRecipe;

    [SerializeField] private CuttingRecipeSO[] cuttingRecipes;


    public event EventHandler<ValueChangedEvent<BaseRecipeSO>> OnRecipeChanged;


    private void Awake() {
        ProgressTracker = GetComponent<ProgressTracker>();

        Container = GetComponent<KitchenObjectsContainer>();
        Container.OnKitchenObjectsChanged += OnKitchenObjectChanged;
    }

    private void Start() {
        ProgressTracker.OnProgressComplete += (sender, e) => OnCuttingCompleted();
    }


    private void OnKitchenObjectChanged(object sender, KitchenObjectsChangedEvent e) {
        ProgressTracker.ResetTotalWork();
        var oldRecipe = CurrentCuttingRecipe;

        if (e.NextKitchenObject == null) {
            CurrentCuttingRecipe = null;
        }
        else {
            CurrentCuttingRecipe = GetRecipeFor(e.NextKitchenObject.KitchenObjectSO);
            if (CurrentCuttingRecipe != null)
                ProgressTracker.SetTotalWork(CurrentCuttingRecipe.TimeToCut);
        }

        OnRecipeChanged?.Invoke(this, new ValueChangedEvent<BaseRecipeSO>(oldRecipe, CurrentCuttingRecipe));
    }


    public override void Interact(Player player) {
        if (Container.IsEmpty()) {
            if (player.HasKitchenObject() && CanCut(player.CurrentKitchenObject.KitchenObjectSO)) {
                player.CurrentKitchenObject.SetContainer(Container);
            }
        }
        else {
            if (!player.HasKitchenObject()) {
                Container.KitchenObject.SetContainer(player.Container);
            }
        }

        InvokeOnInteract(new InteractableEvent(player));
    }

    public override void InteractAlternateContinuous(Player player) {
        if (Container.IsEmpty())
            return;

        if (!CanCut()) {
            Debug.Log("Can't cut " + Container.KitchenObject.KitchenObjectSO.DisplayName);
            return;
        }

        ProgressTracker.AddWorkDone(Time.deltaTime);
        InvokeOnInteractAlternate(new InteractableEvent(player));
    }

    public void OnCuttingCompleted() {
        if (!CanCut())
            return;

        KitchenObjectSO outputKOSO = CurrentCuttingRecipe.Output;

        Container.KitchenObject.DestroySelf();
        KitchenObject.CreateInstance(outputKOSO, Container);
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
