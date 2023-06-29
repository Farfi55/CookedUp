using System.Collections.Generic;
using KitchenObjects;
using KitchenObjects.Container;
using UnityEngine;

namespace UI
{
    public class PlatesIconUI : MonoBehaviour
    {
        [SerializeField] private PlateKitchenObject plateKitchenObject;
        [SerializeField] private PlatesIconSingleUI iconTemplate;


        private void Awake() {
            iconTemplate.gameObject.SetActive(false);
        }

        private void Start() {
            plateKitchenObject.IngredientsContainer.OnKitchenObjectsChanged += OnIngredientsChanged;
        }

        private void OnIngredientsChanged(object sender, KitchenObjectsChangedEvent e) {
            UpdateVisual(e.KitchenObjects);
        }

        private void UpdateVisual(IEnumerable<KitchenObject> kitchenObjects)
        {
            foreach (Transform child in transform) {
                if(child != iconTemplate.transform)
                    Destroy(child.gameObject);
            }
        
            foreach (var kitchenObject in kitchenObjects)
            {
                var platesIconSingleUI = Instantiate(iconTemplate, transform);
                platesIconSingleUI.Init(kitchenObject.KitchenObjectSO);
                platesIconSingleUI.gameObject.SetActive(true);
            }
        }
    }
}
