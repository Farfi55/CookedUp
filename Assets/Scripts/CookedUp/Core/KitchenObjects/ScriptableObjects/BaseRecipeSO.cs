using UnityEngine;

namespace KitchenObjects.ScriptableObjects
{
    public abstract class BaseRecipeSO : ScriptableObject {

        [SerializeField] protected KitchenObjectSO input;
        [SerializeField] protected KitchenObjectSO output;

        /// <summary>
        /// The KitchenObjectSO required for this recipe
        /// </summary>
        public KitchenObjectSO Input => input;

        /// <summary>
        /// The KitchenObjectSO created by this recipe
        /// </summary>
        public KitchenObjectSO Output => output;


#if UNITY_EDITOR
        [ContextMenu("initialize from file name")]
        private void InitializeFromName() {
            var names = name.Split('-');
            if (names.Length != 2) {
                Debug.LogError("Invalid recipe name");
                return;
            }

            string inputName = names[0];
            string outputName = names[1];


            string inputPath = $"Assets/ScriptableObjects/KitchenObjectsSO/{inputName}.asset";
            string outputPath = $"Assets/ScriptableObjects/KitchenObjectsSO/{outputName}.asset";

            input = UnityEditor.AssetDatabase.LoadAssetAtPath<KitchenObjectSO>(inputPath);
            output = UnityEditor.AssetDatabase.LoadAssetAtPath<KitchenObjectSO>(outputPath);
            UnityEditor.EditorUtility.SetDirty(this);
        }
#endif


    }
}
