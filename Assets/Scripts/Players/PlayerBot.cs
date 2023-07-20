using System;
using UnityEngine;
using UnityEngine.AI;

namespace Players
{
    public class PlayerBot : MonoBehaviour
    {
        [SerializeField] private NavMeshAgent agent;
        [SerializeField] private PlayerMovement playerMovement;
        

        private void Start() {
            agent.speed = playerMovement.MovementSpeed;
            agent.angularSpeed = playerMovement.RotationSpeed;
        }
        

        public void MoveTo(Vector3 position) {
            agent.SetDestination(position);
            
        }
        
        
        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                SetDestinationToMousePosition();
            }
            
            playerMovement.SetIsMoving(agent.velocity != Vector3.zero);
            
            // playerMovement.desiredMoveDirection = agent.desiredVelocity;

        }

        void SetDestinationToMousePosition()
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                agent.SetDestination(hit.point);
            }
        }
    }
}
