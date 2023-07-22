using System;
using System.Collections.Generic;
using KitchenObjects;
using KitchenObjects.Container;
using UnityEngine;

namespace ThinkEngine.Sensors {
    public class KOContainerSensorData : MonoBehaviour {
        
        
        [SerializeField] private KitchenObjectsContainer container;

        public int gameObjectID;
        public int sizeLimit;
        public int count;
        
        // ReSharper disable once CollectionNeverQueried.Global
        // ReSharper disable once FieldCanBeMadeReadOnly.Global
        public List<KitchenObjectData> KitchenObjects = new();
        
        public KitchenObjectData FirstKitchenObjectData;
        public bool hasSpace;
        public bool hasAny;
        

        private void Start() {
            container.OnKitchenObjectsChanged += OnKitchenObjectsChanged;
            sizeLimit = container.SizeLimit;
            gameObjectID = container.gameObject.GetInstanceID();
            UpdateContainerData();
        }

        private void OnKitchenObjectsChanged(object sender, KitchenObjectsChangedEvent e) => UpdateContainerData();

        private void UpdateContainerData()
        {
            count = container.Count;
            KitchenObjects.Clear();
            
            foreach (var kitchenObject in container.KitchenObjects)
                KitchenObjects.Add(new KitchenObjectData(kitchenObject));

            hasSpace = container.HasSpace();
            hasAny = container.HasAny();
            
            FirstKitchenObjectData = hasAny ? new KitchenObjectData(container.KitchenObject) : null;

        }
    }
    
    
    public class KitchenObjectData
    {
        public string Name;
        public int ID;
        public int ContainerID;

        public KitchenObjectData(string name, int id, int containerID) {
            Name = name;
            ID = id;
            ContainerID = containerID;
        }
        
        public KitchenObjectData(KitchenObject ko) {
            Name = ko.KitchenObjectSO.name;
            ID = ko.GetInstanceID();
            ContainerID = ko.Container.gameObject.GetInstanceID();
        }
    }
}
