using UnityEngine;

[CreateAssetMenu(fileName = "new CookingRecipeSO", menuName = "CookedUp/CookingRecipeSO", order = 2)]
public class CookingRecipeSO : ScriptableObject {
    [SerializeField] private KitchenObjectSO input;
    [SerializeField] private KitchenObjectSO output;
    [SerializeField, Range(0.01f, 15f)] private float timeToCook = 1f;

    /// <summary>
    /// The KitchenObjectSO required for this recipe
    /// </summary>
    public KitchenObjectSO Input => input;

    /// <summary>
    /// The resulting KitchenObjectSO after cooking the input
    /// </summary>
    public KitchenObjectSO Output => output;

    /// <summary>
    /// Time in seconds to cook the input into the output
    /// </summary>
    public float TimeToCook => timeToCook;

}
