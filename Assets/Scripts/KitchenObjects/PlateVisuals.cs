using System;
using KitchenObjects.Container;
using KitchenObjects.ScriptableObjects;
using UnityEngine;

namespace KitchenObjects
{
    [RequireComponent(typeof(PlateKitchenObject))]
    public class PlateVisuals : MonoBehaviour {

        private PlateKitchenObject plateKitchenObject;
        private KitchenObjectsContainerVisual containerVisual;

        private RecipeArrangement plateArrangementPrefab;
        private RecipeArrangement plateArrangementInstance;


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

            CompleteRecipeSO plateSO = plateKitchenObject.ValidCompleteRecipes[0];

            if (plateSO == null) {
                Debug.LogError("PlateSO is null");
                return;
            }

            var newPlateArrangementPrefab = plateSO.RecipeArrangementPrefab;
            if (oldPlateArrangementPrefab != newPlateArrangementPrefab) {
                plateArrangementPrefab = newPlateArrangementPrefab;

                if (plateArrangementInstance != null)
                    Destroy(plateArrangementInstance);

                plateArrangementInstance = Instantiate(plateArrangementPrefab, plateArrangementParent);
                containerVisual.SetCustomArrangement(plateArrangementInstance);
            }
        }



    }
}
