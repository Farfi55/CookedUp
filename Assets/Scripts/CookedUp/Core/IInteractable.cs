using System;
using CookedUp.Core.Players;

namespace CookedUp.Core
{
    public interface IInteractable {
        event EventHandler<InteractableEvent> OnInteract;
        event EventHandler<InteractableEvent> OnInteractAlternate;
        event EventHandler<SelectionChangedEvent> OnSelectedChanged;


        void Interact(Player player);
        void InteractAlternate(Player player);
        void InteractAlternateContinuous(Player player);


        void SetSelected(Player player, bool isSelected);
        bool IsSelected();
        bool IsSelected(Player player);

    }



    public class InteractableEvent : EventArgs {
        public Player Player { get; private set; }

        public InteractableEvent(Player player) {
            Player = player;
        }

    }

    public class SelectionChangedEvent : EventArgs {
        public Player Player { get; private set; }
        public bool IsSelected { get; private set; }

        public SelectionChangedEvent(Player player, bool isSelected) {
            Player = player;
            IsSelected = isSelected;
        }

    }
}