using System.Collections.Generic;
using UnityEngine;

namespace CookedUp.Core.Players
{
    public class PlayerVisual : MonoBehaviour
    {
        [SerializeField] private List<MeshRenderer> playerMeshRenderers = new();
    
        [SerializeField] private PlayerColorSO playerColorSO;
        public PlayerColorSO PlayerColorSO => playerColorSO;

        private void Awake() {
            SetColor(playerColorSO);
        }

        private void SetColor(PlayerColorSO playerColor) {
            playerColorSO = playerColor;
            foreach (var meshRenderer in playerMeshRenderers) {
                meshRenderer.material = playerColor.Material;
            }
        }

#if UNITY_EDITOR
        private void OnValidate() {
            if(playerColorSO != null)
                SetColor(playerColorSO);
        }
#endif
    }
}
