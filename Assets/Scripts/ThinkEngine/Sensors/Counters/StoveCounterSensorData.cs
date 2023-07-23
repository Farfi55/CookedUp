using Counters;
using KitchenObjects.ScriptableObjects;
using ThinkEngine.Models;
using UnityEngine;

namespace ThinkEngine.Sensors.Counters {
    public class StoveCounterSensorData : MonoBehaviour {
        [SerializeField] private StoveCounter stoveCounter;
        
        [Header("Sensor Data")] public bool HasAny;
        
        public bool CanCook;
        
        public int TimeRemainingToCook;
        
        public bool IsBurning;
        
        public CookingRecipe CurrentCookingRecipe;
        
        private void OnEnable() {
            stoveCounter.OnRecipeChanged += OnRecipeChanged;
            stoveCounter.ProgressTracker.OnProgressChanged += OnProgressChanged;
        }

        private void OnRecipeChanged(object sender, ValueChangedEvent<BaseRecipeSO> e) {
            HasAny = stoveCounter.Container.HasAny();
            CanCook = stoveCounter.CanCook();
            TimeRemainingToCook = Converter.FloatToInt(stoveCounter.GetRemainingTime());
            
            IsBurning = stoveCounter.IsBurningRecipe;

            if (stoveCounter.CurrentCookingRecipe != null)
                CurrentCookingRecipe = new CookingRecipe(stoveCounter.CurrentCookingRecipe);
            else
                CurrentCookingRecipe = null;
        }

        private void OnProgressChanged(object sender, ValueChangedEvent<double> e) {
            TimeRemainingToCook = Converter.FloatToInt(stoveCounter.ProgressTracker.GetWorkRemaining());
        }
    }
}
