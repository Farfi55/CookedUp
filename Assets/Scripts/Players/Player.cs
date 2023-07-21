using System;
using Counters;
using KitchenObjects;
using KitchenObjects.Container;
using UnityEngine;

namespace Players {
    [RequireComponent(typeof(PlayerInput))]
    [RequireComponent(typeof(KitchenObjectsContainer))]
    public class Player : MonoBehaviour {
        public PlayerInput PlayerInput => playerInput;
        private PlayerInput playerInput;
        private KitchenObjectsContainer container;

        private GameManager gameManager;

        public KitchenObjectsContainer Container => container;
        public KitchenObject CurrentKitchenObject => container.KitchenObject;

        public bool HasKitchenObject() => CurrentKitchenObject != null;

        [SerializeField, Range(0f, 10f)] float interactionDistance = 1f;
        [SerializeField, Range(0f, 3f)] float interactionRange = 0.3f;

        [SerializeField] private LayerMask interactionLayerMask;

        /// <summary>
        /// The last movement input that was not zero.
        /// </summary>
        private Vector2 lastMovementInput;

        private bool HasSelectedInteractable() => selectedInteractable != null;
        public IInteractable SelectedInteractable => selectedInteractable;
        private IInteractable selectedInteractable = null;

        public bool IsInteractingAlternate { get; private set; } = false;


        private RaycastHit[] hitBuffer = new RaycastHit[8];


        public event EventHandler<ValueChangedEvent<IInteractable>> OnSelectedInteractableChanged;

        public static event EventHandler<Player> OnAnyPlayerSpawned;
        public static event EventHandler<Player> OnAnyPlayerDestroyed;
        public event EventHandler OnPlayerReady;


        private void Awake() {
            playerInput = GetComponent<PlayerInput>();
            container = GetComponent<KitchenObjectsContainer>();
        }

        private void Start() {
            OnAnyPlayerSpawned?.Invoke(this, this);

            gameManager = GameManager.Instance;

            playerInput.OnInteract += HandleInteractionInput;
            
            playerInput.OnInteractAlternateStarted += HandleAlternateInteractionStartedInput;
            playerInput.OnInteractAlternate += HandleAlternateInteractionInput;
            playerInput.OnInteractAlternateCanceled += HandleAlternateInteractionCanceledInput;
            
            playerInput.OnReady += HandleReadyInput;
            playerInput.OnPause += HandlePauseInput;
        }


        private void OnDestroy() {
            OnAnyPlayerDestroyed?.Invoke(this, this);
            
            playerInput.OnInteract -=  HandleInteractionInput;
            
            playerInput.OnInteractAlternateStarted -= HandleAlternateInteractionStartedInput;
            playerInput.OnInteractAlternate -= HandleAlternateInteractionInput;
            playerInput.OnInteractAlternateCanceled -= HandleAlternateInteractionCanceledInput;
            
            playerInput.OnReady -= HandleReadyInput;
            playerInput.OnPause -= HandlePauseInput;
        }

        private void HandleInteractionInput(object sender, EventArgs e) => TryInteract();

        public bool TryInteract() {
            if (!HasSelectedInteractable()
                || !gameManager.IsGamePlaying
                || gameManager.IsGamePaused) {
                return false;
            }

            selectedInteractable.Interact(this);
            return true;
        }

        private void HandleAlternateInteractionStartedInput(object sender, EventArgs e) => StartAlternateInteract();
        private void HandleAlternateInteractionInput(object sender, EventArgs e) => TryAlternateInteract();
        private void HandleAlternateInteractionCanceledInput(object sender, EventArgs e) => StopAlternateInteract();

        public bool TryAlternateInteract() {
            if (!HasSelectedInteractable()
                || !gameManager.IsGamePlaying
                || gameManager.IsGamePaused) {
                return false;
            }
            selectedInteractable.InteractAlternate(this);
            return true;
        }

        public void StartAlternateInteract() {
            IsInteractingAlternate = true;
        }
        
        public void StopAlternateInteract() {
            IsInteractingAlternate = false;
        }

        private void HandleReadyInput(object sender, EventArgs e) {
            if (!gameManager.IsGamePlaying) {
                OnPlayerReady?.Invoke(this, EventArgs.Empty);
            }
        }

        private void HandlePauseInput(object sender, EventArgs e) {
            gameManager.TogglePause();
        }


        private void Update() {
            if (gameManager.IsGamePaused)
                return;

            UpdateSelectedInteractable();

            if (!gameManager.IsGamePlaying) 
                return;
            
            if (IsInteractingAlternate && HasSelectedInteractable()) {
                selectedInteractable.InteractAlternateContinuous(this);
            }
        }

        private void LateUpdate() {
            if (playerInput.GetMovementInput() != Vector2.zero) {
                lastMovementInput = playerInput.GetMovementInput();
            }
        }


        private void UpdateSelectedInteractable() {
            var moveDirection = transform.forward;
            var movementInput = playerInput.GetMovementInput();
            if (movementInput != Vector2.zero) {
                moveDirection = new Vector3(movementInput.x, 0f, movementInput.y);
            }

            var lastSelectedInteractable = selectedInteractable;

            Vector3 pos = transform.position + moveDirection * interactionDistance;

            int hitsCount = Physics.SphereCastNonAlloc(pos, interactionRange, moveDirection, hitBuffer, interactionDistance,
                interactionLayerMask);

            selectedInteractable = GetBestInteractable(hitsCount);


            if (selectedInteractable != lastSelectedInteractable) {
                lastSelectedInteractable?.SetSelected(this, false);
                selectedInteractable?.SetSelected(this, true);

                OnSelectedInteractableChanged?.Invoke(this, new(lastSelectedInteractable, selectedInteractable));
            }
        }

        private IInteractable GetBestInteractable(int count) {
            float bestCost = float.MaxValue;

            IInteractable bestInteractable = null;
            for (int i = 0; i < count; i++) {
                Collider coll = hitBuffer[i].collider;

                if (!coll.TryGetComponent(out IInteractable interactable))
                    continue;

                float cost = GetInteractableCost(interactable, coll.transform.position);

                if (cost < bestCost) {
                    bestCost = cost;
                    bestInteractable = interactable;
                }
            }

            return bestInteractable;
        }


        private float GetInteractableCost(IInteractable interactable, Vector3 interactablePosition) {
            var pos = transform.position;
            
            var dist = Vector3.Distance(pos, interactablePosition);
            var angle = Vector3.Angle(transform.forward, interactablePosition - pos);
            var cost = dist + angle / 20f;
            return cost;
        }
    }
}
