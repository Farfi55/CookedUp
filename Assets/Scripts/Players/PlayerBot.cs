using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.LowLevel;

namespace Players {
    public class PlayerBot : MonoBehaviour {
        [SerializeField] private NavMeshAgent agent;
        [SerializeField] private PlayerMovement playerMovement;
        [SerializeField] private Player player;


        private void Start() {
            agent.speed = playerMovement.MovementSpeed;
            agent.angularSpeed = playerMovement.RotationSpeed;
        }


        void Update() {
            if (Input.GetMouseButtonDown(1)) {
                SetDestinationToMousePosition();
            }

            if (Input.GetMouseButtonDown(2)) {
                player.TryAlternateInteract();
                player.StartAlternateInteract();
            }
            else if (Input.GetMouseButtonUp(2)) {
                player.StopAlternateInteract();
            }
            
            if (Input.GetMouseButtonDown(0)) {
                player.TryInteract();
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
