using System;
using System.Collections.Generic;
using CookedUp.Core.Players;
using UnityEngine;

namespace CookedUp.Core.Counters
{
    public abstract class BaseCounter : MonoBehaviour, IInteractable {
        public event EventHandler<InteractableEvent> OnInteract;
        public event EventHandler<InteractableEvent> OnInteractAlternate;
        public event EventHandler<SelectionChangedEvent> OnSelectedChanged;

        private List<Player> playersSelecting = new();

        public virtual void Interact(Player player) { }

        public virtual void InteractAlternate(Player player) { }

        public virtual void InteractAlternateContinuous(Player player) { }

        public bool IsSelected() => playersSelecting.Count > 0;

        public bool IsSelected(Player player) => playersSelecting.Contains(player);

        public void SetSelected(Player player, bool isSelected) {
            if (isSelected) {
                if (!playersSelecting.Contains(player)) {
                    playersSelecting.Add(player);
                    OnSelectedChanged?.Invoke(this, new SelectionChangedEvent(player, isSelected));
                }
            }
            else {
                if (playersSelecting.Contains(player)) {
                    playersSelecting.Remove(player);
                    OnSelectedChanged?.Invoke(this, new SelectionChangedEvent(player, isSelected));
                }
            }

        }

        protected void InvokeOnInteract(InteractableEvent interactableEvent) {
            OnInteract?.Invoke(this, interactableEvent);
        }
        protected void InvokeOnInteractAlternate(InteractableEvent interactableEvent) {
            OnInteractAlternate?.Invoke(this, interactableEvent);
        }

    }
}
