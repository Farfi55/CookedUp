using System;
using System.Collections.Generic;
using UnityEngine;

namespace CookedUp.Core.KitchenObjects.Container
{
    public class KitchenObjectsContainer : MonoBehaviour {
        private List<KitchenObject> kitchenObjects = new();

        public KitchenObject KitchenObject => GetNext();

        public IReadOnlyCollection<KitchenObject> KitchenObjects => kitchenObjects.AsReadOnly();

        private ContainerOrderType getPolicy = ContainerOrderType.FirstInFirstOut;


        public int Count => kitchenObjects.Count;


        [SerializeField, Range(-1, 20), Tooltip("If -1, there is no size limit")]
        private int sizeLimit;
        public int SizeLimit => sizeLimit;
        public bool HasSizeLimit() => sizeLimit >= 0;
        public int GetRemainingSpace() => HasSizeLimit() ? sizeLimit - Count : int.MaxValue;


        public event EventHandler<KitchenObject> OnKitchenObjectAdded;
        public event EventHandler<KitchenObject> OnKitchenObjectRemoved;
        public event EventHandler<KitchenObject> OnKitchenObjectPopped;
        public event EventHandler<KitchenObjectsChangedEvent> OnKitchenObjectsChanged;


        public bool IsFull() => HasSizeLimit() && Count == sizeLimit;
        public bool HasSpace() => !IsFull();
        public bool HasAny() => Count > 0;
        public bool IsEmpty() => Count == 0;


        private KitchenObject GetNext() {
            if (IsEmpty())
                return null;

            return getPolicy switch {
                ContainerOrderType.FirstInFirstOut => kitchenObjects[0],
                ContainerOrderType.LastInFirstOut => kitchenObjects[Count - 1],
                _ => throw new NotImplementedException(),
            };
        }

        public List<KitchenObject> GetKitchenObjectsInOrder() {
            var kitchenObjectsInOrder = new List<KitchenObject>(kitchenObjects);
            if (kitchenObjectsInOrder.Count <= 1)
                return kitchenObjectsInOrder;

            switch (getPolicy) {
                case ContainerOrderType.FirstInFirstOut:
                    break;
                case ContainerOrderType.LastInFirstOut:
                    kitchenObjectsInOrder.Reverse();
                    break;
                default:
                    throw new NotImplementedException();
            }
            return kitchenObjectsInOrder;
        }



        /// <summary>
        /// Removes the next KitchenObject from the container and returns it.
        /// </summary>
        /// <returns></returns>
        public KitchenObject Pop() {
            var kitchenObject = GetNext();
            Remove(kitchenObject);
            OnKitchenObjectPopped?.Invoke(this, kitchenObject);
            return kitchenObject;
        }


        public bool Remove(KitchenObject kitchenObject) {
            bool result = kitchenObjects.Remove(kitchenObject);

            if (result) {
                OnKitchenObjectRemoved?.Invoke(this, kitchenObject);
                InvokeOnChange();
            }
            return result;
        }

        public bool TryAdd(KitchenObject kitchenObject) {
            if (IsFull() || kitchenObject == null || Contains(kitchenObject)) {
                return false;
            }

            Add(kitchenObject);
            return true;
        }

        public void Add(KitchenObject kitchenObject) {
            if (kitchenObject == null) {
                throw new ArgumentNullException(nameof(kitchenObject));
            }
            if (Contains(kitchenObject)) {
                throw new ArgumentException($"The container already contains {kitchenObject.name}");
            }

            kitchenObjects.Add(kitchenObject);
            OnKitchenObjectAdded?.Invoke(this, kitchenObject);
            InvokeOnChange();
        }


        public bool Contains(KitchenObject kitchenObject) {
            return kitchenObjects.Contains(kitchenObject);
        }

        public void Clear() {
            var tmp = new List<KitchenObject>(kitchenObjects);
            kitchenObjects.Clear();

            foreach (var kitchenObject in tmp) {
                OnKitchenObjectRemoved?.Invoke(this, kitchenObject);
            }

            InvokeOnChange();
        }
    
        public List<KitchenObjectSO> AsKitchenObjectSOs() {
            return kitchenObjects.ConvertAll(input => input.KitchenObjectSO);
        }

        private void InvokeOnChange() {
            var args = new KitchenObjectsChangedEvent(this, KitchenObjects, GetNext());
            OnKitchenObjectsChanged?.Invoke(this, args);
        }

        public enum ContainerOrderType {
            FirstInFirstOut,
            LastInFirstOut
        }
    }

    public class KitchenObjectsChangedEvent : EventArgs {
        public readonly KitchenObjectsContainer Container;
        public readonly IReadOnlyCollection<KitchenObject> KitchenObjects;
        public readonly KitchenObject NextKitchenObject;

        public bool HasKitchenObjects() => NextKitchenObject != null;

        public KitchenObjectsChangedEvent(
            KitchenObjectsContainer container,
            IReadOnlyCollection<KitchenObject> kitchenObjects,
            KitchenObject nextKitchenObject) {
            this.Container = container;
            this.KitchenObjects = kitchenObjects;
            this.NextKitchenObject = nextKitchenObject;
        }
    }
}