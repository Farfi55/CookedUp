using System;
using System.Collections.Generic;
using UnityEngine;

namespace CookedUp.Core.Players {
    [CreateAssetMenu(fileName = "Players Config", menuName = "CookedUp/PLayers Config", order = 30)]
    public class PlayersConfiguration : ScriptableObject {
        
        [NonSerialized]
        public List<PlayerConfiguration> Players = new();
        
        public PlayerColorSO[] PlayerColors => playerColors;
        [SerializeField] private PlayerColorSO[] playerColors;
        [SerializeField] private Player humanPlayerPrefab;
        [SerializeField] private Player botPlayerPrefab;
        
        
        public Player CreatePlayer(PlayerConfiguration playerConfiguration) {
            var prefab = playerConfiguration.IsHuman ? humanPlayerPrefab : botPlayerPrefab;
            var player = Instantiate(prefab);
            
            var playerName = $"Player_{playerConfiguration.Color.DisplayName}";
            if (playerConfiguration.IsBot) playerName += "_Bot";
            player.name = playerName;
            
            player.GetComponent<PlayerVisual>().SetColor(playerConfiguration.Color);
            
            return player;
        }
        
        
        public PlayerColorSO GetRandomColor() {
            return playerColors[UnityEngine.Random.Range(0, playerColors.Length)];
        }
        
        public PlayerColorSO GetRandomUnusedColor() {
            var unusedColors = new List<PlayerColorSO>(playerColors);
            foreach (var playerConfiguration in Players) {
                unusedColors.Remove(playerConfiguration.Color);
            }
            return unusedColors[UnityEngine.Random.Range(0, unusedColors.Count)];
        }
        
    }
    
    
    [Serializable]
    public class PlayerConfiguration {
        public PlayerColorSO Color;
        public bool IsBot;
        public bool IsHuman => !IsBot;

        public PlayerConfiguration(PlayerColorSO color, bool isBot) {
            Color = color;
            IsBot = isBot;
        }
        
    }
    
}
