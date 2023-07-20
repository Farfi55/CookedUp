using System;
using KitchenObjects;
using KitchenObjects.Container;
using UnityEngine;

namespace Players
{
    [RequireComponent(typeof(PlayerInput))]
    [RequireComponent(typeof(KitchenObjectsContainer))]
    public class Player : MonoBehaviour {
        
        public PlayerInput PlayerInput => playerInput;
        private PlayerInput playerInput;
        private KitchenObjectsContainer container;
        
        private GameManager gameManager;

        public KitchenObjectsContainer Container => container;
        public KitchenObject CurrentKitchenObject => container.KitchenObject;
        

        [SerializeField, Range(0f, 10f)] float interactionDistance = 1f;

        [SerializeField] private LayerMask interactionLayerMask;

        /// <summary>
        /// The last movement input that was not zero.
        /// </summary>
        private Vector2 lastMovementInput;

        private IInteractable selectedInteractable = null;
        public event EventHandler OnSelectedInteractableChanged;
        
        public static event EventHandler<Player> OnAnyPlayerSpawned; 
        public static event EventHandler<Player> OnAnyPlayerDestroyed;
        public event EventHandler OnPlayerReady;

        public bool IsInteractingAlternate { get; private set; } = false;




        private void Awake() {
            playerInput = GetComponent<PlayerInput>();
            container = GetComponent<KitchenObjectsContainer>();
        }

        private void Start() {
            OnAnyPlayerSpawned?.Invoke(this, this);
            
            gameManager = GameManager.Instance;
            
            playerInput.OnInteract += HandleInteractionInput;
            playerInput.OnInteractAlternate += HandleAlternateInteractionInput;
            playerInput.OnReady += HandleReadyInput;
            playerInput.OnPause += HandlePauseInput;
            
        }

        private void OnDestroy() {
            OnAnyPlayerDestroyed?.Invoke(this, this);
        }

        private void HandleInteractionInput(object sender, EventArgs e) {
            if(!gameManager.IsGamePlaying || gameManager.IsGamePaused)
                return;
            
            if (HasInteractableSelected()) {
                selectedInteractable.Interact(this);
            }
        }

        private void HandleAlternateInteractionInput(object sender, EventArgs e) {
            if(!gameManager.IsGamePlaying || gameManager.IsGamePaused)
                return;
            
            IsInteractingAlternate = true;
            if (HasInteractableSelected()) {
                selectedInteractable.InteractAlternate(this);
            }
        }
        
        private void HandleReadyInput(object sender, EventArgs e) {
            if (!gameManager.IsGamePlaying) {
                OnPlayerReady?.Invoke(this, EventArgs.Empty);
            }
                
        }

        private void Update() {
            if(gameManager.IsGamePaused)
                return;
            
            UpdateSelectedInteractable();

            IsInteractingAlternate = playerInput.IsInteractAlternatePressed();

            if (IsInteractingAlternate && HasInteractableSelected()) {
                selectedInteractable.InteractAlternateContinuous(this);
            }

            if (playerInput.GetMovementInput() != Vector2.zero) {
                lastMovementInput = playerInput.GetMovementInput();
            }
        }

        private void UpdateSelectedInteractable() {
            Vector2 input = playerInput.GetMovementInput();
            if (input == Vector2.zero)
                input = lastMovementInput;

            var moveDirection = new Vector3(input.x, 0, input.y);

            IInteractable lastSelectedInteractable = selectedInteractable;
            selectedInteractable = null;

            if (Physics.Raycast(
                    transform.position,
                    moveDirection,
                    out RaycastHit hit,
                    interactionDistance,
                    interactionLayerMask
                )) {
                if (hit.collider.TryGetComponent<IInteractable>(out IInteractable interactable)) {
                    selectedInteractable = interactable;
                }
            }

            if (selectedInteractable != lastSelectedInteractable) {

                lastSelectedInteractable?.SetSelected(this, false);
                selectedInteractable?.SetSelected(this, true);

                OnSelectedInteractableChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        
        private void HandlePauseInput(object sender, EventArgs e) {
            gameManager.TogglePause();
        }

        public bool HasKitchenObject() => CurrentKitchenObject != null;
        private bool HasInteractableSelected() => selectedInteractable != null;

    }
}
