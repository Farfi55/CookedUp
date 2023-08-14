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
    private IInteractable interactable;


    private void Start() {
        interactable = GetComponent<IInteractable>();
        interactable.OnSelectedChanged += OnSelectedChanged;
        UpdateSelected();
    }
    
    private void OnDestroy() {
        interactable.OnSelectedChanged -= OnSelectedChanged;
    }

    private void OnSelectedChanged(object sender, SelectionChangedEvent e) => UpdateSelected();

    private void UpdateSelected() => SetSelected(interactable.IsSelected());

    public void SetSelected(bool selected) {
        foreach (var selectedVisualObj in selectedVisualObjs) {
            selectedVisualObj.SetActive(selected);
        }

        if (hideDefaultVisual && defaultVisualObj != null) {
            defaultVisualObj.SetActive(!selected);
        }
    }


}
