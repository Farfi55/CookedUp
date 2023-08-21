using CookedUp.Core;
using UnityEngine;

namespace CookedUp.UI
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

            foreach (var recipeRequest in deliveryManager.WaitingRequests) {
                AddRecipeRequest(recipeRequest);
            }
        }

        private void OnRecipeRequestCreated(object sender, RecipeRequest e) {
            AddRecipeRequest(e);            
        }

        private void AddRecipeRequest(RecipeRequest recipeRequest) {
            var recipeUI = Instantiate(recipeTemplate, container);
            recipeUI.gameObject.SetActive(true);
            recipeUI.SetRecipeRequest(recipeRequest);
        }
    
        private void OnDestroy() {
            deliveryManager.OnRecipeRequestCreated -= OnRecipeRequestCreated;
        }
    }
}
