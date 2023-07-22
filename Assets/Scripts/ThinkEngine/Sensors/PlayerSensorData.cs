using System.Collections.Generic;
using KitchenObjects;
using KitchenObjects.Container;
using Players;
using UnityEngine;

namespace ThinkEngine.Sensors
{
    public class PlayerSensorData : MonoBehaviour
    {
        private GridManager gridManager;
        [SerializeField] private Player player;
        [SerializeField] private PlayerBot playerBot;
        
        public bool hasSelectedInteractable;
        public string selectedInteractableName;
        

        private void Start() {
            gridManager = GridManager.Instance;
            player.OnSelectedInteractableChanged += OnSelectedInteractableChanged;
            UpdateInteractableData();
        }


        private void OnSelectedInteractableChanged(object sender, ValueChangedEvent<IInteractable> e) => UpdateInteractableData();

        private void UpdateInteractableData()
        {
            hasSelectedInteractable = player.HasSelectedInteractable();
            if (player.SelectedInteractable is MonoBehaviour interactable) {
                selectedInteractableName = interactable.GetType().Name;
                Debug.Log(selectedInteractableName);
            }
            else
            {
                selectedInteractableName = "";
            }
        }
    }
}
