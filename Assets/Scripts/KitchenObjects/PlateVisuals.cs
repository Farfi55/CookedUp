using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlateKitchenObject))]
public class PlateVisuals : MonoBehaviour {

    private PlateKitchenObject plateKitchenObject;
    private KitchenObjectsContainerVisual containerVisual;

    private FinalPlateArrangement plateArrangementPrefab;
    private FinalPlateArrangement plateArrangementInstance;


    [SerializeField] private Transform plateArrangementParent;

    private void Awake() {
        plateKitchenObject = GetComponent<PlateKitchenObject>();
        containerVisual = GetComponent<KitchenObjectsContainerVisual>();
    }

    private void Start() {
        plateKitchenObject.OnValidIngredientsChanged += OnValidIngredientsChanged;
    }

    private void OnValidIngredientsChanged(object sender, EventArgs e) {
        UpdateVisuals();
    }

    private void UpdateVisuals() {
        var oldPlateArrangementPrefab = plateArrangementPrefab;

        FinalPlateSO plateSO = plateKitchenObject.ValidFinalPlates[0];

        if (plateSO == null) {
            Debug.LogError("PlateSO is null");
            return;
        }

        var newPlateArrangementPrefab = plateSO.FinalPlateArrangmentPrefab;
        if (oldPlateArrangementPrefab != newPlateArrangementPrefab) {
            plateArrangementPrefab = newPlateArrangementPrefab;

            if (plateArrangementInstance != null)
                Destroy(plateArrangementInstance);

            plateArrangementInstance = Instantiate(plateArrangementPrefab, plateArrangementParent);
            containerVisual.SetCustomArrangement(plateArrangementInstance);
        }
    }



}
