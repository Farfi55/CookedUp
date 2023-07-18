using System;
using System.Collections.Generic;
using Extensions;
using Unity.VisualScripting.Dependencies.NCalc;
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

        private void OnEnable() {
            Player.OnAnyPlayerSpawned += OnPlayerSpawned;
            Player.OnAnyPlayerDestroyed += OnPlayerDestroyed;
        }



        private void OnDisable() {
            Player.OnAnyPlayerSpawned -= OnPlayerSpawned;
            Player.OnAnyPlayerDestroyed -= OnPlayerDestroyed;
        }
        
        private void OnPlayerSpawned(object sender, Player player) => AddPlayer(player);
        
        private void OnPlayerDestroyed(object sender, Player player) => RemovePlayer(player);

        public Player GetPlayer(int index) {
            return players[index];
        }

        public Player GetRandomPlayer() {
            return players.GetRandomElement();
        }

        public void AddPlayer(Player player) {
            if(players.Contains(player)) return;
            
            players.Add(player);
            player.OnPlayerReady += PlayersReady;
        }


        private void RemovePlayer(Player player) {
            players.Remove(player);
            playersReady.Remove(player);
            player.OnPlayerReady -= PlayersReady;
        }


        private void TogglePlayerReady(Player player) {
            if (playersReady.Contains(player))
                SetPlayerNotReady(player);
            else SetPlayerReady(player);
        }

        private void PlayersReady(object sender, EventArgs eventArgs) {
            TogglePlayerReady(sender as Player);
        }


        private void SetPlayerReady(Player player) {
            playersReady.Add(player);
            OnPlayersReadyChanged?.Invoke(this, EventArgs.Empty);
        }

        private void SetPlayerNotReady(Player player) {
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
