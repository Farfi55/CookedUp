using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public interface IInteractable {
    event EventHandler<InteractableEvent> OnInteract;
    event EventHandler<InteractableEvent> OnInteractAlternate;
    event EventHandler<SelectionChangedEvent> OnSelectedChanged;


    void Interact(Player.Player player);
    void InteractAlternate(Player.Player player);
    void InteractAlternateContinuous(Player.Player player);


    void SetSelected(Player.Player player, bool isSelected);
    bool IsSelected();
    bool IsSelected(Player.Player player);

}



public class InteractableEvent : EventArgs {
    public Player.Player Player { get; private set; }

    public InteractableEvent(Player.Player player) {
        Player = player;
    }

}

public class SelectionChangedEvent : EventArgs {
    public Player.Player Player { get; private set; }
    public bool IsSelected { get; private set; }

    public SelectionChangedEvent(Player.Player player, bool isSelected) {
        Player = player;
        IsSelected = isSelected;
    }

}
