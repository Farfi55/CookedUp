using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using KitchenObjects;
using KitchenObjects.Container;
using KitchenObjects.ScriptableObjects;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.LowLevel;

namespace Players {
    public class PlayerBot : MonoBehaviour {
        
        [SerializeField] private NavMeshAgent agent;
        [SerializeField] private PlayerMovement playerMovement;
        [SerializeField] private Player player;

        public bool HasPlate => Plate != null;
        public PlateKitchenObject Plate { get; private set; }

        public bool HasRecipeRequest => CurrentRecipeRequest != null;
        public RecipeRequest CurrentRecipeRequest { get; private set; }


        public event EventHandler<ValueChangedEvent<RecipeRequest>> OnRecipeRequestChanged;
        public static event EventHandler<ValueChangedEvent<RecipeRequest>> OnAnyRecipeRequestChanged;
        public event EventHandler<ValueChangedEvent<PlateKitchenObject>> OnPlateChanged;
        public event EventHandler<KitchenObjectsChangedEvent> OnPlateIngredientsChanged;

        private void Start() {
            agent.speed = playerMovement.MovementSpeed;
            agent.angularSpeed = playerMovement.RotationSpeed;

            player.Container.OnKitchenObjectAdded += OnPlayerKOAdded;
        }

        private void OnPlayerKOAdded(object sender, KitchenObject e) {
            if (e is PlateKitchenObject plate) {
                if (Plate != plate)
                    SetPlate(plate);
            }
        }

        private void SetPlate(PlateKitchenObject plate) {
            var oldPlate = Plate;
            if (oldPlate == plate)
                return;

            if (oldPlate != null) {
                oldPlate.IngredientsContainer.OnKitchenObjectsChanged -= InvokePlateIngredientsChanged;
                oldPlate.OnDestroyed -= OnPlateDestroyed;
            }

            Plate = plate;

            if (Plate != null) {
                Plate.IngredientsContainer.OnKitchenObjectsChanged += InvokePlateIngredientsChanged;
                Plate.OnDestroyed += OnPlateDestroyed;
            }

            OnPlateChanged?.Invoke(this, new ValueChangedEvent<PlateKitchenObject>(oldPlate, Plate));
            
            if (Plate != null) {
                var ingredientsContainer = Plate.IngredientsContainer;
                OnPlateIngredientsChanged?.Invoke(this, new KitchenObjectsChangedEvent(
                    ingredientsContainer, 
                    ingredientsContainer.KitchenObjects, 
                    ingredientsContainer.KitchenObject
                ));
            }
        }

        private void InvokePlateIngredientsChanged(object sender, KitchenObjectsChangedEvent e) {
            OnPlateIngredientsChanged?.Invoke(this, e);
        }

        private void OnPlateDestroyed(object o, EventArgs eventArgs) {
            Debug.Log("Plate destroyed");
            SetPlate(null);
        }


        void Update() {
            if (Input.GetMouseButtonDown(1)) {
                SetDestinationToMousePosition();
            }

            if (Input.GetMouseButtonDown(2)) {
                player.TryAlternateInteract();
                player.StartAlternateInteract();
            } else if (Input.GetMouseButtonUp(2)) {
                player.StopAlternateInteract();
            }

            if (Input.GetMouseButtonDown(0)) {
                player.TryInteract();
            }


            // playerMovement.desiredMoveDirection = agent.desiredVelocity;
        }

        // private void RecipeRequestCreated(object sender, RecipeRequest recipeRequest) {
        //     if (HasRecipe)
        //         return;
        //     TrySelectFirstRecipe();
        // }

        // private void TrySelectFirstRecipe() {
        //     var newRecipe = deliveryManager.WaitingRequests.FirstOrDefault();
        //     SetRecipeRequest(newRecipe);
        // }
        
        public void SetRecipeRequest(RecipeRequest recipeRequest) {
            var oldRequest = CurrentRecipeRequest;
            if (oldRequest == recipeRequest)
                return;

            if (oldRequest != null) {
                oldRequest.OnRequestCompleted -= OnRecipeRequestCompleted;
                oldRequest.OnRequestExpired -= OnRecipeRequestExpired;
            }

            if (recipeRequest != null) {
                recipeRequest.OnRequestCompleted += OnRecipeRequestCompleted;
                recipeRequest.OnRequestExpired += OnRecipeRequestExpired;
            }
            
            CurrentRecipeRequest = recipeRequest;
            OnRecipeRequestChanged?.Invoke(this, new ValueChangedEvent<RecipeRequest>(oldRequest, recipeRequest));
            OnAnyRecipeRequestChanged?.Invoke(this, new ValueChangedEvent<RecipeRequest>(oldRequest, recipeRequest));
        }

        private void OnRecipeRequestExpired(object sender, EventArgs e) {
            SetRecipeRequest(null);
        }

        private void OnRecipeRequestCompleted(object sender, EventArgs e) {
            SetRecipeRequest(null);
        }


        void SetDestinationToMousePosition() {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit)) {
                playerMovement.TryMoveTo(hit.point);

                playerMovement.LookAt(hit.collider.transform);
            }
        }

        
    }
}
