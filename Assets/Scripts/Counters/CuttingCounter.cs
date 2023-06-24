using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(KitchenObjectHolder))]
public class CuttingCounter : BaseCounter {
    public event EventHandler OnCut;
    public event EventHandler OnCutProgressChanged;

    public KitchenObjectHolder Holder => holder;
    private KitchenObjectHolder holder;

    [SerializeField] private CuttingRecipeSO[] cuttingRecipes;
    public CuttingRecipeSO CurrentRecipe { get; private set; }

    public float RemainingTimeToCut { get; private set; }


    private void Awake() {
        holder = GetComponent<KitchenObjectHolder>();
        holder.OnKitchenObjectChanged += Holder_OnKitchenObjectChanged;
    }


    private void Holder_OnKitchenObjectChanged(object sender, KitchenObjectChangedEvent e) {
        // Set the current recipe and remaining time to cut when the holder's kitchen object changes.
        if (e.NewKitchenObject == null) {
            CurrentRecipe = null;
            RemainingTimeToCut = 0f;
        }
        else {
            CurrentRecipe = RecipeFor(e.NewKitchenObject);
            RemainingTimeToCut = CurrentRecipe != null ? CurrentRecipe.TimeToCut : 0f;
        }

        OnCutProgressChanged?.Invoke(this, EventArgs.Empty);
    }


    public override void Interact(Player player) {
        if (holder.IsEmpty()) {
            if (player.HasKitchenObject() && CanCut(player.CurrentKitchenObject)) {
                player.CurrentKitchenObject.SetHolder(holder);
            }
        }
        else {
            if (!player.HasKitchenObject()) {
                holder.KitchenObject.SetHolder(player.Holder);
            }
        }

        InvokeOnInteract(new InteractableEvent(player));
    }

    public override void InteractAlternateContinuous(Player player) {
        if (!holder.HasKitchenObject())
            return;

        if (!CanCut()) {
            Debug.Log("Can't cut " + holder.KitchenObject.KitchenObjectSO.Name);
            return;
        }

        RemainingTimeToCut = Mathf.Max(RemainingTimeToCut - Time.deltaTime, 0f);


        if (GetCutProgress() >= 1f) {
            KitchenObjectSO output = CurrentRecipe.Output;

            holder.KitchenObject.DestroySelf();
            KitchenObject.CreateInstance(output, holder);
            OnCut?.Invoke(this, EventArgs.Empty);
        }
        else {
            OnCutProgressChanged?.Invoke(this, EventArgs.Empty);
        }


        InvokeOnInteractAlternate(new InteractableEvent(player));
    }

    public float GetCutProgress() {
        if (!CanCut())
            return 0f;
        return 1f - (RemainingTimeToCut / CurrentRecipe.TimeToCut);
    }



    public CuttingRecipeSO RecipeFor(KitchenObject kitchenObject) => RecipeFor(kitchenObject.KitchenObjectSO);
    public CuttingRecipeSO RecipeFor(KitchenObjectSO kitchenObjectSO) {
        return cuttingRecipes.FirstOrDefault(recipe => recipe.Input == kitchenObjectSO);
    }

    public bool CanCut() => CurrentRecipe != null;
    public bool CanCut(KitchenObject kitchenObject) => CanCut(kitchenObject.KitchenObjectSO);
    public bool CanCut(KitchenObjectSO kitchenObjectSO) => RecipeFor(kitchenObjectSO) != null;



    public KitchenObjectSO OutputFor(KitchenObject kitchenObject) => OutputFor(kitchenObject.KitchenObjectSO);
    public KitchenObjectSO OutputFor(KitchenObjectSO kitchenObjectSO) => RecipeFor(kitchenObjectSO)?.Output;

}
