using System;
using UnityEngine;
using UnityEngine.AI;

namespace Players {
    public class PlayerBot : MonoBehaviour {
        [SerializeField] private NavMeshAgent agent;
        [SerializeField] private PlayerMovement playerMovement;


        private void Start() {
            agent.speed = playerMovement.MovementSpeed;
            agent.angularSpeed = playerMovement.RotationSpeed;
        }


        void Update() {
            if (Input.GetMouseButtonDown(0)) {
                SetDestinationToMousePosition();
            }

            // playerMovement.desiredMoveDirection = agent.desiredVelocity;
        }


        void SetDestinationToMousePosition() {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit)) {
                playerMovement.TryMoveTo(hit.point);
                
                playerMovement.LookAt(hit.collider.transform);
            }
        }
    }
}
