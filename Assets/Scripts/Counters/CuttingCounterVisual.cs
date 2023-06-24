using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CuttingCounter))]
public class CuttingCounterVisual : MonoBehaviour {

    [SerializeField] private Animator[] animators;
    private CuttingCounter cuttingCounter;

    private readonly int cutHash = Animator.StringToHash("Cut");

    private double lastCutTime = 0;
    [SerializeField, Range(0f, 2f)] private double baseCutCooldown = 0.3f;
    [SerializeField, Range(0f, 1f)] private float maxRandomBonusCutCooldown = 0.1f;


    private void Awake() {

        cuttingCounter = GetComponent<CuttingCounter>();
        cuttingCounter.OnInteractAlternate += (sender, e) => {
            if (Time.time - lastCutTime > baseCutCooldown) {
                lastCutTime = Time.time + Random.Range(0, maxRandomBonusCutCooldown);
                foreach (var animator in animators)
                    animator.SetTrigger(cutHash);
            }
        };
    }

}
