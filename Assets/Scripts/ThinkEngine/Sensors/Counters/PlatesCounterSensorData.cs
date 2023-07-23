using System;
using Counters;
using KitchenObjects.Container;
using UnityEngine;

namespace ThinkEngine.Sensors.Counters {
    public class PlatesCounterSensorData : MonoBehaviour {
        [SerializeField] private PlatesCounter platesCounter;

        [Header("Sensor Data")]
        public int PlatesLimit;
        public int PlatesCount;
        
        

        private void Start() {
            platesCounter.Container.OnKitchenObjectsChanged += OnPlatesChanged;
            PlatesLimit = platesCounter.PlatesLimit;
            PlatesCount = platesCounter.PlatesCount;
        }

        private void OnPlatesChanged(object sender, KitchenObjectsChangedEvent e) {
            PlatesCount = platesCounter.PlatesCount;
        }

    }
}
