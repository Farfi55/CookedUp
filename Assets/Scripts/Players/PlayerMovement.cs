using System;
using UnityEngine;
using UnityEngine.AI;

namespace Players {
    public class PlayerMovement : MonoBehaviour {
        [SerializeField] private Player player;
        [SerializeField] private Rigidbody rb;
        [SerializeField] private NavMeshAgent agent;
        [SerializeField] private PlayerInput playerInput;

        public float MovementSpeed => movementSpeed;
        [SerializeField] private float movementSpeed = 5f;

        public float RotationSpeed => rotationSpeed;
        [SerializeField] private float rotationSpeed = 540f;

        public bool IsMoving => IsMovingUsingInput || IsMovingUsingNavigation;
        public bool IsMovingUsingInput { get; private set; } = false;
        public bool IsMovingUsingNavigation { get; private set; } = false;
        
        private Transform lookAtTarget = null;
        private bool lookAtTargetUntilSelected = false;
        [SerializeField] private bool stopAllWhenUsingInput = true;
        private bool HasLookAtTarget => lookAtTarget != null;


        public event EventHandler OnDestinationReached;
        public event EventHandler OnLookAtTargetCompleted;


        private void Start() {
            player.OnSelectedInteractableChanged += OnSelectedInteractableChanged;
        }

        private void OnDestroy() {
            player.OnSelectedInteractableChanged -= OnSelectedInteractableChanged;
        }

        private void OnSelectedInteractableChanged(object sender, EventArgs e) {
            if (!lookAtTargetUntilSelected || !HasLookAtTarget) return;
            
            if(player.SelectedInteractable is MonoBehaviour interactable) {
                if (interactable.transform == lookAtTarget) {
                    StopLookingAt();
                    OnLookAtTargetCompleted?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        private void Update() {
            
            
            if (HasReachedDestination()) {
                DestinationReached();
            }
            else if (IsMovingUsingNavigation && agent.pathStatus == NavMeshPathStatus.PathInvalid) {
                StopMoving();
            }

            Vector2 input = playerInput.GetMovementInput();
            IsMovingUsingInput = (input != Vector2.zero);

            if (IsMovingUsingInput && stopAllWhenUsingInput && (IsMovingUsingNavigation || HasLookAtTarget)) {
                StopAll();
            }
            

            var moveDirection = new Vector3(input.x, 0, input.y);
            HandleMovement(moveDirection);
            
            
            if (IsMovingUsingInput) {
                HandleRotation(moveDirection);
            }
            else if (!IsMovingUsingNavigation && HasLookAtTarget) {
                var dir =  lookAtTarget.position - transform.position;
                HandleRotation(dir);
            }
        }

        private bool HasReachedDestination() {
            float dist = agent.remainingDistance;
            return !agent.pathPending && 
                !float.IsPositiveInfinity(dist)
                && agent.remainingDistance <=  agent.stoppingDistance;
        }

        private void DestinationReached() {
            IsMovingUsingNavigation = false;
            agent.ResetPath();
            agent.velocity = Vector3.zero;
            OnDestinationReached?.Invoke(this, EventArgs.Empty);
        }
        

        private void HandleMovement(Vector3 direction) {
            if (IsMovingUsingNavigation && IsMovingUsingInput) {
                agent.velocity = direction * movementSpeed;
            }
            else {
                if(!rb.isKinematic)
                    rb.velocity = direction * movementSpeed;
                else {
                    transform.position += direction * (movementSpeed * Time.deltaTime);
                }
            }
        }

        private void HandleRotation(Vector3 direction) {
            if (IsMovingUsingNavigation || direction == Vector3.zero)
                return;
            
            direction.y = 0;
            var targetRotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
        
        public bool TryMoveTo(Vector3 position) {
            if (agent.SetDestination(position)) {
                IsMovingUsingNavigation = true;
                return true;
            }
            return false;
        }
        
        private void StopAll() {
            StopMoving();
            StopLookingAt();
        }
        
        public void StopMoving() {
            agent.ResetPath();
            IsMovingUsingNavigation = false;
        }
        
        public void LookAt(Transform target) {
            lookAtTarget = target;
            lookAtTargetUntilSelected = false;
        }
        
        
        public void LookAtUntilSelected(Transform target) {
            lookAtTarget = target;
            lookAtTargetUntilSelected = true;
        }
        
        public void StopLookingAt() {
            lookAtTarget = null;
            lookAtTargetUntilSelected = false;
        }
        

    }
}
