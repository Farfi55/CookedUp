using System;
using System.Collections.Generic;
using UnityEngine;

namespace KitchenObjects.Container
{
    public class KitchenObjectsContainerVisual : MonoBehaviour {
        private KitchenObjectsContainer container;

        [SerializeField] private Transform[] kitchenObjectsParents;
        private Transform kitchenObjectsParent => kitchenObjectsParents[0];

        [SerializeField] private Type type;

        [SerializeField, Tooltip("The offset of a kitchen objects from the previous one.")]
        private Vector3 offset;

        private List<KitchenObject> kitchenObjects = new List<KitchenObject>();

        private ICustomArrangementProvider customArrangement;



        private void Awake() {
            if (container == null && !TryGetComponent<KitchenObjectsContainer>(out container)) {
                Debug.LogError($"No KitchenObjectsContainer found on {gameObject.name}", this);
            }
        }

        private void Start() {
            container.OnKitchenObjectsChanged += OnKitchenObjectsChanged;

            if (type == Type.FixedSingle || type == Type.FixedMultiple) {
                if (!container.HasSizeLimit()) {
                    Debug.LogError($"The container {container.name} doesn't have a size limit", this);
                }
                if (kitchenObjectsParents.Length < container.SizeLimit) {
                    Debug.LogError($"The number of kitchen objects parents ({kitchenObjectsParents.Length}) is less than the container size limit ({container.SizeLimit})", this);
                }
            }
            UpdateVisuals();
        }

        private void OnKitchenObjectsChanged(object sender, KitchenObjectsChangedEvent e) => UpdateVisuals();

        private void UpdateVisuals()
        {
            var oldKitchenObjects = new List<KitchenObject>(kitchenObjects);
            kitchenObjects = container.GetKitchenObjectsInOrder();


            foreach (var kitchenObject in oldKitchenObjects)
            {
                if (!kitchenObject.IsInContainer)
                {
                    kitchenObject.SetVisible(false);
                }
            }
            
            if(kitchenObjects.Count == 0)
                return;

            if (type == Type.Custom)
            {
                customArrangement.SetTransforms(kitchenObjects);
            }
            else
            {
                for (int index = 0; index < kitchenObjects.Count; index++)
                {
                    KitchenObject kitchenObject = kitchenObjects[index];
                    kitchenObject.SetVisible(true);
                    SetTrasform(kitchenObject, index);
                }
            }
        }

        private void SetTrasform(KitchenObject kitchenObject, int index) {
            Transform parent;
            Vector3 localPosition = Vector3.zero;
            Quaternion localRotation = Quaternion.identity;

            switch (type) {
                case Type.FixedSingle:
                    parent = kitchenObjectsParent;
                    break;
                case Type.FixedMultiple:
                    parent = kitchenObjectsParents[index];
                    break;
                case Type.DynamicOffset:
                    parent = kitchenObjectsParent;
                    localPosition = offset * index;
                    break;
                default:
                    throw new NotImplementedException();
            }

            kitchenObject.transform.SetParent(parent);
            kitchenObject.transform.localPosition = localPosition;
            kitchenObject.transform.localRotation = localRotation;
        }

        public void SetCustomArrangement(ICustomArrangementProvider customArrangement) {
            this.customArrangement = customArrangement;
            customArrangement.SetTransforms(kitchenObjects);

            if (type != Type.Custom) {
                Debug.Log($"The container {container.name} is now using a custom arrangement", this);
                type = Type.Custom;
            }
        }

        enum Type {
            FixedSingle,
            FixedMultiple,
            DynamicOffset,
            Custom,
        }

        private void OnDestroy() {
            container.OnKitchenObjectsChanged -= OnKitchenObjectsChanged;
        }

    }
}
