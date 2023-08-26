using System;
using CookedUp.Core.Players;
using TMPro;
using UnityEngine;

namespace CookedUp.UI {
    public class PlayerDebugUI : MonoBehaviour {
        [SerializeField] private PlayerBot playerBot;

        [SerializeField] private TextMeshProUGUI stateNameText;
        
        private void Start() {
            playerBot.OnStateNameChanged += OnStateNameChanged;
            stateNameText.text = playerBot.StateName;
        }

        private void OnDestroy() {
            playerBot.OnStateNameChanged -= OnStateNameChanged;
        }

        private void OnStateNameChanged(object sender, EventArgs e) {
            stateNameText.text = playerBot.StateName;
        }
    }
}
