using Shared;
using UnityEngine;

namespace CookedUp.Core
{
    public class ShowOnGameState : MonoBehaviour {
        [SerializeField] private GamePlayingState gameState;

        private void Start() {
            GameManager.Instance.OnGameStateChanged += OnGameStateChanged;
            UpdateVisibility(GameManager.Instance.GameState);
        }

        private void OnGameStateChanged(object sender, ValueChangedEvent<GamePlayingState> e) {
            UpdateVisibility(e.NewValue);
        }

        private void UpdateVisibility(GamePlayingState currentState) {
            if (currentState == gameState)
                Show();
            else
                Hide();
        }

        private void Show() => gameObject.SetActive(true);

        private void Hide() => gameObject.SetActive(false);
    }
}
