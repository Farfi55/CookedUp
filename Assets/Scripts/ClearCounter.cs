using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ClearCounter : MonoBehaviour, IInteractable
{
    public event EventHandler<InteractableEvent> OnInteracted;
    public event EventHandler<SelectionChangedEvent> OnSelectedChanged;

    private bool isSelected = false;

    public void Interact(Player player) {
        Debug.Log($"{gameObject.name} was interacted with!");
        OnInteracted?.Invoke(this, new InteractableEvent(player));
    }

    public bool IsSelected() => isSelected;

    public bool IsSelected(Player player) => isSelected;

    public void SetSelected(Player player, bool isSelected)
    {
        if(this.isSelected == isSelected) 
            return;

        this.isSelected = isSelected;
        OnSelectedChanged?.Invoke(this, new SelectionChangedEvent(player, isSelected));
    }
}
