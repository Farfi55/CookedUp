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
        
        

        private void OnEnable() {
            platesCounter.Container.OnKitchenObjectsChanged += OnPlatesChanged;
        }

        private void OnPlatesChanged(object sender, KitchenObjectsChangedEvent e) {
            PlatesCount = platesCounter.PlatesCount;
        }

        private void Start() {
            PlatesLimit = platesCounter.PlatesLimit;
            PlatesCount = platesCounter.PlatesCount;
        }
    }
}
