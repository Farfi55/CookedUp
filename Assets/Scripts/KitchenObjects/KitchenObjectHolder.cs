using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenObjectChangedEvent : EventArgs {
    public readonly KitchenObjectHolder KitchenObjectHolder;
    public readonly KitchenObject OldKitchenObject;
    public readonly KitchenObject NewKitchenObject;

    public KitchenObjectChangedEvent(KitchenObjectHolder kitchenObjectHolder, KitchenObject oldKitchenObject, KitchenObject newKitchenObject) {
        KitchenObjectHolder = kitchenObjectHolder;
        OldKitchenObject = oldKitchenObject;
        NewKitchenObject = newKitchenObject;
    }
}

public class KitchenObjectHolder : MonoBehaviour {
    [SerializeField] private KitchenObject kitchenObject;

    [SerializeField] private Transform container;

    public KitchenObject KitchenObject => kitchenObject;
    public Transform Container => container;


    public event EventHandler<KitchenObjectChangedEvent> OnKitchenObjectChanged;


    public bool HasKitchenObject() => kitchenObject != null;
    public bool IsEmpty() => kitchenObject == null;

    public void Clear() {
        SetKitchenObject(null);
    }

    public void SetKitchenObject(KitchenObject kitchenObject) {
        if (this.kitchenObject == kitchenObject) {
            return;
        }
        var oldKitchenObject = this.kitchenObject;
        this.kitchenObject = kitchenObject;

        OnKitchenObjectChanged?.Invoke(this, new(this, oldKitchenObject, kitchenObject));
    }
}
