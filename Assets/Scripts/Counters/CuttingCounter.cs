using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(KitchenObjectHolder))]
public class CuttingCounter : MonoBehaviour, IInteractable {
    public event EventHandler<InteractableEvent> OnInteract;
    public event EventHandler<SelectionChangedEvent> OnSelectedChanged;
    public event EventHandler<InteractableEvent> OnInteractAlternate;

    private bool isSelected = false;
    public KitchenObjectHolder Holder => holder;
    private KitchenObjectHolder holder;

    [SerializeField] private CuttingRecipeSO[] cuttingRecipes;

    private void Awake() {
        holder = GetComponent<KitchenObjectHolder>();
    }


    public void Interact(Player player) {
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

        OnInteract?.Invoke(this, new InteractableEvent(player));
    }

    public void InteractAlternate(Player player) {
        if (!holder.HasKitchenObject())
            return;

        var output = OutputFor(holder.KitchenObject);

        if (output == null) {
            Debug.Log("No cutting recipe for " + holder.KitchenObject.KitchenObjectSO.name);
            return;
        }

        holder.KitchenObject.DestroySelf();
        KitchenObject.CreateInstance(output, holder);

        OnInteractAlternate?.Invoke(this, new InteractableEvent(player));
    }

    public bool IsSelected() => isSelected;

    public bool IsSelected(Player player) => isSelected;

    public void SetSelected(Player player, bool isSelected) {
        if (this.isSelected == isSelected)
            return;

        this.isSelected = isSelected;
        OnSelectedChanged?.Invoke(this, new SelectionChangedEvent(player, isSelected));
    }


    public CuttingRecipeSO RecipeFor(KitchenObject kitchenObject) => RecipeFor(kitchenObject.KitchenObjectSO);
    public CuttingRecipeSO RecipeFor(KitchenObjectSO kitchenObjectSO) {
        return cuttingRecipes.FirstOrDefault(recipe => recipe.Input == kitchenObjectSO);
    }

    public bool CanCut(KitchenObject kitchenObject) => CanCut(kitchenObject.KitchenObjectSO);
    public bool CanCut(KitchenObjectSO kitchenObjectSO) => RecipeFor(kitchenObjectSO) != null;



    public KitchenObjectSO OutputFor(KitchenObject kitchenObject) => OutputFor(kitchenObject.KitchenObjectSO);
    public KitchenObjectSO OutputFor(KitchenObjectSO kitchenObjectSO) => RecipeFor(kitchenObjectSO)?.Output;

}
