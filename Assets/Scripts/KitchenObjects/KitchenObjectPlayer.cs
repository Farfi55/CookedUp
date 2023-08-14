using System;
using KitchenObjects.Container;
using Players;
using UnityEngine;

namespace KitchenObjects {
    public class KitchenObjectPlayer : MonoBehaviour {

        private KitchenObject ko;
        public Player Player { get; private set; }
        
        public bool HasPlayer => Player != null;
        
        public bool IsCurrentlyHeldByPlayer() => HasPlayer && Player.CurrentKitchenObject == ko;
        
        public event EventHandler<ValueChangedEvent<Player>> OnPlayerChanged;

        private void Awake() {
            ko = GetComponent<KitchenObject>();
        }

        private void Start() {
            ko.OnContainerChanged += OnContainerChanged;
            ko.OnDestroyed += OnDestroyed;
            Player.OnAnyPlayerDestroyed += OnPlayerDestroyed;
            UpdatePlayer();
        }
        
        private void OnDestroyed(object sender, EventArgs eventArgs) {
            Player.OnAnyPlayerDestroyed -= OnPlayerDestroyed;
        }

        private void OnPlayerDestroyed(object sender, Player e) {
            if(Player == e) {
                SetPlayer(null);
            }
        }

        private void OnContainerChanged(object sender, ValueChangedEvent<KitchenObjectsContainer> e) {
            UpdatePlayer();
        }

        private void UpdatePlayer() {
            if(ko.IsInContainer && ko.Container.TryGetComponent(out Player player)) {
                SetPlayer(player);
            }
        }

        public void SetPlayer(Player player) {
            var oldPlayer = Player;
            if(oldPlayer ==  player) 
                return;
            
            Player = player;
            OnPlayerChanged?.Invoke(this, new(oldPlayer, player));
        }

    }
}
