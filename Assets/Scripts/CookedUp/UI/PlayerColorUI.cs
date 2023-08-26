using System;
using CookedUp.Core.Players;
using UnityEngine;
using UnityEngine.UI;

namespace CookedUp.UI
{
    public class PlayerColorUI : MonoBehaviour
    {
        [SerializeField] private Image colorImage;
        [SerializeField] private Button button;
        [SerializeField] private GameObject selectedIndicator;
        
        public PlayerColorSO playerColorSO { get; private set; }
        
        public bool isSelected { get; private set; }

        

        private void Awake() {
            SetSelected(isSelected);
        }

        public void Init(PlayerColorSO playerColor, Action<PlayerColorUI> onClickCallback) {
            playerColorSO = playerColor;
            colorImage.color = playerColor.Color;
            button.onClick.AddListener(() => onClickCallback(this));
        }
        
        public void SetSelected(bool selected) {
            isSelected = selected;
            selectedIndicator.SetActive(selected);
        }
    }
}
