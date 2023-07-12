using System;
using System.Collections;
using System.Collections.Generic;
using Players;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    private PlayersManager playersManager;
    
    [SerializeField] private ProgressTracker gameStateProgressTracker;

    [SerializeField] private float onPlayersReadyTime = 1f; 
    
    [SerializeField] private float timeToStart = 3f;
    
    [SerializeField] private float timeToPlay = 120f;
    
    public GamePlayingState GameState => gameState;
    private GamePlayingState gameState;

    private bool allPlayersReady = false;
    
    
    private void Awake() {
        if (Instance == null) {
            Instance = this;
        }
        else {
            Debug.LogError("Multiple GameManagers in scene");
            Destroy(gameObject);
        }
        
        gameState = GamePlayingState.Waiting;
    }
    
    private void Start() {
        playersManager = PlayersManager.Instance;
        playersManager.OnPlayersReadyChanged += OnPlayersReadyChanged;
        
        gameStateProgressTracker.OnProgressComplete += OnGameStateProgressComplete;
    }

    private void OnGameStateProgressComplete(object sender, EventArgs e) {
        NextGameState();
    }
    
    private void OnPlayersReadyChanged(object sender, EventArgs e) {
        allPlayersReady = playersManager.AreAllPlayersReady();
    }

    private void Update() {
        switch (gameState) {
            case GamePlayingState.Waiting:
                WaitingUpdate();
                break;
            case GamePlayingState.Starting:
                StaringUpdate();
                break;
            case GamePlayingState.Playing:
                PlayingUpdate();
                break;
            case GamePlayingState.GameOver:
                GameOverUpdate();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void SetState(GamePlayingState newState) {
        gameState = newState;
        
        switch (gameState) {
            case GamePlayingState.Waiting:
                gameStateProgressTracker.SetTotalWork(onPlayersReadyTime);
                break;
            case GamePlayingState.Starting:
                gameStateProgressTracker.SetTotalWork(timeToStart);
                break;
            case GamePlayingState.Playing:
                gameStateProgressTracker.SetTotalWork(timeToPlay);
                break;
            case GamePlayingState.GameOver:
                gameStateProgressTracker.ResetProgress();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void NextGameState() {
        var nextState = gameState switch {
            GamePlayingState.Waiting => GamePlayingState.Starting,
            GamePlayingState.Starting => GamePlayingState.Playing,
            GamePlayingState.Playing => GamePlayingState.GameOver,
            GamePlayingState.GameOver => throw new ArgumentOutOfRangeException(), 
            _ => throw new ArgumentOutOfRangeException()
        };   
        SetState(nextState);
    }

    private void PlayingUpdate() {
        AddTimeProgress();
    }

    private void WaitingUpdate() {
        if (allPlayersReady) {
            AddTimeProgress();
        }
        else if (gameStateProgressTracker.Progress > 0) {
            gameStateProgressTracker.RemoveWorkDone(Time.deltaTime * 2f);
        }
    }
    private void StaringUpdate() {
        AddTimeProgress();
    }

    private void GameOverUpdate() {
        
    }
    
    private void AddTimeProgress() {
        gameStateProgressTracker.AddWorkDone(Time.deltaTime);
    }
}

public enum GamePlayingState {
    Waiting,
    Starting,
    Playing,
    GameOver,
}
