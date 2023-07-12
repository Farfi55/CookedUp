using System;
using System.Collections.Generic;
using Extensions;
using UnityEngine;

namespace Players {
    public class PlayersManager : MonoBehaviour {
        public static PlayersManager Instance { get; private set; }
        
        public List<Player> Players => players;
        [SerializeField] private List<Player> players;
        
        public int PlayerCount => players.Count;

        private readonly HashSet<Player> playersReady = new();

        public event EventHandler OnPlayersReadyChanged;

        private void Awake() {
            if (Instance == null) {
                Instance = this;
            }
            else {
                Debug.LogError("Multiple PlayersManagers in scene");
                Destroy(gameObject);
            }
        }
        
        public Player GetPlayer(int index) {
            return players[index];
        }
        
        public Player GetRandomPlayer() {
            return players.GetRandomElement();
        }
        
        public void AddPlayer(Player player) {
            players.Add(player);
        }
        
        public void RemovePlayer(Player player) {
            players.Remove(player);
        }
        
        
        public void TogglePlayerReady(Player player) {
            if (playersReady.Contains(player))
                SetPlayerNotReady(player);
            else SetPlayerReady(player);
        }
        
        public void SetPlayerReady(Player player) {
            playersReady.Add(player);
            OnPlayersReadyChanged?.Invoke(this, EventArgs.Empty);
        }
        
        public void SetPlayerNotReady(Player player) {
            playersReady.Remove(player);
            OnPlayersReadyChanged?.Invoke(this, EventArgs.Empty);
        }
        
        public bool AreAllPlayersReady() {
            return playersReady.Count == players.Count;
        }
        
        public void ResetPlayersReady() {
            playersReady.Clear();
        }
        
        
        
    }
}
