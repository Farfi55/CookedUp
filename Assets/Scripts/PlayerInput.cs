using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private PlayerInputActions inputActions;
    

    private void Awake() {
        inputActions = new PlayerInputActions();
        inputActions.Player.Enable();
    }

    /// <returns>the movement input normalized.</returns>
    public Vector2 GetMovementInput() {
        return inputActions.Player.Move.ReadValue<Vector2>().normalized;
    }
}
