using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(KitchenObjectsContainer))]
[RequireComponent(typeof(ProgressTracker))]
public class StoveCounter : BaseCounter, IRecipeProvider {
    public ProgressTracker ProgressTracker { get; private set; }
    public KitchenObjectsContainer Container { get; private set; }


    public CookingRecipeSO CurrentCookingRecipe { get; private set; }
    public BaseRecipeSO CurrentRecipe => CurrentCookingRecipe;
    [SerializeField] private CookingRecipeSO[] cookingRecipes;

    [SerializeField, Range(0f, 10f), Tooltip("Work done per second, default is 1.")]
    private float cookingSpeed = 1f;

    [SerializeField, Range(0f, 10f), Tooltip("Work done per second, default is 1.")]
    private float burningCookingSpeed = 1f;

    public bool IsBurningRecipe => IsCooking() && CurrentCookingRecipe.IsBurningRecipe;

    public event EventHandler<ValueChangedEvent<BaseRecipeSO>> OnRecipeChanged;


    private void Awake() {
        ProgressTracker = GetComponent<ProgressTracker>();
        Container = GetComponent<KitchenObjectsContainer>();
        Container.OnKitchenObjectsChanged += OnKitchenObjectsChanged;
    }

    private void Start() {
        ProgressTracker.OnProgressComplete += (sender, e) => OnCookingCompleted();
    }


    private void OnKitchenObjectsChanged(object sender, KitchenObjectsChangedEvent e) {
        // Set the current recipe and remaining time to cook when the holder's kitchen object changes.
        var oldRecipe = CurrentCookingRecipe;

        ProgressTracker.ResetTotalWork();

        if (e.NextKitchenObject == null) {
            CurrentCookingRecipe = null;
        }
        else {
            CurrentCookingRecipe = GetRecipeFor(e.NextKitchenObject.KitchenObjectSO);
            if (CurrentCookingRecipe != null) {
                ProgressTracker.SetTotalWork(CurrentCookingRecipe.TimeToCook);
            }
        }

        OnRecipeChanged?.Invoke(this, new(oldRecipe, CurrentCookingRecipe));
    }

    public override void Interact(Player player) {
        if (Container.IsEmpty()) {
            if (player.HasKitchenObject() && CanCook(player.CurrentKitchenObject.KitchenObjectSO)) {
                player.CurrentKitchenObject.SetContainer(Container);
                InvokeOnInteract(new InteractableEvent(player));
            }
        }
        else if (Container.HasAny()){
            if (player.Container.IsEmpty()) {
                Container.KitchenObject.SetContainer(player.Container);
                InvokeOnInteract(new InteractableEvent(player));
            }
            else if (player.HasKitchenObject()) {
                if(player.CurrentKitchenObject.InteractWith(Container.KitchenObject))
                    InvokeOnInteract(new InteractableEvent(player));
            }
        }

    }

    private void Update() {
        if (IsCooking()) {
            var work = Time.deltaTime * (IsBurningRecipe ? burningCookingSpeed : cookingSpeed);
            ProgressTracker.AddWorkDone(work);
        }
    }

    private void OnCookingCompleted() {
        if (!CanCook())
            return;

        var outputKOSO = CurrentCookingRecipe.Output;
        Container.KitchenObject.DestroySelf();
        KitchenObject.CreateInstance(outputKOSO, Container);
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
