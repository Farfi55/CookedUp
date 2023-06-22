using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(IInteractable))]
public class SelectedVisual : MonoBehaviour
{
    [SerializeField]
    private GameObject selectedVisualObj;
    [SerializeField] private bool hideDefaulVisual = false;
    [SerializeField] private GameObject defaultVisualObj;

    private void Start() {
        GetComponent<IInteractable>().OnSelectedChanged += OnSelectedChanged;
        selectedVisualObj.SetActive(false);
    }

    private void OnSelectedChanged(object sender, SelectionChangedEvent e)
    {
        SetSelected(e.IsSelected);
    }

    public void SetSelected(bool selected) {
        selectedVisualObj.SetActive(selected);
        
        if(hideDefaulVisual && defaultVisualObj != null) {
            defaultVisualObj.SetActive(!selected);
        }
    }


}
