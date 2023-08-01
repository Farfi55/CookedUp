using System;
using System.Collections.Generic;
using System.Linq;
using KitchenObjects.ScriptableObjects;
using ThinkEngine.Models;
using UnityEngine;

namespace ThinkEngine.Sensors {
    public class DeliverySensorData : MonoBehaviour {
        private DeliveryManager deliveryManager;
        
        [Header("Sensor Data")] public List<CompleteRecipe> Recipes = new();

        private void Start() {
            deliveryManager = DeliveryManager.Instance;
            deliveryManager.OnRecipeCreated += OnRecipeCreated;
            deliveryManager.OnRecipeDelivered += OnRecipeDelivered;

            UpdateRecipesData();
        }
        
        private void OnDestroy() {
            deliveryManager.OnRecipeCreated -= OnRecipeCreated;
            deliveryManager.OnRecipeDelivered -= OnRecipeDelivered;
        }


        private void OnRecipeCreated(object sender, CompleteRecipeSO e) => UpdateRecipesData();

        private void OnRecipeDelivered(object sender, RecipeDeliveryEvent e) => UpdateRecipesData();

        private void UpdateRecipesData() {
            Recipes = deliveryManager.WaitingRecipeSOs.ToList().ConvertAll(completeRecipeSO => new CompleteRecipe(completeRecipeSO));
        }
    }
}
