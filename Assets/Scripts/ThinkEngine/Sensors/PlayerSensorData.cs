using System;
using System.Collections.Generic;
using KitchenObjects;
using KitchenObjects.Container;
using KitchenObjects.ScriptableObjects;
using Players;
using UnityEngine;
using UnityEngine.Serialization;

namespace ThinkEngine.Sensors {
    public class PlayerSensorData : MonoBehaviour {
        private IDManager idManager;

        [SerializeField] private Player player;


        [Header("Sensor Data"), Header("Interactable")] 
        public bool HasSelectedInteractable;
        public string SelectedInteractableType;
        public int SelectedInteractableID;

        private void Start() {
            idManager = IDManager.Instance;
            player.OnSelectedInteractableChanged += OnSelectedInteractableChanged;
            UpdateInteractableData();
        }

        private void OnSelectedInteractableChanged(object sender, ValueChangedEvent<IInteractable> e) =>
            UpdateInteractableData();

        private void UpdateInteractableData() {
            HasSelectedInteractable = player.HasSelectedInteractable();
            if (player.SelectedInteractable is MonoBehaviour interactable) {
                SelectedInteractableType = interactable.GetType().Name;
                SelectedInteractableID = idManager.GetID(interactable.gameObject);
            } else {
                SelectedInteractableType = "";
                SelectedInteractableID = 0;
            }
        }
    }
}
