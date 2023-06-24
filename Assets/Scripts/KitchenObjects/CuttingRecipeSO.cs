using UnityEngine;

[CreateAssetMenu(fileName = "new CuttingRecipeSO", menuName = "CookedUp/CuttingRecipeSO", order = 1)]
public class CuttingRecipeSO : ScriptableObject {
    [SerializeField] private KitchenObjectSO input;
    [SerializeField] private KitchenObjectSO output;
    [SerializeField, Range(0.01f, 10f)] private float timeToCut = 1f;

    public KitchenObjectSO Input => input;

    public KitchenObjectSO Output => output;

    /// <summary>
    /// Time in seconds to cut the input into the output
    /// </summary>
    public float TimeToCut => timeToCut;

}
