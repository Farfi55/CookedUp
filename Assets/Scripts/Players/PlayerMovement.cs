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
        
        public event EventHandler OnDestinationReached;
        public event EventHandler OnLookAtTargetCompleted;


        private void Start() {
            player.OnSelectedInteractableChanged += OnSelectedInteractableChanged;
        }

        private void OnDestroy() {
            player.OnSelectedInteractableChanged -= OnSelectedInteractableChanged;
        }

        private void OnSelectedInteractableChanged(object sender, EventArgs e) {
            if (!lookAtTargetUntilSelected || lookAtTarget == null) return;
            
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

            Vector2 input = playerInput.GetMovementInput();
            IsMovingUsingInput = (input != Vector2.zero);

            var moveDirection = new Vector3(input.x, 0, input.y);
            HandleMovement(moveDirection);
            
            
            if (IsMovingUsingInput) {
                HandleRotation(moveDirection);
            }
            else if (!IsMovingUsingNavigation && lookAtTarget != null) {
                HandleLookAt();
            }
                

            
        }

        private bool HasReachedDestination() {
            float dist = agent.remainingDistance;
            return !agent.pathPending && 
                !float.IsPositiveInfinity(dist)
                   && agent.pathStatus == NavMeshPathStatus.PathComplete
                   && agent.remainingDistance <=  agent.stoppingDistance;
        }

        private void DestinationReached() {
            IsMovingUsingNavigation = false;
            agent.ResetPath();
            agent.velocity = Vector3.zero;
            OnDestinationReached?.Invoke(this, EventArgs.Empty);
        }
        

        private void HandleMovement(Vector3 moveDirection) {
            if (IsMovingUsingNavigation && IsMovingUsingInput) {
                agent.velocity = moveDirection * movementSpeed;
            }
            else {
                rb.velocity = moveDirection * movementSpeed;
            }
        }

        private void HandleRotation(Vector3 moveDirection) {
            if (IsMovingUsingNavigation || moveDirection == Vector3.zero)
                return;

            var targetRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
        
        private void HandleLookAt() {
            if (lookAtTarget == null)
                return;
            
            var direction = lookAtTarget.position - transform.position;
            direction.y = 0;
            var targetRotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        public bool TryMoveTo(Vector3 position) {
            if (agent.SetDestination(position)) {
                Debug.Log("Moving to " + position);
                IsMovingUsingNavigation = true;
                return true;
            }
            return false;
        }
        
        public void LookAt(Transform target) {
            lookAtTarget = target;
            lookAtTargetUntilSelected = false;
        }
        
        public void LookAtUntilSelected(Transform target) {
            lookAtTarget = target;
            lookAtTargetUntilSelected = true;
            
            Debug.Log("Looking at " + target.name);
        }
        
        public void StopLookingAt() {
            lookAtTarget = null;
            lookAtTargetUntilSelected = false;
        }
        

    }
}
