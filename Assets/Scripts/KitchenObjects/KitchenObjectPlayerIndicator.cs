using System;
using KitchenObjects.Container;
using Players;
using UnityEngine;

namespace KitchenObjects
{
    public class KitchenObjectPlayerIndicator : MonoBehaviour
    {
    
        public Player Player { get; private set; } 
        private PlayerVisual playerVisual;
        public KitchenObject KitchenObject { get; private set; }
    
        [SerializeField] private SpriteRenderer spriteRenderer;
    
    
    
        public void Setup(Player player, KitchenObject kitchenObject) {
            var isFirstSetup = Player == null;
            
            Player = player;
            KitchenObject = kitchenObject;
            playerVisual = player.GetComponent<PlayerVisual>();
            spriteRenderer.color = playerVisual.PlayerColorSO.Color;

            DestroyOthers(); 
            
            Show();

            if (isFirstSetup) {
                KitchenObject.OnContainerChanged += OnContainerChanged;
                KitchenObject.OnDestroyed += OnKitchenObjectDestroyed;
            }
        }

        private void OnContainerChanged(object sender, ValueChangedEvent<KitchenObjectsContainer> e) {
            if (e.NewValue == null) {
                Hide();
                return;
            }
        
            if (e.NewValue.TryGetComponent(out PlateKitchenObject plateKitchenObject)) {
                Hide();
            }
            else {
                Show();
            }   
        }
    
    
        private void Show() {
            spriteRenderer.enabled = true;
        }
    
        private void Hide() {
            spriteRenderer.enabled = false;
        }
    
        private void OnKitchenObjectDestroyed(object sender, EventArgs e) => Destroy(gameObject);
    
        public void DestroySelf() {
            KitchenObject.OnContainerChanged -= OnContainerChanged;
            KitchenObject.OnDestroyed -= OnKitchenObjectDestroyed;
            Destroy(gameObject);
        }
    
        public void DestroyOthers()
        {
            foreach (var kitchenObjectPlayerIndicator in
                     KitchenObject.GetComponentsInChildren<KitchenObjectPlayerIndicator>())
            {
                if (kitchenObjectPlayerIndicator != this)
                    kitchenObjectPlayerIndicator.DestroySelf();
            }
        }
    
    
    }
}
