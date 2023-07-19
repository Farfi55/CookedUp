using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class GamePausedUI : MonoBehaviour
    {
        private GameManager gameManager;
        
        [SerializeField] private Button resumeButton;
        [SerializeField] private Button optionsButton;
        [SerializeField] private Button mainMenuButton;

        [SerializeField] private OptionsUI optionsUI;
        

        private void Awake() {
            resumeButton.onClick.AddListener(() => {
                gameManager.TogglePause();
            });
            
            optionsButton.onClick.AddListener(() => {
                optionsUI.Show();
                Hide();
            });
            
            mainMenuButton.onClick.AddListener(() => {
                if(gameManager.IsGamePaused)
                    gameManager.TogglePause();
                SceneLoader.Load(SceneLoader.Scene.MainMenuScene);
            });
        }

        void Start()
        {
            gameManager = GameManager.Instance;
            gameManager.OnGamePauseStateChanged += OnGamePauseStateChanged;

            optionsUI.OnHide += (_, _) => Show();
            
            
            if (gameManager.IsGamePaused)
                Show();
            else Hide();
        }

        private void OnGamePauseStateChanged(object sender, ValueChangedEvent<GamePauseState> e) {
            if (e.NewValue == GamePauseState.Paused) {
                Show();
            }
            else {
                Hide();
            }
        }


        private void Show()
        {
            resumeButton.Select();
            gameObject.SetActive(true);
        }
        
        private void Hide()
        {
            gameObject.SetActive(false);
        }
        
        private void OnDestroy() {
            gameManager.OnGamePauseStateChanged -= OnGamePauseStateChanged;
        }
    }
}
