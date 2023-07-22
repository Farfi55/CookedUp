using System.Collections.Generic;
using KitchenObjects;
using KitchenObjects.Container;
using Players;
using UnityEngine;

namespace ThinkEngine.Sensors
{
    public class PlayerBotSensorData : MonoBehaviour
    {
        private GridManager gridManager;
        [SerializeField] private Player player;
        [SerializeField] private PlayerBot playerBot;


        public int playerID;
        public Vector2Int pos;

        public bool hasSelectedInteractable;
        public string selectedInteractableName;
        

        private void Start() {
            gridManager = GridManager.Instance;
            player.OnSelectedInteractableChanged += OnSelectedInteractableChanged;
            playerID = player.GetInstanceID();
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

        private void Update() {
            pos = gridManager.GetGridPosition(player.transform.position);
        }

    }
}
