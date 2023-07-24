using System.Collections.Generic;
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
        
        public void RemoveGameObject(int id) {
            idMap.Remove(id);
        }
        
        
    }
}
