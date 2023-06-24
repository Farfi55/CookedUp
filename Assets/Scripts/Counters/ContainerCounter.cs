using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerCounter : BaseCounter {
    [SerializeField] private KitchenObjectSO kitchenObjectSO;
    public KitchenObjectSO KitchenObjectSO => kitchenObjectSO;


    public override void Interact(Player player) {
        if (player.HasKitchenObject()) {
            KitchenObject kitchenObject = player.CurrentKitchenObject;
            if (kitchenObject.KitchenObjectSO == kitchenObjectSO) {
                kitchenObject.DestroySelf();
                InvokeOnInteract(new InteractableEvent(player));
            }
        }
        else {
            KitchenObject.CreateInstance(kitchenObjectSO, player.Holder);
            InvokeOnInteract(new InteractableEvent(player));
        }
    }
}
