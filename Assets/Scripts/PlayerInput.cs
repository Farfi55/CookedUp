using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour {
    private PlayerInputActions inputActions;

    public event EventHandler OnInteract;


    private void Awake() {
        inputActions = new PlayerInputActions();
        inputActions.Player.Enable();

        inputActions.Player.Interact.performed += InteractionPerformed;
    }

    private void InteractionPerformed(InputAction.CallbackContext context) {
        OnInteract?.Invoke(this, EventArgs.Empty);
    }

    /// <returns>the movement input normalized.</returns>
    public Vector2 GetMovementInput() {
        return inputActions.Player.Move.ReadValue<Vector2>().normalized;
    }


}
