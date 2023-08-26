using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CookedUp.Core.Counters;
using CookedUp.Core.KitchenObjects;
using CookedUp.Core.Players;
using Shared;
using Shared.Extensions;
using UnityEngine;

namespace CookedUp.Core
{
    public class DeliveryManager : MonoBehaviour {
        public static DeliveryManager Instance { get; private set; }

        private GameManager gameManager;

        [SerializeField] private CompleteRecipeSOList recipes;

        [SerializeField] private List<CompleteRecipeSO> startingRecipes;

        private readonly ObservableCollection<RecipeRequest> waitingRequests = new();
    
        public ObservableCollection<RecipeRequest> WaitingRequests => waitingRequests;


        private ProgressTracker progressTracker;

        private int nextRecipeRequestID = 0;
    
        [SerializeField] private float timeToDeliverRecipe = 20f;
        [SerializeField] private float timeForNewRequest = 10f;

        [SerializeField] private int maxWaitingRequests = 3;

        [SerializeField] private int startingWaitingRequests = 1;
        [SerializeField] private bool oneStartingRecipePerPlayer = true;
        public int SuccessfulDeliveriesCount { get; private set; }
        public float SuccessfulDeliveriesValue { get; private set; }


        public event EventHandler<RecipeRequest> OnRecipeRequestCreated;
        public event EventHandler<RecipeDeliveryEvent> OnRecipeDelivered;
        public event EventHandler<RecipeDeliveryEvent> OnDeliverySuccess;
        public event EventHandler<RecipeDeliveryEvent> OnDeliveryFailed;


        private void Awake() {
            if (Instance == null)
                Instance = this;
            else {
                Debug.LogError("Multiple DeliveryManagers in scene!");
                Destroy(gameObject);
            }
        
            progressTracker = GetComponent<ProgressTracker>();
        }

        private void Start() {
            gameManager = GameManager.Instance;
        
            progressTracker.SetTotalWork(timeForNewRequest);
            progressTracker.OnProgressComplete += ProgressTrackerOnOnProgressComplete;

            foreach (var startingRecipe in startingRecipes) {
                AddRecipe(startingRecipe);
            }
            startingRecipes.Clear();
        
            for (int i = waitingRequests.Count; i < startingWaitingRequests; i++) {
                CreateNewRecipe();
            }
        
            SuccessfulDeliveriesCount = 0;
            SuccessfulDeliveriesValue = 0;
        }


        private void Update() {
            if (!gameManager.IsGamePlaying) {
                while (oneStartingRecipePerPlayer && waitingRequests.Count < PlayersManager.Instance.PlayerCount) {
                    CreateNewRecipe();
                }
            }
        
            foreach (var recipeRequest in waitingRequests) {
                recipeRequest.UpdateTime(Time.deltaTime);
            }
        
            waitingRequests.RemoveAll(request => request.IsExpired);
        
        
            if (waitingRequests.Count < maxWaitingRequests)
                progressTracker.AddWorkDone(Time.deltaTime);
        }

        private void ProgressTrackerOnOnProgressComplete(object sender, EventArgs e) {
            CreateNewRecipe();
        }

    
        private void CreateNewRecipe() {
            var completeRecipeSO = recipes.RecipeSOList.GetRandomElement();
            AddRecipe(completeRecipeSO);
        }

        private void AddRecipe(CompleteRecipeSO recipeSO) {
            var recipeRequest = new RecipeRequest(recipeSO, timeToDeliverRecipe, nextRecipeRequestID);
            AddRecipeRequest(recipeRequest);
        }

        private void AddRecipeRequest(RecipeRequest recipeRequest) {
            waitingRequests.Add(recipeRequest);
            nextRecipeRequestID = recipeRequest.ID + 1;
            progressTracker.ResetProgress();
            OnRecipeRequestCreated?.Invoke(this, recipeRequest);
            Debug.Log($"New recipe request: {recipeRequest.Recipe.DisplayName}");
        }


        public void DeliverRecipe(PlateKitchenObject plate, Player player, DeliveryCounter deliveryCounter) {
            List<KitchenObjectSO> kitchenObjectSOs = plate.IngredientsContainer.AsKitchenObjectSOs();

            var recipeRequest = waitingRequests.FirstOrDefault(request => request.Recipe.MatchesCompletely(kitchenObjectSOs));
            if (recipeRequest == null) {
                OnDeliveryFailed?.Invoke(this, new RecipeDeliveryEvent(null, player, deliveryCounter));
                Debug.Log("No match found in waiting recipes");
            }
            else { 
                waitingRequests.Remove(recipeRequest);
                SuccessfulDeliveriesCount++;
                SuccessfulDeliveriesValue += recipeRequest.Value;
                recipeRequest.Complete();
                Debug.Log($"Recipe {recipeRequest.Recipe.DisplayName} delivered!");
                plate.DestroySelf();
                OnRecipeDelivered?.Invoke(this, new RecipeDeliveryEvent(recipeRequest, player, deliveryCounter));
                OnDeliverySuccess?.Invoke(this, new RecipeDeliveryEvent(recipeRequest, player, deliveryCounter));

                if (waitingRequests.Count == 0)
                    CreateNewRecipe();
            }
        }

        public RecipeRequest GetRecipeRequestFromID(int recipeRequestID) {
            return waitingRequests.FirstOrDefault(request => request.ID == recipeRequestID);
        }
    
    }

    public struct RecipeDeliveryEvent {
        public RecipeRequest RecipeRequest { get; }
        public Player Player { get; }
        public DeliveryCounter DeliveryCounter { get; }

        public RecipeDeliveryEvent(RecipeRequest recipeRequest, Player player, DeliveryCounter deliveryCounter) {
            RecipeRequest = recipeRequest;
            Player = player;
            DeliveryCounter = deliveryCounter;
        }
    }
}
