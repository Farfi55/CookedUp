using System;
using System.Collections.Generic;
using KitchenObjects.ScriptableObjects;
using Players;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI {
    public class DeliveryManagerSingleUI : MonoBehaviour {
        [SerializeField] private TextMeshProUGUI recipeNameText;
        [SerializeField] private TextMeshProUGUI recipeIDText;

        [Header("Ingredients"), SerializeField]
        private Transform ingredientsContainer;

        [SerializeField] private KitchenObjectIconUI ingredientTemplate;

        [Header("Player Indicators"), SerializeField]
        private Transform playerIndicatorsContainer;

        [SerializeField] private Transform playerIndicatorTemplate;
        private Dictionary<Player, GameObject> playerIndicators = new();

        [SerializeField] private ProgressTracker progressTracker;
        
        private RecipeRequest recipeRequest;
        public RecipeRequest RecipeRequest => recipeRequest;

        private void OnEnable() {
            ingredientTemplate.gameObject.SetActive(false);
            if (playerIndicatorTemplate != null) {
                playerIndicatorTemplate.gameObject.SetActive(false);
                PlayerBot.OnAnyRecipeRequestChanged += OnAnyRecipeRequestChanged;
            }
        }

        private void OnAnyRecipeRequestChanged(object sender, ValueChangedEvent<RecipeRequest> e) {
            var playerBot = (PlayerBot) sender;
            if (e.OldValue != null && e.OldValue == recipeRequest) {
                if (playerIndicators.TryGetValue(playerBot.Player, out var indicator)) {
                    Destroy(indicator);
                    playerIndicators.Remove(playerBot.Player);
                }
            }
            
            if (e.NewValue != null && e.NewValue == recipeRequest) {
                AddPlayerIndicator(playerBot.Player);
            }
        }

        private void AddPlayerIndicator(Player player)
        {
            if (playerIndicators.ContainsKey(player)) {
                Debug.LogWarning($"Recipe request {recipeRequest.ID} already has an indicator for player {player.name}");
                return;
            }
            
            var indicator = Instantiate(playerIndicatorTemplate, playerIndicatorsContainer);
            indicator.gameObject.SetActive(true);
            indicator.GetComponent<Image>().color = player.GetComponent<PlayerVisual>().PlayerColorSO.Color;
            playerIndicators.Add(player, indicator.gameObject);
        }


        public void SetRecipeRequest(RecipeRequest request) {
            recipeRequest = request;
            recipeNameText.text = request.Recipe.DisplayName;
            recipeIDText.text = "#" + request.ID;
            recipeRequest.OnRequestCompleted += OnRequestCompleted;
            recipeRequest.OnRequestExpired += OnRequestExpired;

            progressTracker.SetTotalWork(recipeRequest.TimeToComplete);

            foreach (Transform child in ingredientsContainer) {
                if (child == ingredientTemplate.transform)
                    continue;
                Destroy(child.gameObject);
            }

            foreach (var kitchenObjectSO in request.Recipe.Ingredients) {
                var ingredientUI = Instantiate(ingredientTemplate, ingredientsContainer);
                ingredientUI.gameObject.SetActive(true);
                ingredientUI.Init(kitchenObjectSO);
            }

            if (playerIndicatorTemplate != null) {
                foreach (var player in PlayersManager.Instance.Players)
                    if (player.TryGetComponent(out PlayerBot playerBot)
                        && playerBot.HasRecipeRequest && playerBot.CurrentRecipeRequest == recipeRequest) {
                        AddPlayerIndicator(player);
                    }
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
            if (recipeRequest != null) {
                recipeRequest.OnRequestCompleted -= OnRequestCompleted;
                recipeRequest.OnRequestExpired -= OnRequestExpired;
            }
            PlayerBot.OnAnyRecipeRequestChanged -= OnAnyRecipeRequestChanged;
            Destroy(gameObject);
        }
    }
}
