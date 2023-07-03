using System;
using System.Collections;
using System.Collections.Generic;
using Counters;
using Extensions;
using Players;
using UnityEngine;

public class SoundManager : MonoBehaviour {
    public static SoundManager Instance { get; private set; }

    [SerializeField] private AudioClipRefsSO audioClipRefsSo;

    private Transform cameraTransform;


    private void Awake() {
        if (Instance == null) {
            Instance = this;
        }
        else {
            Debug.LogError("Multiple SoundManagers in scene");
            Destroy(gameObject);
        }
    }

    private void Start() {
        if (Camera.main != null)
            cameraTransform = Camera.main.transform;
        else
            Debug.LogError("No main camera found");


        if (DeliveryManager.Instance != null) {
            DeliveryManager.Instance.OnRecipeSuccess += OnRecipeSuccess;
            DeliveryManager.Instance.OnRecipeFailed += OnRecipeFailed;
        }

        CuttingCounter.OnAnyChop += OnAnyChop;
        TrashCounter.OnAnyTrashed += OnAnyTrashed;
    }
    
    private void PlaySound(AudioClip audioClip, Vector3 position, float volume = 1f) {
        AudioSource.PlayClipAtPoint(audioClip, position, volume);
    }

    private void PlaySound(AudioClip[] audioClips, Vector3 position, float volume = 1f) {
        AudioSource.PlayClipAtPoint(audioClips.GetRandomElement(), position, volume);
    }



    private void OnAnyChop(object sender, Player e) {
        if (sender is CuttingCounter cuttingCounter)
            PlaySound(audioClipRefsSo.Chop, cuttingCounter.transform.position);
        else Debug.LogError("OnAnyCut sender is not CuttingCounter");
    }
    
    private void OnAnyTrashed(object sender, TrashedEvent e) {
        PlaySound(audioClipRefsSo.Trash, e.TrashCounter.transform.position);
    }

    private void OnRecipeSuccess(object sender, RecipeDeliveryEvent e) {
        PlaySound(audioClipRefsSo.DeliverySuccess, e.DeliveryCounter.transform.position);
    }

    private void OnRecipeFailed(object sender, RecipeDeliveryEvent e) {
        PlaySound(audioClipRefsSo.DeliveryFail, e.DeliveryCounter.transform.position);
    }


    public void PlayFootstepSound(Vector3 position) {
        PlaySound(audioClipRefsSo.Footstep, position);
    }

    public void PlayPickupSound(Vector3 position) {
        PlaySound(audioClipRefsSo.ObjectPickup, position);
    }
    
    public void PlayDropSound(Vector3 position) {
        PlaySound(audioClipRefsSo.ObjectDrop, position);
    }


    public void PlayWarningSound(Vector3 position) {
        PlaySound(audioClipRefsSo.Warning, position);
    }
}
