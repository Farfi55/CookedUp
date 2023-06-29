using System;
using System.Collections;
using System.Collections.Generic;
using Extensions;
using UnityEngine;

public class SoundManager : MonoBehaviour {
    [SerializeField] private AudioClipRefsSO audioClipRefsSo;

    private Transform cameraTransform;


    private void Start() {
        if (Camera.main != null)
            cameraTransform = Camera.main.transform;
        else
            Debug.LogError("No main camera found");


        if (DeliveryManager.Instance != null) {
            DeliveryManager.Instance.OnRecipeSuccess += OnRecipeSuccess;
            DeliveryManager.Instance.OnRecipeFailed += OnRecipeFailed;
        }
    }

    private void OnRecipeSuccess(object sender, RecipeDeliveryEvent e) {
        PlaySound(audioClipRefsSo.DeliverySuccess, e.DeliveryCounter.transform.position);
    }

    private void OnRecipeFailed(object sender, RecipeDeliveryEvent e) {
        PlaySound(audioClipRefsSo.DeliveryFail, e.DeliveryCounter.transform.position);
    }


    private void PlaySound(AudioClip audioClip, Vector3 position, float volume = 1f) {
        AudioSource.PlayClipAtPoint(audioClip, position, volume);
    }

    private void PlaySound(AudioClip[] audioClips, Vector3 position, float volume = 1f) {
        AudioSource.PlayClipAtPoint(audioClips.GetRandomElement(), position, volume);
    }
}
