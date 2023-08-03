using KitchenObjects;
using KitchenObjects.ScriptableObjects;
using UnityEngine;

namespace Players
{
    public class PlayerBotVisual : MonoBehaviour {
        [SerializeField] private KitchenObjectPlayerIndicator kitchenObjectIndicatorPrefab;
        [SerializeField] private Player player;
        [SerializeField] private PlayerBot playerBot;


        private void Start() {
            playerBot.OnPlateChanged += OnPlateChanged;
            playerBot.OnRecipeChanged += OnRecipeChanged;
        }

        private void OnRecipeChanged(object sender, ValueChangedEvent<CompleteRecipeSO> e) {
        }

        private void OnPlateChanged(object sender, ValueChangedEvent<PlateKitchenObject> e) {
            if (e.OldValue != null) {
                var indicators = e.OldValue.gameObject.GetComponentsInChildren<KitchenObjectPlayerIndicator>();
                foreach (var indicator in indicators) {
                    if (indicator.Player == player)
                        indicator.DestroySelf();
                }
            }

            var newKitchenObject = e.NewValue;
            if (newKitchenObject == null)
                return;

            var koIndicator = newKitchenObject.GetComponentInChildren<KitchenObjectPlayerIndicator>();
            if (koIndicator == null) 
                koIndicator = Instantiate(kitchenObjectIndicatorPrefab, newKitchenObject.transform);
        
            koIndicator.Setup(player, newKitchenObject);
        }
    }
}
