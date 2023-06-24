using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerCounter : MonoBehaviour, IInteractable {
    [SerializeField] private KitchenObjectSO kitchenObjectSO;
    public KitchenObjectSO KitchenObjectSO => kitchenObjectSO;

    bool isSelected = false;

    public event EventHandler<InteractableEvent> OnInteract;
    public event EventHandler<SelectionChangedEvent> OnSelectedChanged;
    public event EventHandler<InteractableEvent> OnInteractAlternate;



    public void Interact(Player player) {
        if (player.HasKitchenObject()) {
            KitchenObject kitchenObject = player.CurrentKitchenObject;
            if (kitchenObject.KitchenObjectSO == kitchenObjectSO) {
                kitchenObject.DestroySelf();
                OnInteract?.Invoke(this, new InteractableEvent(player));
            }
        }
        else {
            KitchenObject.CreateInstance(kitchenObjectSO, player.Holder);
            OnInteract?.Invoke(this, new InteractableEvent(player));
        }

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
