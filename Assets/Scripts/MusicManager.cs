using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }
    
    private AudioSource audioSource;
    
    private const string PLAYER_PREFS_MUSIC_VOLUME = "MusicVolume";

    
    public float MusicVolume { get; private set; } = 0.3f;
    
    public event EventHandler<ValueChangedEvent<float>> OnMusicVolumeChanged;

    
    private void Awake() {
        if (Instance == null) {
            Instance = this;
        }
        else {
            Debug.LogError("Multiple MusicManagers in scene");
            Destroy(gameObject);
        }
       
        MusicVolume = PlayerPrefs.GetFloat(PLAYER_PREFS_MUSIC_VOLUME, MusicVolume);
        
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = MusicVolume;
        
    }


    public void ChangeMusicVolume(float value) {
        var oldMusicVolume = MusicVolume;
        MusicVolume = value;
        audioSource.volume = MusicVolume;
        PlayerPrefs.SetFloat(PLAYER_PREFS_MUSIC_VOLUME, MusicVolume);
        PlayerPrefs.Save();
        OnMusicVolumeChanged?.Invoke(this, new ValueChangedEvent<float>(oldMusicVolume, MusicVolume));
    }

    
}
