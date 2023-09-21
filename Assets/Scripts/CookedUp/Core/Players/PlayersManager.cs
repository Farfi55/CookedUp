using System;
using System.Collections.Generic;
using Shared.Extensions;
using UnityEngine;

namespace CookedUp.Core.Players {
    public class PlayersManager : MonoBehaviour {
        public static PlayersManager Instance { get; private set; }

        public List<Player> Players => players;
        [SerializeField] private List<Player> players = new();

        [SerializeField] private PlayersConfiguration playersConfiguration;
        
        [SerializeField] private Transform playersSpawnPoint;
        
        public int PlayerCount => players.Count;

        private readonly HashSet<Player> playersReady = new();

        [Header("Debug")]
        [SerializeField] private bool SpawnHumanPlayer = true;
        [SerializeField] private int BotsToSpawn = 1;
        
        public event EventHandler OnPlayersChanged; 
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


        private void Start() {
#if DEVELOPMENT_BUILD || UNITY_EDITOR
            if (playersConfiguration == null) {
                Debug.LogError("PlayersConfiguration is null");
                return;
            }
            Debug.Log($"PlayersConfiguration:\n{playersConfiguration}"); 
#endif
            
            if (playersConfiguration.Players.Count == 0) {
                if (SpawnHumanPlayer) {
                    var conf = new PlayerConfiguration(playersConfiguration.GetRandomUnusedColor(), false);
                    playersConfiguration.Players.Add(conf);
                }
                for (var i = 0; i < BotsToSpawn; i++) {
                    var conf = new PlayerConfiguration(playersConfiguration.GetRandomUnusedColor(), true);
                    playersConfiguration.Players.Add(conf);
                }
            }
            
            foreach (var playerConfiguration in playersConfiguration.Players) {
                var player = playersConfiguration.CreatePlayer(playerConfiguration);
                player.transform.position = playersSpawnPoint.position + Vector3.right * (players.Count * 2f);
                AddPlayer(player);
            }
        }

        private void OnPlayerSpawned(object sender, Player player) => AddPlayer(player);
        
        private void OnPlayerDestroyed(object sender, Player player) => RemovePlayer(player);

        public Player GetPlayer(int index = 0) {
            return players[index];
        }
        
        public Player GetPlayerByID(int id) {
            return players.Find(player => player.GetInstanceID() == id);
        }

        public Player GetRandomPlayer() {
            return players.GetRandomElement();
        }

        public void AddPlayer(Player player) {
            if(players.Contains(player)) return;
            
            players.Add(player);
            player.OnPlayerReady += PlayersReady;
            OnPlayersChanged?.Invoke(this, EventArgs.Empty);
        }


        private void RemovePlayer(Player player) {
            players.Remove(player);
            playersReady.Remove(player);
            player.OnPlayerReady -= PlayersReady;
            OnPlayersChanged?.Invoke(this, EventArgs.Empty);
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
