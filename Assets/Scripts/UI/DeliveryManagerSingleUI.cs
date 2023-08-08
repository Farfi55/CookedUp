using System;
using KitchenObjects.ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI {
    public class DeliveryManagerSingleUI : MonoBehaviour {
        [SerializeField] private TextMeshProUGUI recipeNameText;
        [SerializeField] private TextMeshProUGUI recipeIDText;
        
        [SerializeField] private Transform ingredientsContainer;
        [SerializeField] private KitchenObjectIconUI ingredientTemplate;
        [SerializeField] private ProgressTracker progressTracker;
        private RecipeRequest recipeRequest;

        private void Awake() {
            ingredientTemplate.gameObject.SetActive(false);
        }

        public void SetRecipeRequest(RecipeRequest request) {
            recipeRequest = request;
            recipeNameText.text = request.Recipe.DisplayName;
            recipeIDText.text = "#" + request.ID;
            recipeRequest.OnRequestCompleted += OnRequestCompleted;
            recipeRequest.OnRequestExpired += OnRequestExpired;
            
            progressTracker.SetTotalWork(recipeRequest.TimeToComplete);
            
            foreach (Transform child in ingredientsContainer) {
                if(child == ingredientTemplate.transform)
                    continue;
                Destroy(child.gameObject);
            }
            
            foreach (var kitchenObjectSO in request.Recipe.Ingredients)
            {
                var ingredientUI = Instantiate(ingredientTemplate, ingredientsContainer);
                ingredientUI.gameObject.SetActive(true);
                ingredientUI.Init(kitchenObjectSO);
            }
        }


        
        private void Update() {
            progressTracker.SetWorkRemaining(recipeRequest.RemainingTimeToComplete);
        }
        
        private void OnRequestCompleted(object sender, EventArgs e) {
            DestroySelf();
        }
        private void OnRequestExpired(object sender, EventArgs e) {
            DestroySelf();    
        }

        private void DestroySelf() {
            Destroy(gameObject);
        }
        
    }
}
