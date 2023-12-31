using System;
using CookedUp.Core;
using Shared;
using TMPro;
using UnityEngine;

namespace CookedUp.UI
{
    public class PlayingClockUI : MonoBehaviour {
        private SoundManager soundManager;
        private GameManager gameManager;

        [SerializeField] private TextMeshProUGUI clockText;

        float lastTickTime = 0f;

        private void Start() {
            gameManager = GameManager.Instance;
            soundManager = SoundManager.Instance;

            gameManager.OnGameStateChanged += OnGameStateChanged;
            UpdateVisibility(gameManager.GameState);
        }

        private void OnDestroy() {
            gameManager.OnGameStateChanged -= OnGameStateChanged;
            gameManager.GameStateProgressTracker.OnProgressChanged -= OnGameProgressChanged;
        }

        private void OnGameStateChanged(object sender, ValueChangedEvent<GamePlayingState> e) {
            if (e.NewValue == GamePlayingState.Playing) {
                gameManager.GameStateProgressTracker.OnProgressChanged += OnGameProgressChanged;
            }
            else {
                gameManager.GameStateProgressTracker.OnProgressChanged -= OnGameProgressChanged;
            }
        
            UpdateVisibility(e.NewValue);
        }

        private void OnGameProgressChanged(object sender, ValueChangedEvent<double> e) {
            float progress = (float)e.NewValue;
            float timeLeft = (float)gameManager.GameStateProgressTracker.GetWorkRemaining();
            var time = TimeSpan.FromSeconds(timeLeft);
            clockText.text = time.ToString(@"m\:ss");
            
            if (progress > 0.5f && Time.time - lastTickTime > 1f) {
                lastTickTime = Time.time;
                var volumeMultiplier = Mathf.Pow(2*(progress - 0.5f), 3);
                soundManager.PlayClockTickSound(volumeMultiplier);
            }
        }
    
        private void UpdateVisibility(GamePlayingState currentState) {
            if (currentState == GamePlayingState.Playing)
                Show();
            else
                Hide();
        }

        private void Show() => gameObject.SetActive(true);

        private void Hide() => gameObject.SetActive(false);
    }
}
