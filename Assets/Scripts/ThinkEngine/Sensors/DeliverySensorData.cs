using System;
using System.Collections.Generic;
using System.Linq;
using KitchenObjects.ScriptableObjects;
using ThinkEngine.Models;
using UnityEngine;

namespace ThinkEngine.Sensors {
    public class DeliverySensorData : MonoBehaviour {
        private DeliveryManager deliveryManager;
        
        [Header("Sensor Data")] public List<RecipeRequestASP> WaitingRecipesRequests = new();

        private void Start() {
            deliveryManager = DeliveryManager.Instance;
            deliveryManager.OnRecipeRequestCreated += RecipeRequestCreated;
            deliveryManager.OnRecipeDelivered += OnRecipeDelivered;

            UpdateRecipesData();
        }
        
        private void OnDestroy() {
            deliveryManager.OnRecipeRequestCreated -= RecipeRequestCreated;
            deliveryManager.OnRecipeDelivered -= OnRecipeDelivered;
        }


        private void RecipeRequestCreated(object sender, RecipeRequest recipeRequest) => UpdateRecipesData();

        private void OnRecipeDelivered(object sender, RecipeDeliveryEvent e) => UpdateRecipesData();

        private void UpdateRecipesData() {
            WaitingRecipesRequests = deliveryManager.WaitingRequests.ToList().ConvertAll(recipeRequest => new RecipeRequestASP(recipeRequest));
        }
    }
}
