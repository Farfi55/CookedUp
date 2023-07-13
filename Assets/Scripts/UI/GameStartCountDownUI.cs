using System;
using TMPro;
using UnityEngine;

namespace UI
{
    public class GameStartCountDownUI : MonoBehaviour
    {
    
        [SerializeField] private TextMeshProUGUI countDownText;
    
        private void Start() {
            GameManager.Instance.OnGameStateChanged += OnGameStateChanged;
            Hide();
        }

        private void OnGameStateChanged(object sender, ValueChangedEvent<GamePlayingState> e) {
            if (e.NewValue == GamePlayingState.Starting) {
                Show();
                GameManager.Instance.GameStateProgressTracker.OnProgressChanged += OnStartTimerChanged;
            }
            else {
                Hide();
                GameManager.Instance.GameStateProgressTracker.OnProgressChanged -= OnStartTimerChanged;
            }
        }

        private void OnStartTimerChanged(object sender, ValueChangedEvent<double> e) {
            var pt = GameManager.Instance.GameStateProgressTracker;
            var timerText = Math.Ceiling(pt.GetWorkRemaining()).ToString();
            countDownText.text = timerText;
        }


        private void Show() {
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
