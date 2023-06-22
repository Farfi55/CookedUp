using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenObject : MonoBehaviour {
    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    private KitchenObjectHolder currentHolder;


    public KitchenObjectSO KitchenObjectSO => kitchenObjectSO;
    public KitchenObjectHolder CurrentHolder => currentHolder;



    public void SetHolder(KitchenObjectHolder holder) {
        currentHolder = holder;
    }




}
