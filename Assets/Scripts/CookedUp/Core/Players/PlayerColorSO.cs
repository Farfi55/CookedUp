using UnityEngine;

namespace Players
{
    [CreateAssetMenu(menuName = "CookedUp/PlayerColorSO", fileName = "PlayerColor", order = 10)]
    public class PlayerColorSO : ScriptableObject {
        [SerializeField] private Color color;
        [SerializeField] private Material material;

        public Color Color => color;
        public Material Material => material;



#if UNITY_EDITOR
        private void OnValidate() {
            if (material != null && color.Equals(new Color(0,0,0,0)))
                color = material.color;
        }
#endif
    }
}
