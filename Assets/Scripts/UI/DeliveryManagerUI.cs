using System.Collections.Specialized;
using UnityEngine;

namespace UI
{
    public class DeliveryManagerUI : MonoBehaviour {
        [SerializeField] private Transform container;
        [SerializeField] private DeliveryManagerSingleUI recipeTemplate;
        private DeliveryManager deliveryManager;

        private void Awake() {
            recipeTemplate.gameObject.SetActive(false);
        }

        private void Start() {
            deliveryManager = DeliveryManager.Instance;
            deliveryManager.OnRecipeRequestCreated += OnRecipeRequestCreated;
            UpdateVisual();
        }

        private void OnRecipeRequestCreated(object sender, RecipeRequest e) {
            AddRecipeRequest(e);            
        }

        private void AddRecipeRequest(RecipeRequest recipeRequest) {
            var recipeUI = Instantiate(recipeTemplate, container);
            recipeUI.gameObject.SetActive(true);
            recipeUI.SetRecipeRequest(recipeRequest);
        }


        private void UpdateVisual() {
            foreach (Transform child in container) {
                if (child == recipeTemplate.transform)
                    continue;
                Destroy(child.gameObject);
            }

            foreach (var recipeRequest in deliveryManager.WaitingRequests) {
                var recipeUI = Instantiate(recipeTemplate, container);
                recipeUI.gameObject.SetActive(true);
                recipeUI.SetRecipeRequest(recipeRequest);
            }
        }
    
        private void OnDestroy() {
            deliveryManager.OnRecipeRequestCreated -= OnRecipeRequestCreated;
        }
    }
}
