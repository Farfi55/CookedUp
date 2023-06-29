using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryCounter : BaseCounter
{
    public KitchenObjectsContainer Container { get; private set;  }
    
    private void Awake() {
        Container = GetComponent<KitchenObjectsContainer>();
    }

    public override void Interact(Player player) {
        
        if (player.HasKitchenObject() && player.CurrentKitchenObject.TryGetPlate(out var plate)) {
            plate.SetContainer(Container);
            DeliveryManager.Instance.DeliverRecipe(plate);
            
            InvokeOnInteract(new InteractableEvent(player));
        }
    }
}
