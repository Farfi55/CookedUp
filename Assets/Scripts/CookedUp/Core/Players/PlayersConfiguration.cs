using System;
using System.Collections.Generic;
using UnityEngine;

namespace CookedUp.Core.Players {
    [CreateAssetMenu(fileName = "Players Config", menuName = "CookedUp/PLayers Config", order = 30)]
    public class PlayersConfiguration : ScriptableObject {
        public List<PlayerConfiguration> Players;
    }
    
    
    [Serializable]
    public struct PlayerConfiguration {
        public PlayerColorSO Color;
        public bool IsBot;
        public bool IsHuman => !IsBot;

        public PlayerConfiguration(PlayerColorSO color, bool isBot) {
            Color = color;
            IsBot = isBot;
        }
        
    }
    
}
