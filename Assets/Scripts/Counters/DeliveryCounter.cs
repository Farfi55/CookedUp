using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryCounter : BaseCounter
{
    public KitchenObjectsContainer Container { get; private set;  }

    public override void Interact(Player player) {
        
        if (player.HasKitchenObject() && player.CurrentKitchenObject.TryGetPlate(out var plate)) {
            plate.DestroySelf();
        }
    }
}
