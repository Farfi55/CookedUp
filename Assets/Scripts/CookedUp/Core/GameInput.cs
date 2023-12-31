using CookedUp.Core.Players;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CookedUp.Core
{
    public class GameInput : MonoBehaviour
    {
        private GameManager gameManager;
        private PlayerInputActions inputActions;
    

        private void Start() {
            gameManager = GameManager.Instance;
            inputActions = new PlayerInputActions();
            inputActions.Player.Enable();
        
            inputActions.Player.Pause.performed += PausePerformed;
        }

        private void OnDestroy() {
            inputActions.Player.Pause.performed -= PausePerformed;
            inputActions.Dispose();
        }

        private void PausePerformed(InputAction.CallbackContext obj) {
            Debug.Log("Pause");
            gameManager.TogglePause();
        }
    }
}
