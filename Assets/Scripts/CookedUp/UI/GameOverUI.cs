using CookedUp.Core;
using Shared;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace CookedUp.UI
{
    public class GameOverUI : MonoBehaviour {

        private DeliveryManager deliveryManager;
        private GameManager gameManager;
        [FormerlySerializedAs("recipesDeliveredText")] [SerializeField] private TextMeshProUGUI recipesDeliveredCountText;
        [SerializeField] private TextMeshProUGUI recipesDeliveredValueText;
        
        private void Start() {
            deliveryManager = DeliveryManager.Instance;
            gameManager = GameManager.Instance;
            gameManager.OnGameStateChanged += OnGameStateChanged;
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
            
            deliveryManager = DeliveryManager.Instance;
            recipesDeliveredCountText.text = deliveryManager.SuccessfulDeliveriesCount.ToString();
            recipesDeliveredValueText.text = deliveryManager.SuccessfulDeliveriesValue.ToString("F0");
            gameObject.SetActive(true);
        }
    
        private void Hide() {
            gameObject.SetActive(false);
        }
    
        private void OnDestroy() {
            gameManager.OnGameStateChanged -= OnGameStateChanged;
        }
    }
}
