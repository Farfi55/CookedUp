using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class FinalPlateArrangement : MonoBehaviour, ICustomArrangementProvider {
    [SerializeField] private FinalPlateSO finalPlateSO;
    public FinalPlateSO FinalPlateSO => finalPlateSO;

    public List<KitchenObjectSO> Ingredients => finalPlateSO.Ingredients;

    [SerializeField] private List<IngredientTransform> ingredientsTrasform = new();
    public List<IngredientTransform> IngredientsTrasform => ingredientsTrasform;

    private Dictionary<KitchenObjectSO, Transform> ingredientTransformDict;



    private void Awake() {
        if (ingredientsTrasform.Count == 0)
            LoadIngredientTransforms();
        ClearGrandChildren();

        ingredientTransformDict = new Dictionary<KitchenObjectSO, Transform>();
        foreach (var ingredientTransform in ingredientsTrasform) {
            ingredientTransformDict.Add(ingredientTransform.Ingredient, ingredientTransform.Transform);
        }
    }

    private void ClearGrandChildren() {
        foreach (Transform child in transform) {
            foreach (Transform grandChild in child) {
                Destroy(grandChild.gameObject);
            }
        }
    }


    public void SetTrasforms(List<KitchenObject> kitchenObjects) {

        foreach (var kitchenObject in kitchenObjects) {
            if (ingredientTransformDict.TryGetValue(kitchenObject.KitchenObjectSO, out var ingredientTransform)) {
                kitchenObject.transform.SetParent(ingredientTransform);
                kitchenObject.transform.localPosition = Vector3.zero;
                kitchenObject.transform.localRotation = Quaternion.identity;
                kitchenObject.transform.localScale = Vector3.one;
            }
            else {
                Debug.LogError($"Couldn't find position for {kitchenObject.name} in {this.name}", this);
            }
        }
    }


    [ContextMenu("Load FinalPlateSO from name")]
    private void LoadFinalPlateSOFromName() {
        var name = gameObject.name.Replace("Arrangement", "");


        string path = "Assets/_Assets/ScriptableObjects/FinalPlatesSO/" + name + ".asset";
        finalPlateSO = UnityEditor.AssetDatabase.LoadAssetAtPath<FinalPlateSO>(path);
        if (finalPlateSO == null) {
            Debug.LogError($"Couldn't find FinalPlateSO at {path}", this);
        }
    }

    [ContextMenu("Load ingredient transforms")]
    private void LoadIngredientTransforms() {
        foreach (var ingredient in Ingredients) {
            var ingredientPosition = transform.Find(ingredient.name);
            if (ingredientPosition == null) {
                Debug.LogError($"Couldn't find position for {ingredient.name} in {this.name}", this);
                continue;
            }

            ingredientsTrasform.Add(new IngredientTransform(ingredient, ingredientPosition));
        }
#if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(this);
#endif
    }

#if UNITY_EDITOR
    [ContextMenu("Create ingredient transforms")]
    private void CreateIngredientTrasforms() {
        ingredientsTrasform = new List<IngredientTransform>();
        string visualPrefabsBasePath = "Assets/_Assets/PrefabsVisuals/KitchenObjectsVisuals/";
        foreach (var ingredient in Ingredients) {
            var ingredientGO = new GameObject(ingredient.name);
            ingredientGO.transform.SetParent(transform);
            ingredientsTrasform.Add(new IngredientTransform(ingredient, ingredientGO.transform));

            // Load visual prefab and instantiate it as a child of the ingredient
            string path = visualPrefabsBasePath + ingredient.name + "_Visual.prefab";
            GameObject visualPrefab = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(path);

            if (visualPrefab == null) {
                Debug.LogError($"Couldn't find visual prefab for {ingredient.name} at {path}", this);
                continue;
            }
            Instantiate(visualPrefab, Vector3.zero, Quaternion.identity, ingredientGO.transform);
        }
        UnityEditor.EditorUtility.SetDirty(this);
    }
#endif

    [Serializable]

    public struct IngredientTransform {
        public KitchenObjectSO Ingredient;
        public Transform Transform;

        public IngredientTransform(KitchenObjectSO ingredient, Transform transform) {
            this.Ingredient = ingredient;
            this.Transform = transform;
        }
    }
}
