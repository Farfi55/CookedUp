using Extensions;
using UnityEditor;
using UnityEngine;

namespace KitchenObjects.ScriptableObjects
{
    [CreateAssetMenu(fileName = "new KitchenObject", menuName = "CookedUp/KitchenObject", order = 0)]
    public class KitchenObjectSO : ScriptableObject {

        [SerializeField] private KitchenObject prefab;
        [SerializeField] private Sprite sprite;
        [SerializeField] private string displayName;


        public KitchenObject Prefab => prefab;
        public Sprite Sprite => sprite;

        public string DisplayName => displayName;


#if UNITY_EDITOR
        [ContextMenu("load from file name")]
        private void LoadFromName() {
            string prefabPath = $"Assets/Prefabs/KitchenObjects/{name}.prefab";
            prefab = AssetDatabase.LoadAssetAtPath<KitchenObject>(prefabPath);
            if (prefab == null) {
                Debug.LogError($"Couldn't find prefab for {displayName} at {prefabPath}", this);
                return;
            }

            string spritePath = $"Assets/_Assets/Textures/Icons/{name}.png";
            sprite = AssetDatabase.LoadAssetAtPath<Sprite>(spritePath);
            if (sprite == null) {
                Debug.LogError($"Couldn't find sprite for {displayName} at {spritePath}", this);
                return;
            }

            displayName = name.CamelCaseToSentence();

            UnityEditor.EditorUtility.SetDirty(this);
        }
#endif

    }
}
