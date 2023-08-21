using CookedUp.Core.KitchenObjects;
using Shared;
using UnityEngine;

namespace CookedUp.Core.Counters.Visual
{
    public class StoveCounterVisual : MonoBehaviour {

        [SerializeField] private ParticleSystem SizzlingParticles;
        [SerializeField] private GameObject stoveOnVisual;


        private StoveCounter stoveCounter;

        private void Awake() {
            stoveCounter = GetComponentInParent<StoveCounter>();
            stoveCounter.OnRecipeChanged += OnRecipeChanged;
        }

        private void OnRecipeChanged(object sender, ValueChangedEvent<BaseRecipeSO> e) {
            var recipe = e.NewValue as CookingRecipeSO;
            if (recipe != null) {
                TurnOn();
            }
            else {
                TurnOff();
            }
        }

        private void TurnOff() {
            SizzlingParticles.gameObject.SetActive(false);
            SizzlingParticles.Stop();

            stoveOnVisual.SetActive(false);
        }

        private void TurnOn() {
            SizzlingParticles.gameObject.SetActive(true);
            SizzlingParticles.Play();

            stoveOnVisual.SetActive(true);
        }
    }
}
