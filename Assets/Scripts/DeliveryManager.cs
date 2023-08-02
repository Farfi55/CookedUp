using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Counters;
using Extensions;
using KitchenObjects;
using KitchenObjects.ScriptableObjects;
using Players;
using Unity.VisualScripting;
using UnityEngine;
using Random = System.Random;

public class DeliveryManager : MonoBehaviour {
    public static DeliveryManager Instance { get; private set; }

    private GameManager gameManager;

    [SerializeField] private CompleteRecipeSOList recipes;

    [SerializeField] private List<CompleteRecipeSO> startingRecipes;

    private readonly ObservableCollection<CompleteRecipeSO> waitingRecipeSOs = new();
    
    public ObservableCollection<CompleteRecipeSO> WaitingRecipeSOs => waitingRecipeSOs;


    private ProgressTracker progressTracker;

    [SerializeField] private float timeForNewRequest = 10f;

    [SerializeField] private int maxWaitingRequests = 3;

    [SerializeField] private int startingWaitingRequests = 1;
    public int SuccessfulDeliveriesCount { get; private set; } = 0;


    public event EventHandler<CompleteRecipeSO> OnRecipeCreated;
    public event EventHandler<RecipeDeliveryEvent> OnRecipeDelivered;
    public event EventHandler<RecipeDeliveryEvent> OnRecipeSuccess;
    public event EventHandler<RecipeDeliveryEvent> OnRecipeFailed;


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
        
        for (int i = waitingRecipeSOs.Count; i < startingWaitingRequests; i++) {
            CreateNewRecipe();
        }
    }


    private void Update() {
        if (!gameManager.IsGamePlaying) return;

        if (waitingRecipeSOs.Count < maxWaitingRequests)
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
        waitingRecipeSOs.Add(recipeSO);
        progressTracker.ResetProgress();
        OnRecipeCreated?.Invoke(this, recipeSO);
        Debug.Log($"New recipe request: {recipeSO.DisplayName}");
    }


    public void DeliverRecipe(PlateKitchenObject plate, Player player, DeliveryCounter deliveryCounter) {
        List<KitchenObjectSO> kitchenObjectSOs = plate.IngredientsContainer.AsKitchenObjectSOs();

        var completeRecipeSO =
            waitingRecipeSOs.FirstOrDefault(completeRecipeSO => completeRecipeSO.MatchesCompletely(kitchenObjectSOs));
        if (completeRecipeSO == null) {
            OnRecipeFailed?.Invoke(this, new RecipeDeliveryEvent(null, player, deliveryCounter));
            Debug.Log("No match found in waiting recipes");
        }
        else {
            waitingRecipeSOs.Remove(completeRecipeSO);
            SuccessfulDeliveriesCount++;
            Debug.Log($"Recipe {completeRecipeSO.DisplayName} delivered!");
            plate.DestroySelf();
            OnRecipeDelivered?.Invoke(this, new RecipeDeliveryEvent(completeRecipeSO, player, deliveryCounter));
            OnRecipeSuccess?.Invoke(this, new RecipeDeliveryEvent(completeRecipeSO, player, deliveryCounter));
        }
    }
}

public struct RecipeDeliveryEvent {
    public CompleteRecipeSO RecipeSO { get; }
    public Player Player { get; }
    public DeliveryCounter DeliveryCounter { get; }

    public RecipeDeliveryEvent(CompleteRecipeSO recipeSo, Player player, DeliveryCounter deliveryCounter) {
        RecipeSO = recipeSo;
        Player = player;
        DeliveryCounter = deliveryCounter;
    }
}
