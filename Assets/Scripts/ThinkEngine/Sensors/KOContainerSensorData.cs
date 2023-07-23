using System.Collections.Generic;
using KitchenObjects.Container;
using ThinkEngine.Models;
using UnityEngine;

namespace ThinkEngine.Sensors {
    public class KOContainerSensorData : MonoBehaviour {
        
        
        [SerializeField] private KitchenObjectsContainer container;

        public int ContainerID;
        public int SizeLimit;
        public int Count;
        
        // ReSharper disable once CollectionNeverQueried.Global
        // ReSharper disable once FieldCanBeMadeReadOnly.Global
        public List<KitchenObject> KitchenObjects = new();
        
        public KitchenObject FirstKitchenObject;
        public bool HasSpace;
        public bool HasAny;
        

        private void Start() {
            container.OnKitchenObjectsChanged += OnKitchenObjectsChanged;
            SizeLimit = container.SizeLimit;
            ContainerID = container.gameObject.GetInstanceID();
            UpdateContainerData();
        }

        private void OnKitchenObjectsChanged(object sender, KitchenObjectsChangedEvent e) => UpdateContainerData();

        private void UpdateContainerData()
        {
            Count = container.Count;
            KitchenObjects.Clear();
            
            foreach (var kitchenObject in container.KitchenObjects)
                KitchenObjects.Add(new KitchenObject(kitchenObject));

            HasSpace = container.HasSpace();
            HasAny = container.HasAny();
            
            FirstKitchenObject = HasAny ? new KitchenObject(container.KitchenObject) : null;

        }
    }
}
