using System;
using System.Collections.Generic;
using CookedUp.Core.Players;
using CookedUp.UI;
using Shared;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUI : MonoBehaviour {
    [SerializeField] private PlayersConfiguration playersConfiguration;

    
    [SerializeField] private Transform playerConfigurationUIsParent;
    [SerializeField] private PlayerConfigurationUI[] playerConfigurationUIs;

    [SerializeField] private Button startGameButton;
    [SerializeField] private Button backButton;

    public event EventHandler OnHide;
    
    private void Start() {
        playersConfiguration.Players = new List<PlayerConfiguration>();
        foreach (var playerConfigurationUI in playerConfigurationUIs) {
            playerConfigurationUI.Init(playersConfiguration.PlayerColors);
            playerConfigurationUI.OnPlayerConfigurationCreated += OnPlayerConfigurationCreated;
            playerConfigurationUI.OnPlayerConfigurationRemoved += OnPlayerConfigurationRemoved;
        }
        playerConfigurationUIs[0].CreateHumanPlayer();
        
        startGameButton.onClick.AddListener(StartGame);
        backButton.onClick.AddListener(Hide);
        
        UpdateUI();
        Hide();
    }


    private void OnPlayerConfigurationCreated(object sender, PlayerConfiguration e) {
        playersConfiguration.Players.Add(e);
        if (e.IsHuman) {
            foreach (var playerConfigurationUI in playerConfigurationUIs) {
                playerConfigurationUI.SetCanCreateHumanPlayer(false);
            }
        }
        
        UpdateUI();
    }

    private void OnPlayerConfigurationRemoved(object sender, PlayerConfiguration e) {
        playersConfiguration.Players.Remove(e);
        
        if (e.IsHuman) {
            foreach (var playerConfigurationUI in playerConfigurationUIs) {
                playerConfigurationUI.SetCanCreateHumanPlayer(true);
            }
        }
        
        UpdateUI();
    }


    private void UpdateUI() {
        var hasCreatedPlayers = playersConfiguration.Players.Count > 0;
        startGameButton.interactable = hasCreatedPlayers;
    }

    
    private void StartGame() {
        SceneLoader.Load(SceneLoader.Scene.GameScene);
    }
    
    public void Show() {
        gameObject.SetActive(true);
        startGameButton.Select();
    }
    
    public void Hide() {
        gameObject.SetActive(false);
        OnHide?.Invoke(this, EventArgs.Empty);
    }
}
