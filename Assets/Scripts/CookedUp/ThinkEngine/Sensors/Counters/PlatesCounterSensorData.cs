using System;
using Counters;
using KitchenObjects.Container;
using ThinkEngine.Models;
using UnityEngine;

namespace ThinkEngine.Sensors.Counters {
    public class PlatesCounterSensorData : MonoBehaviour {
        [SerializeField] private PlatesCounter platesCounter;

        [Header("Sensor Data")]
        public int PlatesLimit;
        public int PlatesCount;
        public int TimeToNextPlate;
        

        private void Start() {
            platesCounter.Container.OnKitchenObjectsChanged += OnPlatesChanged;
            PlatesLimit = platesCounter.PlatesLimit;
            PlatesCount = platesCounter.PlatesCount;
        }

        private void OnPlatesChanged(object sender, KitchenObjectsChangedEvent e) {
            PlatesCount = platesCounter.PlatesCount;
        }
        
        private void Update() {
            TimeToNextPlate = Converter.FloatToInt(platesCounter.TimeToNextPlate);
        }

        
        
    }
}
