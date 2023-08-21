using Shared;
using UnityEngine;
using UnityEngine.UI;

namespace CookedUp.UI
{
    public class MainMenuUI : MonoBehaviour {
        [SerializeField] private Button playButton;
        [SerializeField] private Button quitButton;

        private void Awake() {
            playButton.onClick.AddListener(OnPlayButtonClicked);
            quitButton.onClick.AddListener(OnQuitButtonClicked);
        }

        private void Start() {
            playButton.Select();
        }


        private void OnPlayButtonClicked() {
            SceneLoader.Load(SceneLoader.Scene.GameScene);
        }

        private void OnQuitButtonClicked() {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
        }
    }
}
