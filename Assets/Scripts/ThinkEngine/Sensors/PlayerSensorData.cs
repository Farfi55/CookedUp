using System.Collections.Generic;
using KitchenObjects;
using KitchenObjects.Container;
using Players;
using UnityEngine;

namespace ThinkEngine.Sensors
{
    public class PlayerSensorData : MonoBehaviour
    {
        [SerializeField] private Player player;
        [SerializeField] private PlayerBot playerBot;
        
        
        [Header("Sensor Data")]
        public bool HasSelectedInteractable;
        public string SelectedInteractableName;
        

        private void Start() {
            player.OnSelectedInteractableChanged += OnSelectedInteractableChanged;
            UpdateInteractableData();
        }


        private void OnSelectedInteractableChanged(object sender, ValueChangedEvent<IInteractable> e) => UpdateInteractableData();

        private void UpdateInteractableData()
        {
            HasSelectedInteractable = player.HasSelectedInteractable();
            if (player.SelectedInteractable is MonoBehaviour interactable) {
                SelectedInteractableName = interactable.GetType().Name;
                Debug.Log(SelectedInteractableName);
            }
            else
            {
                SelectedInteractableName = "";
            }
        }
    }
}
