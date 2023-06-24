using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(KitchenObjectHolder))]
public class ClearCounter : BaseCounter {


    private KitchenObjectHolder holder;
    public KitchenObjectHolder Holder => holder;

    public KitchenObject CurrentKitchenObject => holder.KitchenObject;

    private void Awake() {
        holder = GetComponent<KitchenObjectHolder>();
    }


    public override void Interact(Player player) {

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

        base.InvokeOnInteractAlternate(new InteractableEvent(player));
    }

}
