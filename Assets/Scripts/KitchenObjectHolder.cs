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
    public KitchenObject KitchenObject => kitchenObject;
    [SerializeField] private KitchenObject kitchenObject;

    [SerializeField] private Transform objectContainer;

    public event EventHandler<KitchenObjectChangedEvent> OnKitchenObjectChanged;



    public bool HasKitchenObject() => kitchenObject != null;
    public bool IsEmpty() => kitchenObject == null;

    public void ClearKitchenObject() {
        SetKitchenObject(null);
    }

    public void SetKitchenObject(KitchenObject kitchenObject) {
        var oldKitchenObject = this.kitchenObject;

        if (oldKitchenObject == kitchenObject) {
            return;
        }

        if(oldKitchenObject != null && oldKitchenObject.CurrentHolder == this) {
            oldKitchenObject.SetHolder(null);
            oldKitchenObject.transform.SetParent(null);
            oldKitchenObject.gameObject.SetActive(false);
        }

        this.kitchenObject = kitchenObject;

        if (kitchenObject != null) {
            kitchenObject.SetHolder(this);
            kitchenObject.transform.SetParent(objectContainer);
            kitchenObject.transform.localPosition = Vector3.zero;
            oldKitchenObject.gameObject.SetActive(true);
        }

        OnKitchenObjectChanged?.Invoke(this, new KitchenObjectChangedEvent(this, oldKitchenObject, kitchenObject));

    }

    public void CreateKitchenObject(KitchenObjectSO kitchenObjectSO) {
        kitchenObject = Instantiate(kitchenObjectSO.Prefab, transform);
        kitchenObject.transform.localPosition = Vector3.zero;
        SetKitchenObject(kitchenObject);
    }

    public void MoveKitchenObjectTo(KitchenObjectHolder toHolder) {
        if (toHolder == null) {
            Debug.LogError("toHolder is null");
            return;
        }

        if (toHolder.HasKitchenObject()) {
            Debug.LogError("toHolder already has a kitchen object");
            return;
        }

        this.ClearKitchenObject();
        toHolder.SetKitchenObject(kitchenObject);


    }
}
