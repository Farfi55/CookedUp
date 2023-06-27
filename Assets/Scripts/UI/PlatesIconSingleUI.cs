using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PlatesIconSingleUI : MonoBehaviour {
    
    [SerializeField] private Image Icon;


    public void Init(KitchenObjectSO kitchenObjectSO) {
        gameObject.name = kitchenObjectSO.name + "Icon";
        Icon.sprite = kitchenObjectSO.Sprite;
    }

    
}
