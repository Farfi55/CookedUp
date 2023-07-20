using System;
using System.Collections;
using System.Collections.Generic;
using Counters;
using UnityEngine;

public class DeliveryResultUI : MonoBehaviour
{
    [SerializeField] private DeliveryCounter deliveryCounter;
    [SerializeField] private CanvasGroup canvasGroup;
    
    private float timeSinceDelivery;
    [SerializeField] private float hideStartDelay = 1f;
    [SerializeField] private float hideEndDelay = 1.5f;
    
    [SerializeField] private GameObject successPanel;
    [SerializeField] private GameObject failedPanel;
    
    
    
    private void Start() {
        DeliveryManager.Instance.OnRecipeSuccess += OnRecipeSuccess;
        DeliveryManager.Instance.OnRecipeFailed += OnRecipeFailed;

        canvasGroup.alpha = 0f;
        timeSinceDelivery = hideEndDelay;
        successPanel.SetActive(false);
        failedPanel.SetActive(false);
    }

    private void Update() {
        timeSinceDelivery += Time.deltaTime;
        canvasGroup.alpha = 1f - Mathf.Clamp01((timeSinceDelivery - hideStartDelay) / (hideEndDelay - hideStartDelay));
    }


    private void OnRecipeSuccess(object sender, RecipeDeliveryEvent e) {
        if(e.DeliveryCounter != deliveryCounter) return;
        
        timeSinceDelivery = 0f;
        canvasGroup.alpha = 1f;
        successPanel.SetActive(true);
        failedPanel.SetActive(false);
    }
    
    private void OnRecipeFailed(object sender, RecipeDeliveryEvent e) {
        if(e.DeliveryCounter != deliveryCounter) return;
        timeSinceDelivery = 0f;
        canvasGroup.alpha = 1f;
        successPanel.SetActive(false);
        failedPanel.SetActive(true);
    }
}
