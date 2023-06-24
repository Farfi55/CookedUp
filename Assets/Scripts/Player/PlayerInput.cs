using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour {
    private PlayerInputActions inputActions;


    public event EventHandler OnInteract;
    public event EventHandler OnInteractAlternate;
    public event EventHandler OnInteractAlternateCanceled;


    private void Awake() {
        inputActions = new PlayerInputActions();
        inputActions.Player.Enable();

        inputActions.Player.Interact.performed += InteractPerformed;

        inputActions.Player.InteractAlternate.performed += InteractAlternatePerformed;
        inputActions.Player.InteractAlternate.canceled += InteractAlternateCanceled;


    }



    /// <returns>the movement input normalized.</returns>
    public Vector2 GetMovementInput() {
        return inputActions.Player.Move.ReadValue<Vector2>().normalized;
    }

    private void InteractPerformed(InputAction.CallbackContext context) {
        OnInteract?.Invoke(this, EventArgs.Empty);
    }

    private void InteractAlternatePerformed(InputAction.CallbackContext context) {
        OnInteractAlternate?.Invoke(this, EventArgs.Empty);
    }

    private void InteractAlternateCanceled(InputAction.CallbackContext context) {
        OnInteractAlternateCanceled?.Invoke(this, EventArgs.Empty);
    }

    public bool IsInteractAlternatePressed() {
        return inputActions.Player.InteractAlternate.IsPressed();
    }


}
