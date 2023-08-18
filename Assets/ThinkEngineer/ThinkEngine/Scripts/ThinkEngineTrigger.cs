using System;
using UnityEngine;

// every method of this class without parameters and that returns a bool value can be used to trigger the reasoner.
namespace ThinkEngine {
    
    public class ThinkEngineTrigger : ScriptableObject {
        private int lastSensorIteration = Int32.MinValue;
        private const int ITERATIONS_BETWEEN_BRAIN_EXECUTION = 1;


        public bool OnNewSensorIteration() {
            var sensorIteration = SensorsManager.iteration;
            
            if(sensorIteration >= lastSensorIteration + ITERATIONS_BETWEEN_BRAIN_EXECUTION) {
                lastSensorIteration = sensorIteration;
                return true;
            }
            
            return false;
        }
        
        
        
    }
}
