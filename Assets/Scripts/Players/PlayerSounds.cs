using System;
using KitchenObjects;
using UnityEngine;

namespace Players {
    [RequireComponent(typeof(Player))]
    public class PlayerSounds : MonoBehaviour {

        private Player player;
        private PlayerMovement playerMovement;
        
        [SerializeField] private float footstepDistance = 1f;
        private float remainingFootstepDistance;

        private Vector3 lastPlayerPosition;
        
        
        private void Awake() {
            player = GetComponent<Player>();
            playerMovement = GetComponent<PlayerMovement>();
        }

        private void Start() {
            player.Container.OnKitchenObjectAdded += OnKitchenObjectAdded;
            player.Container.OnKitchenObjectRemoved += OnKitchenObjectRemoved;
        }

        private void OnKitchenObjectAdded(object sender, KitchenObject e) {
            SoundManager.Instance.PlayPickupSound(player.transform.position);
        }

        private void OnKitchenObjectRemoved(object sender, KitchenObject e) {
            SoundManager.Instance.PlayDropSound(player.transform.position);
        }
        

        private void Update() {
            Vector3 delta = player.transform.position - lastPlayerPosition;
            remainingFootstepDistance -= delta.magnitude;

            if (playerMovement.IsMoving && remainingFootstepDistance <= 0f) {
                
                remainingFootstepDistance = footstepDistance;
                SoundManager.Instance.PlayFootstepSound(player.transform.position);
            }
            
            lastPlayerPosition = player.transform.position;
        }

    }
}
