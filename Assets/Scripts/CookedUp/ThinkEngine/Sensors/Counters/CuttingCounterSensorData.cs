using CookedUp.Core.Counters;
using CookedUp.Core.KitchenObjects;
using CookedUp.ThinkEngine.Models;
using Shared;
using UnityEngine;

namespace CookedUp.ThinkEngine.Sensors.Counters {
    public class CuttingCounterSensorData : MonoBehaviour {
        [SerializeField] private CuttingCounter cuttingCounter;

        [Header("Sensor Data")] public bool HasAny;
        
        public bool CanCut;

        public int TimeRemainingToCut;

        public CuttingRecipe CurrentCuttingRecipe;

        private void Start() {
            cuttingCounter.OnRecipeChanged += OnRecipeChanged;
            cuttingCounter.ProgressTracker.OnProgressChanged += OnProgressChanged;
        }

        private void OnProgressChanged(object sender, ValueChangedEvent<double> e) {
            if (cuttingCounter.CanCut())
                TimeRemainingToCut = Converter.FloatToInt(cuttingCounter.ProgressTracker.GetWorkRemaining());
            else
                TimeRemainingToCut = 0;
        }

        private void OnRecipeChanged(object sender, ValueChangedEvent<BaseRecipeSO> e) {
            HasAny = cuttingCounter.Container.HasAny();
            CanCut = cuttingCounter.CanCut();
            var currentRecipeSO = cuttingCounter.CurrentCuttingRecipe;
            CurrentCuttingRecipe = currentRecipeSO != null ? new CuttingRecipe(currentRecipeSO) : null;
        }
    }
}
