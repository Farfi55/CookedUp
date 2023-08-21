using System;
using TMPro;
using UnityEngine;

namespace UI
{
    public class GameStartCountDownUI : MonoBehaviour
    {
        private GameManager gameManager;

        private Animator animator;
    
        [SerializeField] private TextMeshProUGUI countDownText;
        
        
        private readonly int numberPopupHash = Animator.StringToHash("NumberPopup");

        private void Awake() {
            animator = GetComponent<Animator>();
        }

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
            var oldTimerText = countDownText.text;
            countDownText.text = timerText;
            
            if(timerText != oldTimerText) {
                animator.SetTrigger(numberPopupHash);
                if(timerInt > 0)
                    SoundManager.Instance.PlayCountdownSound();
                else SoundManager.Instance.PlayCountdownEndSound();
            }
        }


        private void OnDestroy() {
            gameManager.OnGameStateChanged -= OnGameStateChanged;
        }
    }
}
