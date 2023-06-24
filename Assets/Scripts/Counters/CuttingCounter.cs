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

    public event EventHandler OnCut;
    public event EventHandler OnCutProgressChanged;

    private bool isSelected = false;
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

    public void InteractAlternate(Player player) { }

    public void InteractAlternateContinuous(Player player) {
        Debug.Log("Cutting");
        if (!holder.HasKitchenObject())
            return;

        if (!CanCut()) {
            Debug.Log("Can't cut " + holder.KitchenObject.KitchenObjectSO.Name);
            return;
        }

        RemainingTimeToCut = Mathf.Max(RemainingTimeToCut - Time.deltaTime, 0f);


        if (GetCutProgress() >= 1f) {
            KitchenObjectSO output = CurrentRecipe.Output;
            Debug.Log("Cut " + holder.KitchenObject.KitchenObjectSO.Name + " into " + output.Name);

            holder.KitchenObject.DestroySelf();
            KitchenObject.CreateInstance(output, holder);
            OnCut?.Invoke(this, EventArgs.Empty);
        }
        else {
            OnCutProgressChanged?.Invoke(this, EventArgs.Empty);
        }


        OnInteractAlternate?.Invoke(this, new InteractableEvent(player));
    }

    private IEnumerator Cut(Player player) {
        while (RemainingTimeToCut > 0f) {
            RemainingTimeToCut -= Time.deltaTime;
            yield return null;
        }


    }

    public bool IsSelected() => isSelected;

    public bool IsSelected(Player player) => isSelected;

    public void SetSelected(Player player, bool isSelected) {
        if (this.isSelected == isSelected)
            return;

        this.isSelected = isSelected;
        OnSelectedChanged?.Invoke(this, new SelectionChangedEvent(player, isSelected));
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
