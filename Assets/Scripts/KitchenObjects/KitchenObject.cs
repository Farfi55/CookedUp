using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenObject : MonoBehaviour {
    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    private KitchenObjectsContainer container;


    public KitchenObjectSO KitchenObjectSO => kitchenObjectSO;
    public KitchenObjectsContainer Container => container;

    public bool isInContainer => container != null;

    public void SetContainer(KitchenObjectsContainer newContainer) {
        if (newContainer == container) {
            return;
        }

        var oldContainer = container;
        container = newContainer;

        if (oldContainer != null) {
            oldContainer.Remove(this);
        }

        if (isInContainer) {
            if (!container.TryAdd(this)) {
                Debug.LogError($"Couldn't add {this.name} to {container.name}", this);
                container = null;
            }
        }
        else {
            transform.SetParent(null);
        }

        SetVisible(isInContainer);
    }

    public void SetVisible(bool visible) {
        gameObject.SetActive(visible);
    }

    public void RemoveFromContainer() => SetContainer(null);


    /// <summary>
    /// Removes itself from the current holder and destroys itself.
    /// </summary>
    public void DestroySelf() {
        RemoveFromContainer();
        Destroy(gameObject);
    }


    public bool IsSameType(KitchenObjectSO kitchenObjectSO) => this.kitchenObjectSO == kitchenObjectSO;
    public bool IsSameType(KitchenObject kitchenObject) => IsSameType(kitchenObject.kitchenObjectSO);


    public static KitchenObject CreateInstance(KitchenObjectSO kitchenObjectSO, KitchenObjectsContainer container = null) {
        var kitchenObject = Instantiate(kitchenObjectSO.Prefab);
        kitchenObject.SetContainer(container);
        return kitchenObject;
    }

}
