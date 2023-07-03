using System;
using KitchenObjects.ScriptableObjects;
using UnityEngine;

namespace Counters.Sound
{
    public class StoveCounterSound : MonoBehaviour {
        [SerializeField] private StoveCounter stoveCounter;
        [SerializeField] private AudioSource audioSource;

        private void Start() {
            stoveCounter.OnRecipeChanged += OnRecipeChanged;
        }

        private void OnRecipeChanged(object sender, ValueChangedEvent<BaseRecipeSO> e) {
            if (stoveCounter.IsCooking()) {
                audioSource.Play();
                if(e.NewValue is CookingRecipeSO cookingRecipeSO && cookingRecipeSO.IsBurningRecipe) {
                    SoundManager.Instance.PlayWarningSound(stoveCounter.transform.position);
                }
            }
            else {
                audioSource.Stop();
            }
        }

        private void OnDestroy() {
            stoveCounter.OnRecipeChanged -= OnRecipeChanged;
        }
    }
}
