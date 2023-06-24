using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(KitchenObjectHolder))]
public class ClearCounter : MonoBehaviour, IInteractable {
    public event EventHandler<InteractableEvent> OnInteract;
    public event EventHandler<InteractableEvent> OnInteractAlternate;
    public event EventHandler<SelectionChangedEvent> OnSelectedChanged;

    private bool isSelected = false;


    private KitchenObjectHolder holder;
    public KitchenObjectHolder Holder => holder;


    public KitchenObject CurrentKitchenObject => holder.KitchenObject;

    private void Awake() {
        holder = GetComponent<KitchenObjectHolder>();
    }



    public void Interact(Player player) {

        if (holder.IsEmpty()) {
            if (player.HasKitchenObject()) {
                player.CurrentKitchenObject.SetHolder(holder);
            }
        }
        else {
            if (!player.HasKitchenObject()) {
                holder.KitchenObject.SetHolder(player.Holder);
            }
        }

        OnInteractAlternate?.Invoke(this, new InteractableEvent(player));
    }

    public void InteractAlternate(Player player) {
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

}
