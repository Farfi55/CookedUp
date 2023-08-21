using UnityEngine;

namespace KitchenObjects.ScriptableObjects
{
    [CreateAssetMenu(fileName = "new CookingRecipeSO", menuName = "CookedUp/Cooking Recipe", order = 2)]
    public class CookingRecipeSO : BaseRecipeSO {
        [SerializeField, Range(0.01f, 15f)] private float timeToCook = 1f;

        [SerializeField] private bool isBurningRecipe = false;

        /// <summary>
        /// Time in seconds to cook the input into the output
        /// </summary>
        public float TimeToCook => timeToCook;

        /// <summary>
        /// Whether or not this recipe needs a warning
        /// </summary>
        public bool IsBurningRecipe => isBurningRecipe;
    }
}
