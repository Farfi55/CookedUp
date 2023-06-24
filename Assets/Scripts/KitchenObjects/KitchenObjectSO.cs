using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CreateAssetMenu(fileName = "new KitchenObject", menuName = "CookedUp/KitchenObject", order = 0)]
public class KitchenObjectSO : ScriptableObject {

    [SerializeField] private KitchenObject prefab;
    [SerializeField] private Sprite sprite;
    [SerializeField] private string _name;


    public KitchenObject Prefab => prefab;
    public Sprite Sprite => sprite;

    public string Name => _name;


#if UNITY_EDITOR
    [ContextMenu("load from file name")]
    private void LoadFromName() {
        string prefabPath = $"Assets/Prefabs/KitchenObjects/{name}.prefab";
        prefab = AssetDatabase.LoadAssetAtPath<KitchenObject>(prefabPath);

        string spritePath = $"Assets/_Assets/Textures/Icons/{name}.png";
        sprite = AssetDatabase.LoadAssetAtPath<Sprite>(spritePath);

        _name = name.CamelCaseToSentence();

        UnityEditor.EditorUtility.SetDirty(this);
    }
#endif

}
