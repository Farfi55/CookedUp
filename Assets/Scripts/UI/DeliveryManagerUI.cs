using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UI;
using UnityEngine;

public class DeliveryManagerUI : MonoBehaviour {
    [SerializeField] private Transform container;
    [SerializeField] private DeliveryManagerSingleUI recipeTemplate;

    private void Awake() {
        recipeTemplate.gameObject.SetActive(false);
    }

    private void Start() {
        DeliveryManager.Instance.WaitingRecipeSOs.CollectionChanged += WaitingRecipesChanged;
        
        UpdateVisual();
    }

    private void WaitingRecipesChanged(object sender, NotifyCollectionChangedEventArgs e) {
        UpdateVisual();
    }

    private void UpdateVisual() {
        foreach (Transform child in container) {
            if (child == recipeTemplate.transform)
                continue;
            Destroy(child.gameObject);
        }

        foreach (var recipeSO in DeliveryManager.Instance.WaitingRecipeSOs) {
            var recipeUI = Instantiate(recipeTemplate, container);
            recipeUI.gameObject.SetActive(true);
            recipeUI.SetRecipeSO(recipeSO);
        }
    }
    
    private void OnDestroy() {
        DeliveryManager.Instance.WaitingRecipeSOs.CollectionChanged -= WaitingRecipesChanged;
    }
}
