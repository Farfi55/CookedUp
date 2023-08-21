using CookedUp.Core.KitchenObjects;
using Shared;
using UnityEngine;

namespace CookedUp.Core.Counters.Sound
{
    public class StoveCounterSound : MonoBehaviour {
        private SoundManager soundManager;
        
        [SerializeField] private StoveCounter stoveCounter;
        [SerializeField] private AudioSource audioSource;

        [SerializeField] private float delayBetweenWarningSounds = 0.25f;
        private float remainingTimeToWarningSound = 0f;
        private bool isBurningRecipe = false;
        
        private void Start() {
            soundManager = SoundManager.Instance;
            stoveCounter.OnRecipeChanged += OnRecipeChanged;
        }

        private void OnRecipeChanged(object sender, ValueChangedEvent<BaseRecipeSO> e) {
            isBurningRecipe = false;
            if (stoveCounter.IsCooking()) {
                audioSource.Play();
                if(e.NewValue is CookingRecipeSO cookingRecipeSO) {
                    isBurningRecipe = cookingRecipeSO.IsBurningRecipe;
                    remainingTimeToWarningSound = 0f;
                }
            }
            else {
                audioSource.Stop();
            }
        }
        
        private void Update() {
            if (!isBurningRecipe) return;
            
            remainingTimeToWarningSound -= Time.deltaTime;
            if (remainingTimeToWarningSound <= 0f) {
                soundManager.PlayWarningSound(stoveCounter.transform.position);
                remainingTimeToWarningSound = delayBetweenWarningSounds;
            }
        }

        private void OnDestroy() {
            stoveCounter.OnRecipeChanged -= OnRecipeChanged;
        }
    }
}
