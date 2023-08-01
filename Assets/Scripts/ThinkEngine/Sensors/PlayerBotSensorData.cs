using System;
using System.Collections.Generic;
using KitchenObjects;
using KitchenObjects.Container;
using KitchenObjects.ScriptableObjects;
using Players;
using UnityEngine;
using UnityEngine.Serialization;

namespace ThinkEngine.Sensors {
    public class PlayerBotSensorData : MonoBehaviour {
        private IDManager idManager;

        [SerializeField] private Player player;
        [SerializeField] private PlayerBot playerBot;


        [Header("Sensor Data")] [Header("Recipe")]
        public bool HasRecipe;

        public string RecipeName;

        [Header("Plate")] public bool HasPlate;
        public int PlateForRecipeID;

        [Header("Plate Ingredients")] public bool HasMissingIngredients;
        public List<string> MissingIngredientsNames = new();
        public bool HasInvalidIngredients;


        private void Start() {
            idManager = IDManager.Instance;
            
            playerBot.OnRecipeChanged += OnRecipeChanged;
            playerBot.OnPlateChanged += OnPlateChanged;
            playerBot.OnPlateIngredientsChanged += OnPlateIngredientsChanged;
            player.Container.OnKitchenObjectsChanged += OnPlateIngredientsChanged;
            UpdateRecipeData();
            UpdatePlateData();
            UpdateMissingIngredientsData();
        }

        private void OnPlateIngredientsChanged(object sender, KitchenObjectsChangedEvent e) =>
            UpdateMissingIngredientsData();


        private void OnRecipeChanged(object sender, ValueChangedEvent<CompleteRecipeSO> e) {
            UpdateRecipeData();
            UpdateMissingIngredientsData();
        }

        private void UpdateRecipeData() {
            HasRecipe = playerBot.HasRecipe;
            RecipeName = playerBot.HasRecipe ? playerBot.CurrentRecipe.name : "";
        }

        private void OnPlateChanged(object sender, ValueChangedEvent<PlateKitchenObject> e) {
            UpdatePlateData();
            UpdateMissingIngredientsData();
        }

        private void UpdateMissingIngredientsData() {
            MissingIngredientsNames.Clear();
            HasMissingIngredients = false;
            HasInvalidIngredients = false;
            if (!playerBot.HasRecipe)
                return;
            List<KitchenObjectSO> ingredients;
            if (!playerBot.HasPlate) {
                ingredients = player.Container.AsKitchenObjectSOs();
            }
            else {
                ingredients = playerBot.Plate.IngredientsContainer.AsKitchenObjectSOs();
            }

            MissingIngredientsNames = playerBot.CurrentRecipe.GetMissingIngredient(ingredients)
                .ConvertAll(koso => koso.name);
            HasMissingIngredients = MissingIngredientsNames.Count > 0;
            HasInvalidIngredients = !playerBot.CurrentRecipe.IsValidFor(ingredients);
        }

        private void UpdatePlateData() {
            HasPlate = playerBot.HasPlate;

            if (playerBot.HasPlate) {
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
