using CookedUp.Core;
using CookedUp.Core.Counters;
using UnityEngine;

namespace CookedUp.UI
{
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
            DeliveryManager.Instance.OnDeliverySuccess += DeliverySuccess;
            DeliveryManager.Instance.OnDeliveryFailed += DeliveryFailed;

            canvasGroup.alpha = 0f;
            timeSinceDelivery = hideEndDelay;
            successPanel.SetActive(false);
            failedPanel.SetActive(false);
        }

        private void Update() {
            timeSinceDelivery += Time.deltaTime;
            canvasGroup.alpha = 1f - Mathf.Clamp01((timeSinceDelivery - hideStartDelay) / (hideEndDelay - hideStartDelay));
        }


        private void DeliverySuccess(object sender, RecipeDeliveryEvent e) {
            if(e.DeliveryCounter != deliveryCounter) return;
        
            timeSinceDelivery = 0f;
            canvasGroup.alpha = 1f;
            successPanel.SetActive(true);
            failedPanel.SetActive(false);
        }
    
        private void DeliveryFailed(object sender, RecipeDeliveryEvent e) {
            if(e.DeliveryCounter != deliveryCounter) return;
            timeSinceDelivery = 0f;
            canvasGroup.alpha = 1f;
            successPanel.SetActive(false);
            failedPanel.SetActive(true);
        }
    }
}
