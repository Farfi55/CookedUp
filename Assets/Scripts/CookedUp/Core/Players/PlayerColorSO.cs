using UnityEngine;

namespace CookedUp.Core.Players
{
    [CreateAssetMenu(menuName = "CookedUp/PlayerColorSO", fileName = "PlayerColor", order = 10)]
    public class PlayerColorSO : ScriptableObject {
        [SerializeField] private Color color;
        [SerializeField] private Material material;
        [SerializeField] private string displayName = "Not Set";

        public Color Color => color;
        public Material Material => material;
        public string DisplayName => displayName;



#if UNITY_EDITOR
        private void OnValidate() {
            if (material != null && color.Equals(new Color(0,0,0,0)))
                color = material.color;
            if (DisplayName == "Not Set")
                displayName = name.Replace("PlayerColor", "");
        }
#endif
    }
}
