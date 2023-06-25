using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering.Universal;
using UnityEngine;


[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(KitchenObjectsContainer))]
public class Player : MonoBehaviour {
    private PlayerInput playerInput;
    private KitchenObjectsContainer container;
    private Rigidbody rb;

    public KitchenObjectsContainer Container => container;
    public KitchenObject CurrentKitchenObject => container.KitchenObject;




    [SerializeField] private float movementSpeed = 5f;

    [SerializeField] private float rotationSpeed = 5f;

    [SerializeField, Range(0f, 10f)] float interactionDistance = 1f;

    [SerializeField] private LayerMask interactionLayerMask;

    /// <summary>
    /// The last movement input that was not zero.
    /// </summary>
    private Vector2 lastMovementInput;

    private IInteractable selectedInteractable = null;
    public event EventHandler OnSelectedInteractableChanged;

    public bool IsMoving => isMoving;
    private bool isMoving = false;

    public bool IsInteractingAlternate { get; private set; } = false;




    private void Awake() {
        playerInput = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody>();
        container = GetComponent<KitchenObjectsContainer>();
    }

    private void Start() {
        playerInput.OnInteract += PlayerInput_HandleInteractions;
        playerInput.OnInteractAlternate += PlayerInput_HandleAlternateInteractions;
    }


    private void PlayerInput_HandleInteractions(object sender, EventArgs e) {
        if (HasInteractableSelected()) {
            selectedInteractable.Interact(this);
        }
    }

    private void PlayerInput_HandleAlternateInteractions(object sender, EventArgs e) {
        IsInteractingAlternate = true;
        if (HasInteractableSelected()) {
            selectedInteractable.InteractAlternate(this);
        }
    }

    private void Update() {
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

    private void HandleRotation(Vector3 moveDirection) {
        isMoving = moveDirection != Vector3.zero;

        if (isMoving) {
            var targetRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            rb.rotation = Quaternion.Slerp(
                rb.rotation,
                targetRotation,
                rotationSpeed * Time.fixedDeltaTime
            );
        }
    }

    private void FixedUpdate() {
        Vector2 input = playerInput.GetMovementInput();
        var moveDirection = new Vector3(input.x, 0, input.y);

        HandleMovement(moveDirection);

        HandleRotation(moveDirection);
    }

    private void HandleMovement(Vector3 moveDirection) {
        rb.velocity = moveDirection * movementSpeed;
    }

    public bool HasKitchenObject() => CurrentKitchenObject != null;
    private bool HasInteractableSelected() => selectedInteractable != null;


}
