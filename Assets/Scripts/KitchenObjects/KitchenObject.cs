using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenObject : MonoBehaviour {
    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    private KitchenObjectHolder currentHolder;


    public KitchenObjectSO KitchenObjectSO => kitchenObjectSO;
    public KitchenObjectHolder CurrentHolder => currentHolder;

    bool isHeld => currentHolder != null;




    public void SetHolder(KitchenObjectHolder holder) {
        if (holder == currentHolder) {
            return;
        }

        var previusHolder = currentHolder;
        currentHolder = holder;

        if (previusHolder != null) {
            previusHolder.Clear();
        }

        if (isHeld) {
            if (currentHolder.HasKitchenObject()) {
                Debug.LogError($"Holder {currentHolder} already has a kitchen object {currentHolder.KitchenObject}", currentHolder.KitchenObject);
                currentHolder.KitchenObject.SetHolder(null);
            }
            currentHolder.SetKitchenObject(this);
        }


        gameObject.SetActive(isHeld);
        transform.SetParent(isHeld ? currentHolder.Container : null);
        transform.localPosition = Vector3.zero;
    }

    public void ClearHolder() => SetHolder(null);


    /// <summary>
    /// Removes itself from the current holder and destroys itself.
    /// </summary>
    public void DestroySelf() {
        ClearHolder();
        Destroy(gameObject);
    }


    public static KitchenObject CreateInstance(KitchenObjectSO kitchenObjectSO, KitchenObjectHolder holder = null) {
        var kitchenObject = Instantiate(kitchenObjectSO.Prefab, holder.Container);
        kitchenObject.SetHolder(holder);
        return kitchenObject;
    }

}
