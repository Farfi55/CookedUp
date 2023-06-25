using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerCounter : BaseCounter {
    [SerializeField] private KitchenObjectSO kitchenObjectSO;
    public KitchenObjectSO KitchenObjectSO => kitchenObjectSO;


    public override void Interact(Player player) {
        if (player.HasKitchenObject()) {
            if (player.CurrentKitchenObject.IsSameType(kitchenObjectSO)) {
                player.CurrentKitchenObject.DestroySelf();
                InvokeOnInteract(new InteractableEvent(player));
            }
        }
        else {
            KitchenObject.CreateInstance(kitchenObjectSO, player.Container);
            InvokeOnInteract(new InteractableEvent(player));
        }
    }
}
