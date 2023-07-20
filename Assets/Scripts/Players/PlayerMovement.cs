using System;
using UnityEngine;

namespace Players {
    
    public class PlayerMovement : MonoBehaviour {
        
        [SerializeField] private Rigidbody rb;
        [SerializeField] private PlayerInput playerInput;

        public float MovementSpeed => movementSpeed;
        [SerializeField] private float movementSpeed = 5f;
        
        public float RotationSpeed => rotationSpeed;
        [SerializeField] private float rotationSpeed = 540f;
        
        public bool IsMoving => isMoving;
        private bool isMoving = false;
        
        public bool useInput = true;


        private void HandleRotation(Vector3 moveDirection) {
            if (isMoving && moveDirection != Vector3.zero) {
                var targetRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
                rb.rotation = Quaternion.RotateTowards(rb.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
                
                // rb.rotation = Quaternion.Slerp(
                //     rb.rotation,
                //     targetRotation,
                //     rotationSpeed * Time.fixedDeltaTime
                // );
            }
        }

        private void FixedUpdate() {
            var moveDirection = Vector3.zero;
            if (useInput) {
                Vector2 input = playerInput.GetMovementInput();
                SetIsMoving(input != Vector2.zero);
                
                moveDirection = new Vector3(input.x, 0, input.y);
            }

            HandleMovement(moveDirection);
        
            HandleRotation(moveDirection);
        }

        private void HandleMovement(Vector3 moveDirection) {
            rb.velocity = moveDirection * movementSpeed;
        }
        
        
        public void SetIsMoving(bool isMoving) {
            this.isMoving = isMoving;
        }

    }
}
