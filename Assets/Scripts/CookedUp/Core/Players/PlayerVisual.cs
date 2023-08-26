using System.Collections.Generic;
using UnityEngine;

namespace CookedUp.Core.Players
{
    public class PlayerVisual : MonoBehaviour
    {
        [SerializeField] private List<MeshRenderer> playerMeshRenderers = new();
        [SerializeField] private List<GameObject> playerEyes = new();
        
    
        [SerializeField] private PlayerColorSO playerColorSO;
        public PlayerColorSO PlayerColorSO => playerColorSO;

        private void Awake() {
            SetColor(playerColorSO);
        }

        public void SetColor(PlayerColorSO playerColor) {
            playerColorSO = playerColor;
            foreach (var meshRenderer in playerMeshRenderers) {
                meshRenderer.material = playerColor.Material;
            }
        }
        
        
        public void SetMaterial(Material material) {
            foreach (var meshRenderer in playerMeshRenderers) {
                meshRenderer.material = material;
            }
        }
        
        
        public void SetEyesActive(bool active) {
            foreach (var eye in playerEyes) {
                eye.SetActive(active);
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
