using System;
using UnityEngine;
using UnityEngine.AI;

namespace Players {
    public class PlayerMovement : MonoBehaviour {
        [SerializeField] private Player player;
        [SerializeField] private Rigidbody rb;
        [SerializeField] private NavMeshAgent agent;

        public float MovementSpeed => movementSpeed;
        [SerializeField] private float movementSpeed = 5f;

        public float RotationSpeed => rotationSpeed;
        [SerializeField] private float rotationSpeed = 540f;

        public bool IsMoving => IsMovingUsingInput || IsMovingUsingNavigation;
        public bool IsMovingUsingInput { get; private set; } = false;
        public bool IsMovingUsingNavigation { get; private set; } = false;

        public bool HasAgent => agent != null;
        
        public Vector2 MovementInput { get; set; } = Vector2.zero;

        private Transform lookAtTarget = null;
        private bool lookAtTargetUntilSelected = false;
        [SerializeField] private bool stopAllWhenUsingInput = true;
        private bool HasLookAtTarget => lookAtTarget != null;

        public event EventHandler OnMoveToStarted;
        public event EventHandler OnMoveToCompleted;
        public event EventHandler OnMoveToCanceled;
        
        public event EventHandler OnLookAtTargetCompleted;


        private void Start() {
            player.OnSelectedInteractableChanged += OnSelectedInteractableChanged;
        }

        private void OnDestroy() {
            player.OnSelectedInteractableChanged -= OnSelectedInteractableChanged;
        }

        private void OnSelectedInteractableChanged(object sender, ValueChangedEvent<IInteractable> e) {
            if (!lookAtTargetUntilSelected || !HasLookAtTarget) return;

            if (player.SelectedInteractable is MonoBehaviour interactable) {
                if (interactable.transform == lookAtTarget) {
                    StopLookingAt();
                    OnLookAtTargetCompleted?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        private void Update() {
            if (HasAgent) {
                if (HasReachedDestination()) {
                    DestinationReached();
                }
                else if (IsMovingUsingNavigation && agent.pathStatus == NavMeshPathStatus.PathInvalid) {
                    StopMoving();
                }
            }

            
            IsMovingUsingInput = (MovementInput != Vector2.zero);

            if (IsMovingUsingInput && stopAllWhenUsingInput && (IsMovingUsingNavigation || HasLookAtTarget)) {
                StopAll();
            }


            var moveDirection = new Vector3(MovementInput.x, 0, MovementInput.y);
            HandleMovement(moveDirection);


            if (IsMovingUsingInput) {
                HandleRotation(moveDirection);
            }
            else if (!IsMovingUsingNavigation && HasLookAtTarget) {
                var dir = lookAtTarget.position - transform.position;
                HandleRotation(dir);
            }
        }

        private bool HasReachedDestination() {
            float dist = agent.remainingDistance;
            return !agent.pathPending &&
                   !float.IsPositiveInfinity(dist)
                   && agent.remainingDistance <= agent.stoppingDistance;
        }

        private void DestinationReached() {
            IsMovingUsingNavigation = false;
            agent.ResetPath();
            agent.velocity = Vector3.zero;
            OnMoveToCompleted?.Invoke(this, EventArgs.Empty);
        }


        private void HandleMovement(Vector3 direction) {
            if (HasAgent && IsMovingUsingNavigation && IsMovingUsingInput) {
                agent.velocity = direction * movementSpeed;
            }
            else {
                if (!rb.isKinematic)
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
            transform.rotation =
                Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        public bool TryMoveToAndLookAt(Transform target) {
            if (TryMoveTo(target.position)) {
                LookAt(target);
                return true;
            }
            StopLookingAt();
            return false;
        }
            
            
            
        public bool TryMoveTo(Vector3 position) {
            StopMoving();
            
            if (HasAgent && agent.SetDestination(position)) {
                IsMovingUsingNavigation = true;
                OnMoveToStarted?.Invoke(this, EventArgs.Empty);
                return true;
            }
            return false;
        }

        private void StopAll() {
            StopMoving();
            StopLookingAt();
        }

        public void StopMoving() {
            if (HasAgent)
                agent.ResetPath();
            var hadDestination = IsMovingUsingNavigation;
            IsMovingUsingNavigation = false;
            
            if (hadDestination)
                OnMoveToCanceled?.Invoke(this, EventArgs.Empty);
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
