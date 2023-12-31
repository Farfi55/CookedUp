using System;
using CookedUp.Core.KitchenObjects.Container;
using Shared;
using UnityEngine;

namespace CookedUp.Core.KitchenObjects {
    public class KitchenObject : MonoBehaviour {
        [SerializeField] private KitchenObjectSO kitchenObjectSO;
        [SerializeField] private GameObject visuals;
        private KitchenObjectsContainer container;


        public KitchenObjectSO KitchenObjectSO => kitchenObjectSO;
        public KitchenObjectsContainer Container => container;

        public bool IsInContainer => container != null;
        public bool IsVisible => visuals.activeSelf;


        public event EventHandler OnDestroyed;
        public event EventHandler<ValueChangedEvent<KitchenObjectsContainer>> OnContainerChanged;


        public bool SetContainer(KitchenObjectsContainer newContainer) {
            var oldContainer = container;
            if (newContainer == oldContainer) {
                return true;
            }

            container = newContainer;

            if (oldContainer != null) {
                if (!oldContainer.Remove(this)) {
                    Debug.LogError($"Couldn't remove {this.name} from {oldContainer.name}", this);
                    container = oldContainer;
                    return false;
                }
            }

            if (container != null) {
                if (!container.TryAdd(this)) {
                    Debug.LogError($"Couldn't add {this.name} to {container.name}", this);
                    container = null;
                    return false;
                }
            }
            else {
                transform.SetParent(null);
            }

            SetVisible(IsInContainer);
            OnContainerChanged?.Invoke(this, new(oldContainer, newContainer));
            return true;
        }


        public void RemoveFromContainer() => SetContainer(null);


        /// <summary>
        /// Removes itself from the current holder and destroys itself.
        /// </summary>
        public virtual void DestroySelf() {
            RemoveFromContainer();
            OnDestroyed?.Invoke(this, EventArgs.Empty);
            Destroy(gameObject);
        }


        public bool IsSameType(KitchenObjectSO kitchenObjectSO) => this.kitchenObjectSO == kitchenObjectSO;
        public bool IsSameType(KitchenObject kitchenObject) => IsSameType(kitchenObject.kitchenObjectSO);


        public void SetVisible(bool visible) {
            visuals.SetActive(visible);
        }

        public static KitchenObject CreateInstance(KitchenObjectSO kitchenObjectSO,
            KitchenObjectsContainer container = null) {
            var kitchenObject = Instantiate(kitchenObjectSO.Prefab);
            kitchenObject.name = kitchenObjectSO.name;
            kitchenObject.SetContainer(container);
            return kitchenObject;
        }

        public virtual bool InteractWith(KitchenObject otherKitchenObject) { return false; }


        public bool TryGetPlate(out PlateKitchenObject plateKitchenObject) {
            if (this is PlateKitchenObject plate) {
                plateKitchenObject = plate;
                return true;
            }

            plateKitchenObject = null;
            return false;
        }

        public static KitchenObject ConvertKitchenObject(KitchenObject originalKO, KitchenObjectSO newKOSO) {
            var container = originalKO.Container;
            return ConvertKitchenObject(originalKO, newKOSO, container);
        }


        public static KitchenObject ConvertKitchenObject(KitchenObject originalKO, KitchenObjectSO newKOSO, KitchenObjectsContainer container) {
            var player = originalKO.GetComponent<KitchenObjectPlayer>()?.Player;
            originalKO.DestroySelf();
            
            var newKO = CreateInstance(newKOSO, container);
            
            if (player != null && newKO.TryGetComponent(out KitchenObjectPlayer koPlayer)) {
                koPlayer.SetPlayer(player);
            }
            
            return newKO;
        }
    }
}
