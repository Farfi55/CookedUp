using TMPro;
using UnityEngine;

namespace UI
{
    public class GameOverUI : MonoBehaviour {

        [SerializeField] private TextMeshProUGUI recipesDeliveredText;
        
        private void Start() {
            GameManager.Instance.OnGameStateChanged += OnGameStateChanged;
            Hide();
        }

        private void OnGameStateChanged(object sender, ValueChangedEvent<GamePlayingState> e) {
            if (e.NewValue == GamePlayingState.GameOver) {
                Show();
            }
            else {
                Hide();
            }
        }

        private void Show() {
            recipesDeliveredText.text = DeliveryManager.Instance.SuccessfulDeliveriesCount.ToString();
            gameObject.SetActive(true);
        }
    
        private void Hide() {
            gameObject.SetActive(false);
        }
    
        private void OnDestroy() {
            GameManager.Instance.OnGameStateChanged -= OnGameStateChanged;
        }
    }
}
