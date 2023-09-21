using System;
using System.Collections.Generic;
using UnityEngine;

namespace CookedUp.Core.Players {
    [CreateAssetMenu(fileName = "Players Config", menuName = "CookedUp/PLayers Config", order = 30)]
    public class PlayersConfiguration : ScriptableObject {
        
        
        public List<PlayerConfiguration> Players = new();
        
        public PlayerColorSO[] PlayerColors => playerColors;
        [SerializeField] private PlayerColorSO[] playerColors;
        [SerializeField] private Player humanPlayerPrefab;
        [SerializeField] private Player botPlayerPrefab;
        
        private void OnEnable() => hideFlags = HideFlags.DontUnloadUnusedAsset;
        
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

        public override string ToString() {
            var sb = new System.Text.StringBuilder();
            sb.AppendLine($"{Players.Count} Players:");
            for (var index = 0; index < Players.Count; index++) {
                var playerConfiguration = Players[index];
                sb.AppendLine($"[{index}] {playerConfiguration}");
            }

            return sb.ToString();
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
        
        public override string ToString() {
            return $"{nameof(Color)}: {Color.DisplayName}, {nameof(IsBot)}: {IsBot}";
        }
        
    }
    
}
