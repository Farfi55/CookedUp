using KitchenObjects.Container;
using KitchenObjects.ScriptableObjects;
using UnityEngine;

namespace KitchenObjects
{
    public class KitchenObject : MonoBehaviour {
        [SerializeField] private KitchenObjectSO kitchenObjectSO;

        private KitchenObjectsContainer container;


        public KitchenObjectSO KitchenObjectSO => kitchenObjectSO;
        public KitchenObjectsContainer Container => container;

        public bool isInContainer => container != null;

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

            SetVisible(isInContainer);
            return true;
        }



        public void RemoveFromContainer() => SetContainer(null);



        /// <summary>
        /// Removes itself from the current holder and destroys itself.
        /// </summary>
        public void DestroySelf() {
            RemoveFromContainer();
            Destroy(gameObject);
        }


        public bool IsSameType(KitchenObjectSO kitchenObjectSO) => this.kitchenObjectSO == kitchenObjectSO;
        public bool IsSameType(KitchenObject kitchenObject) => IsSameType(kitchenObject.kitchenObjectSO);


        public void SetVisible(bool visible) {
            gameObject.SetActive(visible);
        }

        public static KitchenObject CreateInstance(KitchenObjectSO kitchenObjectSO, KitchenObjectsContainer container = null) {
            var kitchenObject = Instantiate(kitchenObjectSO.Prefab);
            kitchenObject.SetContainer(container);
            return kitchenObject;
        }

        public virtual bool InteractWith(KitchenObject currentKitchenObject) { return false; }

    
        public bool TryGetPlate(out PlateKitchenObject plateKitchenObject) {
            if (this is PlateKitchenObject plate) {
                plateKitchenObject = plate;
                return true;
            }

            plateKitchenObject = null;
            return false;
        }
    }
}
