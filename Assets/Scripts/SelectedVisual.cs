using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Serialization;


public class SelectedVisual : MonoBehaviour {
    [SerializeField]
    private GameObject[] selectedVisualObjs;
    [FormerlySerializedAs("hideDefaulVisual")] [SerializeField] private bool hideDefaultVisual = false;
    [SerializeField] private GameObject defaultVisualObj;


    private void Start() {
        SetSelected(false);
        GetComponent<IInteractable>().OnSelectedChanged += (sender, e) => SetSelected(e.IsSelected);
    }

    public void SetSelected(bool selected) {
        foreach (var selectedVisualObj in selectedVisualObjs) {
            selectedVisualObj.SetActive(selected);
        }

        if (hideDefaultVisual && defaultVisualObj != null) {
            defaultVisualObj.SetActive(!selected);
        }
    }


}
