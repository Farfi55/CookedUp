using System;
using UnityEngine;

namespace Players {
    public class PlayerInputHandler : MonoBehaviour {
        private GameManager gameManager;
        
        [SerializeField] private Player player;
        [SerializeField] private PlayerInput playerInput;
        [SerializeField] private PlayerMovement playerMovement;
        
        private void Start() {
            gameManager = GameManager.Instance;
            playerInput.OnInteract += HandleInteractionInput;
            
            playerInput.OnInteractAlternateStarted += HandleAlternateInteractionStartedInput;
            playerInput.OnInteractAlternate += HandleAlternateInteractionInput;
            playerInput.OnInteractAlternateCanceled += HandleAlternateInteractionCanceledInput;
            
            playerInput.OnReady += HandleReadyInput;
            playerInput.OnPause += HandlePauseInput;
        }

        private void Update() {
            var movementInput = playerInput.GetMovementInput();
            player.SelectionDirection = movementInput;
            playerMovement.MovementInput = movementInput;
        }

        private void OnDestroy() {
            playerInput.OnInteract -=  HandleInteractionInput;
            
            playerInput.OnInteractAlternateStarted -= HandleAlternateInteractionStartedInput;
            playerInput.OnInteractAlternate -= HandleAlternateInteractionInput;
            playerInput.OnInteractAlternateCanceled -= HandleAlternateInteractionCanceledInput;
            
            playerInput.OnReady -= HandleReadyInput;
            playerInput.OnPause -= HandlePauseInput;
        }
        
        private void HandleInteractionInput(object sender, EventArgs e) => player.TryInteract();
        
        private void HandleAlternateInteractionStartedInput(object sender, EventArgs e) => player.StartAlternateInteract();
        private void HandleAlternateInteractionInput(object sender, EventArgs e) => player.TryAlternateInteract();
        private void HandleAlternateInteractionCanceledInput(object sender, EventArgs e) => player.StopAlternateInteract();
        
        private void HandleReadyInput(object sender, EventArgs e) => player.OnReady();
        
        private void HandlePauseInput(object sender, EventArgs e) => gameManager.TogglePause();
    }
}
