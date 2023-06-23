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
            previusHolder.ClearKitchenObject();
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


}
