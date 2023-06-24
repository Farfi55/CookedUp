using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MultipleKitchenObjectHolder : MonoBehaviour {
    [SerializeField] private List<KitchenObject> kitchenObjects = new();

    [SerializeField] private List<Transform> containers = new();

    public KitchenObject KitchenObject => GetNextAvailableKitchenObject();

    private GetPolicy getPolicy = GetPolicy.FirstInFirstOut;


    public event EventHandler<KitchenObjectChangedEvent> OnKitchenObjectChanged;

    public int Count => kitchenObjects.Count;

    public bool IsFull() => Count == containers.Count;
    public bool HasKitchenObject() => Count > 0;
    public bool IsEmpty() => Count == 0;

    public void Clear() {
        SetKitchenObject(null);
    }

    public void SetKitchenObject(KitchenObject kitchenObject) {
        throw new NotImplementedException();
    }

    private KitchenObject GetNextAvailableKitchenObject() {
        if (kitchenObjects.Count == 0) {
            return null;
        }

        return getPolicy switch {
            GetPolicy.FirstInFirstOut => kitchenObjects[0],
            GetPolicy.LastInFirstOut => kitchenObjects[Count - 1],
            _ => throw new NotImplementedException(),
        };
    }
}


// TODO: change name
public enum GetPolicy {
    FirstInFirstOut,
    LastInFirstOut
}
