using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CookedUp.Core.Players
{
    public class PlayerInput : MonoBehaviour {
        private PlayerInputActions inputActions;
        
        public event EventHandler OnInteract;
        public event EventHandler OnInteractAlternateStarted;
        public event EventHandler OnInteractAlternate;
        public event EventHandler OnInteractAlternateCanceled;
        public event EventHandler OnReady;


        private void Awake() {
            inputActions = new PlayerInputActions();
            inputActions.Player.Enable();
            
            inputActions.Player.Interact.performed += InteractPerformed;

            inputActions.Player.InteractAlternate.started += InteractAlternateStarted;
            inputActions.Player.InteractAlternate.performed += InteractAlternatePerformed;
            inputActions.Player.InteractAlternate.canceled += InteractAlternateCanceled;
            
            inputActions.Player.Ready.performed += ReadyPerformed;
        }

       

        private void OnDestroy() {
            inputActions.Player.Interact.performed -= InteractPerformed;
            
            inputActions.Player.InteractAlternate.started -= InteractAlternateStarted;
            inputActions.Player.InteractAlternate.performed -= InteractAlternatePerformed;
            inputActions.Player.InteractAlternate.canceled -= InteractAlternateCanceled;
            
            inputActions.Player.Ready.performed -= ReadyPerformed;

            inputActions.Dispose();
        }


        /// <returns>the movement input normalized.</returns>
        public Vector2 GetMovementInput() {
            return inputActions.Player.Move.ReadValue<Vector2>().normalized;
        }

        private void InteractPerformed(InputAction.CallbackContext context) {
            OnInteract?.Invoke(this, EventArgs.Empty);
        }

        
        private void InteractAlternateStarted(InputAction.CallbackContext obj) {
            OnInteractAlternateStarted?.Invoke(this, EventArgs.Empty);
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
        
        private void ReadyPerformed(InputAction.CallbackContext obj) {
            OnReady?.Invoke(this, EventArgs.Empty);
        }

    }
}
