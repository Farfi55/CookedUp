using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour {

    [SerializeField] private ProgressTracker progressTracker;
    private IRecipeProvider recipeProvider;

    [Header("UI")]
    [SerializeField] private Image progressFillBar;
    [SerializeField] private Image outputPreviewImage;
    [SerializeField] private GameObject progressBarUIObject;

    public BaseRecipeSO CurrentRecipe => currentRecipe;
    private BaseRecipeSO currentRecipe;

    [SerializeField] private bool hideIfEmpty = true;
    [SerializeField] private bool hideIfFull = true;


    private bool isBurningRecipe = false;

    [SerializeField] private Color burningRecipeProgressColor = Color.red;
    private Color normalRecipeProgressColor;

    private void Start() {
        recipeProvider = progressTracker.GetComponent<IRecipeProvider>();
        if (recipeProvider != null) {
            recipeProvider.OnRecipeChanged += (sender, e) => SetRecipe(e.NewValue);
        }

        normalRecipeProgressColor = progressFillBar.color;
        progressTracker.OnProgressChanged += (sender, e) => SetProgress(e.NewValue);

        SetProgress(progressTracker.Progress);
        SetRecipe(recipeProvider?.CurrentRecipe);
    }


    public void SetProgress(double progress) {
        progressFillBar.fillAmount = (float)progress;

        if ((hideIfEmpty && progress <= 0d) || (hideIfFull && progress >= 1d))
            Hide();
        else
            Show();
    }

    public void SetRecipe(BaseRecipeSO recipe) {
        currentRecipe = recipe;
        if (recipe != null)
            outputPreviewImage.sprite = recipe.Output.Sprite;

        isBurningRecipe = false;
        if (recipe is CookingRecipeSO cookingRecipe) {
            isBurningRecipe = cookingRecipe.IsBurningRecipe;
        }

        if (isBurningRecipe)
            progressFillBar.color = burningRecipeProgressColor;
        else
            progressFillBar.color = normalRecipeProgressColor;


        outputPreviewImage.gameObject.SetActive(recipe != null);
    }


    private void Show() {
        progressBarUIObject.SetActive(true);
    }

    private void Hide() {
        progressBarUIObject.SetActive(false);
    }
}
