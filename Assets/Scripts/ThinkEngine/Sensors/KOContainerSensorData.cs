using System;
using System.Collections.Generic;
using KitchenObjects.Container;
using UnityEngine;

namespace ThinkEngine.Sensors {
    public class KOContainerSensorData : MonoBehaviour {
        [SerializeField] private KitchenObjectsContainer container;

        public int sizeLimit;
        public int count;
        
        public List<string> kosNames = new();
        public string koName;
        
        public bool hasSpace;
        public bool hasAny;
        

        private void Start() {
            container.OnKitchenObjectsChanged += OnKitchenObjectsChanged;
            sizeLimit = container.SizeLimit;
        }

        private void OnKitchenObjectsChanged(object sender, KitchenObjectsChangedEvent e) {
            
            count = container.Count;
            kosNames.Clear();
            foreach (var ko in container.KitchenObjects) {
                kosNames.Add(ko.KitchenObjectSO.name);
            }
            hasSpace = container.HasSpace();
            hasAny = container.HasAny();
            
            koName = hasAny ? container.KitchenObject.KitchenObjectSO.name : "";
        }
    }
}
