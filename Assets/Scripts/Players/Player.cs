using System;
using Counters;
using KitchenObjects;
using KitchenObjects.Container;
using UnityEngine;

namespace Players {
    [RequireComponent(typeof(KitchenObjectsContainer))]
    public class Player : MonoBehaviour {
        private KitchenObjectsContainer container;

        private GameManager gameManager;

        public KitchenObjectsContainer Container => container;
        public KitchenObject CurrentKitchenObject => container.KitchenObject;

        public bool HasKitchenObject() => CurrentKitchenObject != null;

        [SerializeField, Range(0f, 10f)] float interactionDistance = 1f;
        [SerializeField, Range(0f, 3f)] float interactionRange = 0.3f;

        [SerializeField] private LayerMask interactionLayerMask;
        

        public bool HasSelectedInteractable() => selectedInteractable != null;
        
        public IInteractable SelectedInteractable => selectedInteractable;
        private IInteractable selectedInteractable = null;

        public bool IsInteractingAlternate { get; private set; } = false;


        private readonly RaycastHit[] hitBuffer = new RaycastHit[8];
        
        /// <summary>
        /// ignored when equal to Vector2.zero
        /// <br/>
        /// Determines the direction of the selection ray 
        /// </summary>
        public Vector2 SelectionDirection { get; set; }


        public event EventHandler<ValueChangedEvent<IInteractable>> OnSelectedInteractableChanged;

        public static event EventHandler<Player> OnAnyPlayerSpawned;
        public static event EventHandler<Player> OnAnyPlayerDestroyed;
        public event EventHandler OnPlayerReady;


        private void Awake() {
            container = GetComponent<KitchenObjectsContainer>();
        }

        private void Start() {
            OnAnyPlayerSpawned?.Invoke(this, this);

            gameManager = GameManager.Instance;
        }


        private void OnDestroy() {
            OnAnyPlayerDestroyed?.Invoke(this, this);
        }

        public bool TryInteract() {
            if (!HasSelectedInteractable()
                || !gameManager.IsGamePlaying
                || gameManager.IsGamePaused) {
                return false;
            }

            selectedInteractable.Interact(this);
            return true;
        }

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

      
        public void OnReady()
        {
            if (!gameManager.IsGamePlaying)
            {
                OnPlayerReady?.Invoke(this, EventArgs.Empty);
            }
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

        private void UpdateSelectedInteractable() {
            var moveDirection = transform.forward;
            
            if (SelectionDirection != Vector2.zero) {
                moveDirection = new Vector3(SelectionDirection.x, 0f, SelectionDirection.y);
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

        
        public GameObject GetSelectedGameObject() {
            if (selectedInteractable is MonoBehaviour interactable) {
                return interactable.gameObject;
            }

            return null;   
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
