using System;
using KitchenObjects.ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI {
    public class DeliveryManagerSingleUI : MonoBehaviour {
        [SerializeField] private TextMeshProUGUI recipeNameText;
        [SerializeField] private Transform ingredientsContainer;
        [SerializeField] private Transform ingredientTemplate;


        private void Awake() {
            ingredientTemplate.gameObject.SetActive(false);
        }

        public void SetRecipeSO(CompleteRecipeSO recipeSO) {
            recipeNameText.text = recipeSO.DisplayName;

            foreach (Transform child in ingredientsContainer) {
                if(child == ingredientTemplate.transform)
                    continue;
                Destroy(child.gameObject);
            }
            
            foreach (var kitchenObjectSO in recipeSO.Ingredients)
            {
                Transform ingredientUI = Instantiate(ingredientTemplate, ingredientsContainer);
                ingredientUI.gameObject.SetActive(true);
                ingredientUI.GetComponent<Image>().sprite = kitchenObjectSO.Sprite;
            }
        }
    }
}
