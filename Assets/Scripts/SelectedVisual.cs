using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;


public class SelectedVisual : MonoBehaviour {
    [SerializeField]
    private GameObject[] selectedVisualObjs;
    [SerializeField] private bool hideDefaulVisual = false;
    [SerializeField] private GameObject defaultVisualObj;


    private void Start() {
        SetSelected(false);
        GetComponent<IInteractable>().OnSelectedChanged += (sender, e) => SetSelected(e.IsSelected);
    }

    public void SetSelected(bool selected) {
        foreach (var selectedVisualObj in selectedVisualObjs) {
            selectedVisualObj.SetActive(selected);
        }

        if (hideDefaulVisual && defaultVisualObj != null) {
            defaultVisualObj.SetActive(!selected);
        }
    }


}
