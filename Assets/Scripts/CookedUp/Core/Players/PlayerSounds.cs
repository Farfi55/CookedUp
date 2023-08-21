using CookedUp.Core.KitchenObjects;
using UnityEngine;

namespace CookedUp.Core.Players {
    [RequireComponent(typeof(Player))]
    public class PlayerSounds : MonoBehaviour {

        private Player player;
        private PlayerMovement playerMovement;
        
        [SerializeField] private float footstepDistance = 1f;
        private float remainingFootstepDistance;

        private Vector3 lastPlayerPosition;
        private SoundManager soundManager;


        private void Awake() {
            player = GetComponent<Player>();
            playerMovement = GetComponent<PlayerMovement>();
        }

        private void Start() {
            soundManager = SoundManager.Instance;
            
            if(soundManager == null) {
                Debug.LogWarning("SoundManager not found!, disabling PlayerSounds");
                enabled = false;
                return;
            }
            
            player.Container.OnKitchenObjectAdded += OnKitchenObjectAdded;
            player.Container.OnKitchenObjectRemoved += OnKitchenObjectRemoved;
        }

        private void OnKitchenObjectAdded(object sender, KitchenObject e) {
            soundManager.PlayPickupSound(player.transform.position);
        }

        private void OnKitchenObjectRemoved(object sender, KitchenObject e) {
            soundManager.PlayDropSound(player.transform.position);
        }
        

        private void Update() {
            Vector3 delta = player.transform.position - lastPlayerPosition;
            remainingFootstepDistance -= delta.magnitude;

            if (playerMovement.IsMoving && remainingFootstepDistance <= 0f) {
                
                remainingFootstepDistance = footstepDistance;
                soundManager.PlayFootstepSound(player.transform.position);
            }
            
            lastPlayerPosition = player.transform.position;
        }

    }
}
