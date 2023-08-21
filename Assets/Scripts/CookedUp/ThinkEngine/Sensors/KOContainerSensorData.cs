using System;
using System.Collections.Generic;
using KitchenObjects.Container;
using ThinkEngine.Models;
using UnityEngine;

namespace ThinkEngine.Sensors {
    public class KOContainerSensorData : MonoBehaviour {
        
        private IDManager idManager;
        
        [SerializeField] private KitchenObjectsContainer container;

        [Header("Sensor Data")]
        public int ContainerID;
        public int SizeLimit;
        public int Count;
        
        // ReSharper disable once CollectionNeverQueried.Global
        // ReSharper disable once FieldCanBeMadeReadOnly.Global
        public List<KitchenObjectASP> KitchenObjects = new();
        
        public KitchenObjectASP FirstKitchenObject;
        public bool HasSpace;
        public bool HasAny;


        private void Start() {
            idManager = IDManager.Instance;
            container.OnKitchenObjectsChanged += OnKitchenObjectsChanged;
            SizeLimit = container.SizeLimit;
            ContainerID = idManager.GetID(container.gameObject);
            UpdateContainerData();
        }

        private void OnKitchenObjectsChanged(object sender, KitchenObjectsChangedEvent e) => UpdateContainerData();

        private void UpdateContainerData()
        {
            Count = container.Count;
            
            foreach (var oldKitchenObject in KitchenObjects)
                idManager.RemoveID(oldKitchenObject.ID);
            
            KitchenObjects.Clear();

            foreach (var kitchenObject in container.KitchenObjects)
                KitchenObjects.Add(new KitchenObjectASP(kitchenObject));

            HasSpace = container.HasSpace();
            HasAny = container.HasAny();
            
            FirstKitchenObject = HasAny ? new KitchenObjectASP(container.KitchenObject) : null;
        }

        private void OnDestroy() {
            idManager.RemoveID(ContainerID);
        }
    }
}
