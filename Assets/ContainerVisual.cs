using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ContainerCounter))]
public class ContainerVisual : MonoBehaviour {

    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Animator animator;

    private ContainerCounter containerCounter;

    private readonly int OpenCloseHash = Animator.StringToHash("OpenClose");

    private void Awake() {

        containerCounter = GetComponent<ContainerCounter>();
        spriteRenderer.sprite = containerCounter.KitchenObjectSO.Sprite;
        containerCounter.OnInteracted += (sender, e) => {
            animator.SetTrigger(OpenCloseHash);
        };
    }
}
