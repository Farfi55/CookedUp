using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "new FinalPlateKitchenObjectSO", menuName = "CookedUp/Final Plate", order = 3)]
public class FinalPlateSO : ScriptableObject {

    [SerializeField] private string displayName;
    [SerializeField] private List<KitchenObjectSO> ingredients = new();
    [SerializeField, Range(0, 150)] private int value = 10;

    [SerializeField] private FinalPlateArrangement finalPlateArrangmentPrefab;


    public string DisplayName => displayName;
    public List<KitchenObjectSO> Ingredients => ingredients;
    public float Value => value;

    public FinalPlateArrangement FinalPlateArrangmentPrefab => finalPlateArrangmentPrefab;



    public bool IsValidFor(IEnumerable<KitchenObjectSO> otherIngredients) {
        foreach (var otherIngredient in otherIngredients) {
            if (!ingredients.Contains(otherIngredient))
                return false;
        }

        return true;
    }

    public List<KitchenObjectSO> GetMissingIngredient(List<KitchenObjectSO> otherIngredients) {
        var missingIngredients = new List<KitchenObjectSO>();
        foreach (var ingredient in ingredients) {
            if (!otherIngredients.Contains(ingredient))
                missingIngredients.Add(ingredient);
        }

        return missingIngredients;
    }


#if UNITY_EDITOR
    [ContextMenu("load from file name")]
    private void LoadFromName() {
        displayName = name.CamelCaseToSentence();

        string path = $"Assets/Prefabs/FinalPlateArrangements/{name}Arrangement.prefab";
        finalPlateArrangmentPrefab = UnityEditor.AssetDatabase.LoadAssetAtPath<FinalPlateArrangement>(path);
        if (finalPlateArrangmentPrefab == null) {
            Debug.LogError($"Couldn't find prefab for {displayName} at {path}", this);
        }


        UnityEditor.EditorUtility.SetDirty(this);
    }
#endif

}
