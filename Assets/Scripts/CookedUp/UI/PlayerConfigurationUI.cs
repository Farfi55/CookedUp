using System;
using CookedUp.Core.Players;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace CookedUp.UI
{
    public class PlayerConfigurationUI : MonoBehaviour {
        private PlayerConfiguration playerConfiguration;
        
        [SerializeField] private TextMeshProUGUI playerTypeText;
        [Space]
        [SerializeField] private PlayerVisual playerVisual;
        [Space]
        [SerializeField] private Transform playerColorsSection;
        [SerializeField] private Transform playerColorUIContainer;
        [SerializeField] private PlayerColorUI playerColorUITemplate;
        [Space]
        [SerializeField] private Button addBotButton;
        [SerializeField] private Button addPlayerButton;
        [SerializeField] private Button removeButton;
        [Space]
        [SerializeField] private Material PlayerNotCreatedMaterial;
        
        public bool HasCreatedPlayer => playerConfiguration != null;
        
        private PlayerColorUI[] playerColorUIs;
        private PlayerColorUI selectedPlayerColorUI;

        
        public event EventHandler<PlayerConfiguration> OnPlayerConfigurationCreated;
        public event EventHandler<PlayerConfiguration> OnPlayerConfigurationRemoved;
        
        private bool CanShowCreateButtons = true;
        private bool CanCreateHumanPlayer = true;


        private void Start() {
            
            addBotButton.onClick.AddListener(CreateBot);
            addPlayerButton.onClick.AddListener(CreateHumanPlayer);
            removeButton.onClick.AddListener(RemovePlayer);
            
            playerColorUITemplate.gameObject.SetActive(false);
        }


        public void Init(PlayerColorSO[] playerColorsSOs) {
            UpdateUI();

            playerColorUIs = new PlayerColorUI[playerColorsSOs.Length];
            for (var i = 0; i < playerColorsSOs.Length; i++) {
                var playerColor = playerColorsSOs[i];
                var playerColorUI = Instantiate(playerColorUITemplate, playerColorUIContainer);
                playerColorUI.name = "PlayerColor" + playerColor.DisplayName;
                playerColorUI.Init(playerColor, SetSelectedColor);
                playerColorUI.gameObject.SetActive(true);
                playerColorUIs[i] = playerColorUI;
            }
        }

        private void SetSelectedColor(PlayerColorUI playerColorUI) {
            if(playerConfiguration == null) return;
            
            if(selectedPlayerColorUI != null)
                selectedPlayerColorUI.SetSelected(false);
            
            selectedPlayerColorUI = playerColorUI;
            selectedPlayerColorUI.SetSelected(true);
            var playerColorSO = selectedPlayerColorUI.playerColorSO;
            playerConfiguration.Color = playerColorSO;
            playerVisual.SetColor(playerColorSO);
        }
        
        public void SetCanShowCreateButtons(bool canShow) {
            CanShowCreateButtons = canShow;
            UpdateUI();
        }

        
        public void SetCanCreateHumanPlayer(bool canCreate) {
            CanCreateHumanPlayer = canCreate;
            UpdateUI();
        }
        
        private void UpdateUI() {
            addBotButton.gameObject.SetActive(!HasCreatedPlayer && CanShowCreateButtons);
            addPlayerButton.gameObject.SetActive(!HasCreatedPlayer && CanShowCreateButtons && CanCreateHumanPlayer);
            removeButton.gameObject.SetActive(HasCreatedPlayer);
            
            
            if (HasCreatedPlayer) {
                playerVisual.SetColor(playerConfiguration.Color);
                
                playerTypeText.text = playerConfiguration.IsBot ? "Bot" : "Player";
            }
            else {
                playerVisual.SetMaterial(PlayerNotCreatedMaterial);
                playerTypeText.text = "";
            }
            playerVisual.SetEyesActive(HasCreatedPlayer);
            
            playerColorsSection.gameObject.SetActive(HasCreatedPlayer);
        }
        
        public void CreateHumanPlayer() => CreatePlayer(false);
        public void CreateBot() => CreatePlayer(true);

        public void CreatePlayer(bool isBot) {
            if (HasCreatedPlayer) return;
            
            var colorUI = playerColorUIs[Random.Range(0, playerColorUIs.Length)];
            playerConfiguration = new PlayerConfiguration(colorUI.playerColorSO, isBot);
            UpdateUI();
            SetSelectedColor(colorUI);
            removeButton.Select();
            OnPlayerConfigurationCreated?.Invoke(this, playerConfiguration);
        }


        private void RemovePlayer() {
            var tmpPlayerConfiguration = playerConfiguration;
            
            playerConfiguration = null;
            UpdateUI();
            addBotButton.Select();
            OnPlayerConfigurationRemoved?.Invoke(this, tmpPlayerConfiguration);
        }

        
    }
}
