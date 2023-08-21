using System;
using KitchenObjects.Container;
using Players;
using UnityEngine;

namespace KitchenObjects {
    public class KitchenObjectPlayerIndicator : MonoBehaviour {
        public Player Player { get; private set; }
        private PlayerVisual playerVisual;

        [SerializeField] private KitchenObject kitchenObject;
        [SerializeField] private KitchenObjectPlayer koPlayer;

        [SerializeField] private bool showWhenHeld = false;
        [SerializeField] private bool showWhenInPlate = false;

        [SerializeField] private SpriteRenderer spriteRenderer;


        private void Awake() {
            koPlayer = GetComponentInParent<KitchenObjectPlayer>();
        }

        private void Start() {
            koPlayer.OnPlayerChanged += OnPlayerChanged;
            kitchenObject.OnContainerChanged += OnContainerChanged;

            UpdatePlayer();
            UpdateContainer();
        }

        private void OnDestroy() {
            koPlayer.OnPlayerChanged -= OnPlayerChanged;
            kitchenObject.OnContainerChanged -= OnContainerChanged;
        }

        private void OnPlayerChanged(object sender, ValueChangedEvent<Player> e) => UpdatePlayer();

        private void UpdatePlayer() {
            Player = koPlayer.Player;
            if (Player == null) {
                Hide();
                return;
            }
            playerVisual = koPlayer.Player.GetComponent<PlayerVisual>();
            if (playerVisual == null) {
                Debug.LogWarning($"Player {Player} does not have a PlayerVisual component");
            }
            else {
                spriteRenderer.color = playerVisual.PlayerColorSO.Color;
                UpdateContainer();
            }
        }

        private void OnContainerChanged(object sender, ValueChangedEvent<KitchenObjectsContainer> e) => UpdateContainer();

        private void UpdateContainer()
        {
            if (kitchenObject.Container == null || Player == null)
            {
                Hide();
                return;
            }

            if (kitchenObject.Container.TryGetComponent(out PlateKitchenObject _))
                SetVisible(showWhenInPlate);
            else if (koPlayer.IsCurrentlyHeldByPlayer())
                SetVisible(showWhenHeld);
            else
                Show();
        }

        private void SetVisible(bool visible) {
            spriteRenderer.enabled = visible;
        }

        private void Show() => SetVisible(true);

        private void Hide() => SetVisible(false);
    }
}
