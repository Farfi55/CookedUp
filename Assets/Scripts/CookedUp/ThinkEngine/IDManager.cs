using System.Collections.Generic;
using KitchenObjects.ScriptableObjects;
using UnityEngine;

namespace ThinkEngine {
    public class IDManager : MonoBehaviour {
        public static IDManager Instance { get; private set; }

        private Dictionary<int, GameObject> idMap = new();

        private void Awake() {
            if (Instance == null) {
                Instance = this;
            }
            else {
                Debug.LogError("Multiple IDManagers in scene");
                Destroy(gameObject);
            }
        }


        public int GetID(GameObject gameObject) {
            var id = gameObject.GetInstanceID();
            idMap[id] = gameObject;
            return id;
        }


        public GameObject GetGameObject(int id) {
            return idMap[id];
        }

        public T GetComponentFromID<T>(int id) {
            var go = idMap[id];
            if (go == null)
                Debug.LogError($"GameObject with id {id} does not exist!");
            
            if (go.TryGetComponent(out T component))
                return component;
            
            Debug.LogError($"GameObject {go.name} does not have a {typeof(T)} component!");
            return default;
        }

        public void RemoveID(int id) {
            idMap.Remove(id);
        }

        public void RemoveID(GameObject gameObject) {
            var id = gameObject.GetInstanceID();
            idMap.Remove(id);
        }
    }
}
