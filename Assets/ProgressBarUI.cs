using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour {

    [SerializeField] private CuttingCounter cuttingCounter;

    [SerializeField] private Image progressFillBar;
    [SerializeField] private Image outputPreviewImage;

    [SerializeField] private GameObject progressBarUIObject;

    private KitchenObjectSO kitchenObjectSO;


    private void Start() {
        cuttingCounter.OnCutProgressChanged += CuttingCounter_OnCutProgressChanged;
        CuttingCounter_OnCutProgressChanged(this, EventArgs.Empty);
    }

    private void CuttingCounter_OnCutProgressChanged(object sender, EventArgs e) {

        var progress = cuttingCounter.GetCutProgress();
        progressFillBar.fillAmount = progress;

        progressBarUIObject.SetActive(progress > 0f);

        if (cuttingCounter.CanCut()) {
            outputPreviewImage.sprite = cuttingCounter.CurrentRecipe.Output.Sprite;
            outputPreviewImage.gameObject.SetActive(true);
        }
        else
            outputPreviewImage.gameObject.SetActive(false);

    }
}
