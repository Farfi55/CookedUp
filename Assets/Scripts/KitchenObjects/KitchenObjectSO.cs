using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new KitchenObject", menuName = "CookedUp/KitchenObject", order = 0)]
public class KitchenObjectSO : ScriptableObject {

    [SerializeField] private KitchenObject prefab;
    [SerializeField] private Sprite sprite;
    [SerializeField] private string _name;


    public KitchenObject Prefab => prefab;
    public Sprite Sprite => sprite;

    public string Name => _name;


}
