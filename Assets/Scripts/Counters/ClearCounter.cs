using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(KitchenObjectsContainer))]
public class ClearCounter : BaseCounter {

    public KitchenObjectsContainer Container { get; private set; }

    public KitchenObject CurrentKitchenObject => Container.KitchenObject;

    private void Awake() {
        Container = GetComponent<KitchenObjectsContainer>();
    }


    public override void Interact(Player player) {

        if (Container.IsEmpty()) {
            if (player.HasKitchenObject()) {
                player.CurrentKitchenObject.SetContainer(Container);
            }
        }
        else {
            if (!player.HasKitchenObject()) {
                CurrentKitchenObject.SetContainer(player.Container);
            }
        }

        base.InvokeOnInteractAlternate(new InteractableEvent(player));
    }

}
