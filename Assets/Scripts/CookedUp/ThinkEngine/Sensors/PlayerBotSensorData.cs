using System;
using System.Collections.Generic;
using CookedUp.Core;
using CookedUp.Core.KitchenObjects;
using CookedUp.Core.KitchenObjects.Container;
using CookedUp.Core.Players;
using CookedUp.ThinkEngine.Models;
using Shared;
using UnityEngine;

namespace CookedUp.ThinkEngine.Sensors {
    public class PlayerBotSensorData : MonoBehaviour {
        private IDManager idManager;

        [SerializeField] private Player player;
        [SerializeField] private PlayerBot playerBot;


        [Header("Sensor Data")] [Header("Recipe")]
        public bool HasRecipe;

        public string RecipeName;

        public RecipeRequestASP CurrentRecipeRequestASP;
        

        [Header("Plate")] public bool HasPlate;
        public int PlateForRecipeID;

        [Header("Plate Ingredients")] 
        public List<string> IngredientsNames = new();
        
        public bool HasMissingIngredients;
        public List<string> MissingIngredientsNames = new();
        
        public bool HasInvalidIngredients;


        private void Start() {
            idManager = IDManager.Instance;
            
            playerBot.OnRecipeRequestChanged += RecipeRequestChanged;
            playerBot.OnPlateChanged += OnPlateChanged;
            playerBot.OnPlateIngredientsChanged += OnPlateIngredientsChanged;
            player.Container.OnKitchenObjectsChanged += OnPlateIngredientsChanged;
            UpdateRecipeData();
            UpdatePlateData();
            UpdateIngredientsData();
        }


        private void Update() {
            if (HasRecipe) {
                var remainingTimeToComplete = playerBot.CurrentRecipeRequest.RemainingTimeToComplete;
                CurrentRecipeRequestASP.RemainingTimeToComplete = Converter.FloatToInt(remainingTimeToComplete);
            }
        }

        private void OnPlateIngredientsChanged(object sender, KitchenObjectsChangedEvent e) =>
            UpdateIngredientsData();


        private void RecipeRequestChanged(object sender, ValueChangedEvent<RecipeRequest> valueChangedEvent) {
            UpdateRecipeData();
            UpdateIngredientsData();
        }

        private void UpdateRecipeData() {
            HasRecipe = playerBot.HasRecipeRequest;
            if (HasRecipe) {
                RecipeName = playerBot.CurrentRecipeRequest.Recipe.name;
                CurrentRecipeRequestASP = new RecipeRequestASP(playerBot.CurrentRecipeRequest);
            }
            else {
                RecipeName = "";
                CurrentRecipeRequestASP = null;
            }
        }

        private void OnPlateChanged(object sender, ValueChangedEvent<PlateKitchenObject> e) {
            UpdatePlateData();
            UpdateIngredientsData();
        }

        private void UpdateIngredientsData() {
            MissingIngredientsNames.Clear();
            HasMissingIngredients = false;
            HasInvalidIngredients = false;
            
            List<KitchenObjectSO> ingredients;
            if (playerBot.HasPlate) {
                ingredients = playerBot.Plate.IngredientsContainer.AsKitchenObjectSOs();
            }
            else {
                ingredients = new();
            }
            IngredientsNames = ingredients.ConvertAll(koso => koso.name);
            
            if (!playerBot.HasRecipeRequest)
                return;
            
            MissingIngredientsNames = playerBot.CurrentRecipeRequest.Recipe.GetMissingIngredient(ingredients)
                .ConvertAll(koso => koso.name);
            HasMissingIngredients = MissingIngredientsNames.Count > 0;
            HasInvalidIngredients = !playerBot.CurrentRecipeRequest.Recipe.IsValidFor(ingredients);
        }

        private void UpdatePlateData() {
            HasPlate = playerBot.HasPlate;

            if (HasPlate) {
                PlateForRecipeID = idManager.GetID(playerBot.Plate.gameObject);
                playerBot.Plate.OnDestroyed += OnPlateDestroyed;
            }
            else {
                PlateForRecipeID = 0;
            }
        }

        private void OnPlateDestroyed(object sender, EventArgs e) {
            if (sender is PlateKitchenObject plate) {
                idManager.RemoveID(plate.gameObject);
            }
            else {
                Debug.LogError($"OnPlateDestroyed: sender is not a PlateKitchenObject! {sender}");
            }
        }

    }
}
