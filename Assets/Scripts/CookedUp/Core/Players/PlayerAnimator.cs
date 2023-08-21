using UnityEngine;

namespace CookedUp.Core.Players
{
    [RequireComponent(typeof(Animator))]
    public class PlayerAnimator : MonoBehaviour {
        [SerializeField] private PlayerMovement playerMovement;
        private Animator animator;

        private readonly int IsWalkingHash = Animator.StringToHash("IsWalking");

        private void Awake() {
            animator = GetComponent<Animator>();
        }

        // Update is called once per frame
        void Update() {
            animator.SetBool(IsWalkingHash, playerMovement.IsMoving);
        }
    }
}
