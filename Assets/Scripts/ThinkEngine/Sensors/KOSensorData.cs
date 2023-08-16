using System;
using KitchenObjects;
using KitchenObjects.Container;
using Players;
using UnityEngine;

namespace ThinkEngine.Sensors {
    public class KOSensorData : MonoBehaviour {
        
        private IDManager idManager;
        [SerializeField] private KitchenObject kitchenObjectTarget;
        [SerializeField] private KitchenObjectPlayer kitchenObjectPlayer;

        [Header("Sensor Data")] 
        public bool HasOwnerContainer = true;
        public int OwnerContainerID;

        public bool HasPlayer;
        public int PlayerID;
        
        private void Start() {
            idManager = IDManager.Instance;
            kitchenObjectTarget.OnContainerChanged += OnContainerChanged;
            kitchenObjectPlayer.OnPlayerChanged += OnPlayerChanged;
            
            UpdateContainerData();
            UpdatePlayerData();
        }

      

        private void OnContainerChanged(object sender, ValueChangedEvent<KitchenObjectsContainer> e) => UpdateContainerData();

        private void UpdateContainerData() {
            if (kitchenObjectTarget.Container != null)
                OwnerContainerID = idManager.GetID(kitchenObjectTarget.Container.gameObject);
            else
                OwnerContainerID = 0;
        }
        
        private void OnPlayerChanged(object sender, ValueChangedEvent<Player> e) => UpdatePlayerData();

        private void UpdatePlayerData() {
            HasPlayer = kitchenObjectPlayer.HasPlayer;
            
            PlayerID = HasPlayer ? idManager.GetID(kitchenObjectPlayer.Player.gameObject) : 0;
        }
        
        private void OnDestroy() {
            if (kitchenObjectTarget != null) {
                kitchenObjectTarget.OnContainerChanged -= OnContainerChanged;
            }
            if (kitchenObjectPlayer != null) {
                kitchenObjectPlayer.OnPlayerChanged -= OnPlayerChanged;
            }
        }
    }
}
