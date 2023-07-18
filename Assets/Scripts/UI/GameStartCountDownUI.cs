using System;
using TMPro;
using UnityEngine;

namespace UI
{
    public class GameStartCountDownUI : MonoBehaviour
    {
        private GameManager gameManager;
    
        [SerializeField] private TextMeshProUGUI countDownText;

        private void Start() {
            gameManager = GameManager.Instance;
            gameManager.OnGameStateChanged += OnGameStateChanged;
            
            if(gameManager.GameState == GamePlayingState.Starting) {
                gameManager.GameStateProgressTracker.OnProgressChanged += OnStartTimerChanged;
            }
        }

        private void OnGameStateChanged(object sender, ValueChangedEvent<GamePlayingState> e) {
            if (e.NewValue == GamePlayingState.Starting) {
                gameManager.GameStateProgressTracker.OnProgressChanged += OnStartTimerChanged;
            }
            else {
                gameManager.GameStateProgressTracker.OnProgressChanged -= OnStartTimerChanged;
            }
        }

        private void OnStartTimerChanged(object sender, ValueChangedEvent<double> e) {
            var pt = gameManager.GameStateProgressTracker;
            var timerInt = (int)Math.Ceiling(pt.GetWorkRemaining());
            var timerText = timerInt.ToString();
            countDownText.text = timerText;
        }


        private void OnDestroy() {
            gameManager.OnGameStateChanged -= OnGameStateChanged;
        }
    }
}
