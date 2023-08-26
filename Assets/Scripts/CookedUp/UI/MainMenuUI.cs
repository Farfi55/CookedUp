using Shared;
using UnityEngine;
using UnityEngine.UI;

namespace CookedUp.UI
{
    public class MainMenuUI : MonoBehaviour {
        [SerializeField] private Button playButton;
        [SerializeField] private Button quitButton;

        [SerializeField] private LobbyUI lobbyUI;
        
        
        private void Awake() {
            playButton.onClick.AddListener(OnPlayButtonClicked);
            quitButton.onClick.AddListener(OnQuitButtonClicked);
        }

        private void Start() {
            lobbyUI.OnHide += (_, _) => Show();
            Show();
        }

        private void Show() {
            gameObject.SetActive(true);
            playButton.Select();
        }

        private void Hide() => gameObject.SetActive(false);
        
        private void OnPlayButtonClicked() {
            Hide();
            lobbyUI.Show();
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
