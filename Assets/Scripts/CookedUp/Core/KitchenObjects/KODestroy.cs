using Shared;
using UnityEngine;

namespace CookedUp.Core.KitchenObjects
{
    public class KODestroy : MonoBehaviour {
    
        private SoundManager soundManager;
        private GameManager gameManager;
        [SerializeField] private GameObject particlesPrefab;

        private void Start() {
            soundManager = SoundManager.Instance;
            gameManager = GameManager.Instance;
            if(gameManager)
                gameManager.OnGameStateChanged += OnGameStateChanged;
        }

        private void OnDestroy() {
            if(gameManager)
                gameManager.OnGameStateChanged -= OnGameStateChanged;
        }

        private void OnGameStateChanged(object sender, ValueChangedEvent<GamePlayingState> e) {
            if (e.NewValue == GamePlayingState.GameOver) {
                var kitchenObject = GetComponent<KitchenObject>();
                if (kitchenObject.IsVisible) {
                    PlaySound();
                    CreateParticles();
                }
                kitchenObject.DestroySelf();
            }
        }
    
        private void PlaySound() {
            if(soundManager != null)
                soundManager.PlayObjectDestroySound(transform.position);
        }

        private void CreateParticles() {
            Instantiate(particlesPrefab, transform.position, Quaternion.identity);
        }

    }
}
