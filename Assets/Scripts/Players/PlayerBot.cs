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
        private DeliveryManager deliveryManager;
        [SerializeField] private NavMeshAgent agent;
        [SerializeField] private PlayerMovement playerMovement;
        [SerializeField] private Player player;

        public bool HasPlate => Plate != null;
        public PlateKitchenObject Plate { get; private set; }

        public bool HasRecipe => CurrentRecipeRequest != null;
        public RecipeRequest CurrentRecipeRequest { get; private set; }


        public event EventHandler<ValueChangedEvent<RecipeRequest>> OnRecipeRequestChanged;
        public event EventHandler<ValueChangedEvent<PlateKitchenObject>> OnPlateChanged;
        public event EventHandler<KitchenObjectsChangedEvent> OnPlateIngredientsChanged;

        private void Start() {
            deliveryManager = DeliveryManager.Instance;
            agent.speed = playerMovement.MovementSpeed;
            agent.angularSpeed = playerMovement.RotationSpeed;

            // todo: make a HigherLevelAI that handles recipe selection and coordination
            TrySelectRecipe();

            deliveryManager.OnRecipeRequestCreated += RecipeRequestCreated;
            deliveryManager.OnDeliverySuccess += DeliverySuccess;

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


        private void DeliverySuccess(object sender, RecipeDeliveryEvent e) {
            if (e.Player == player) {
                TrySelectRecipe(true);
            }
        }

        private void RecipeRequestCreated(object sender, RecipeRequest recipeRequest) {
            if (HasRecipe)
                return;
            TrySelectRecipe();
        }

        private void TrySelectRecipe(bool forceEvent = false) {
            var oldRequest = CurrentRecipeRequest;
            CurrentRecipeRequest = deliveryManager.WaitingRequests.FirstOrDefault();

            if (forceEvent || oldRequest != CurrentRecipeRequest)
                OnRecipeRequestChanged?.Invoke(this, new(oldRequest, CurrentRecipeRequest));
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
