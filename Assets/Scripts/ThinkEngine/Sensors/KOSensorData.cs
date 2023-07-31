using System;
using KitchenObjects;
using KitchenObjects.Container;
using UnityEngine;

namespace ThinkEngine.Sensors {
    public class KOSensorData : MonoBehaviour {
        
        private IDManager idManager;
        [SerializeField] private KitchenObject kitchenObjectTarget;

        [Header("Sensor Data")] public int OwnerContainerID;
        public bool IsInContainer = true;


        private void Start() {
            idManager = IDManager.Instance;
            kitchenObjectTarget.OnContainerChanged += OnContainerChanged;
            
            UpdateContainerData();
        }

        private void OnContainerChanged(object sender, ValueChangedEvent<KitchenObjectsContainer> e) => UpdateContainerData();

        private void UpdateContainerData() {
            if (kitchenObjectTarget.Container != null)
                OwnerContainerID = idManager.GetID(kitchenObjectTarget.Container.gameObject);
            else
                OwnerContainerID = 0;
        }
        
        private void OnDestroy() {
            kitchenObjectTarget.OnContainerChanged -= OnContainerChanged;
        }
    }
}
