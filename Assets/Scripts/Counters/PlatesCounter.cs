using KitchenObjects;
using KitchenObjects.Container;
using KitchenObjects.ScriptableObjects;
using Players;
using UnityEngine;

namespace Counters
{
    public class PlatesCounter : BaseCounter {

        private GameManager gameManager;
        
        public KitchenObjectsContainer Container { get; private set; }

        private ProgressTracker progressTracker;

        [SerializeField] private float timeToSpawnPlate = 0.5f;

        public int PlatesLimit => Container.SizeLimit;

        public int PlatesCount => Container.Count;

        public bool IsFull() => Container.IsFull();

        [SerializeField] private KitchenObjectSO plateKitchenObjectSO;


        private void Awake() {
            Container = GetComponent<KitchenObjectsContainer>();
            progressTracker = GetComponent<ProgressTracker>();
        }

        private void Start() {
            gameManager = GameManager.Instance;
            
            progressTracker.SetTotalWork(timeToSpawnPlate);
            progressTracker.OnProgressComplete += (sender, e) => SpawnPlate();

        }

        private void SpawnPlate() {
            if (IsFull())
                return;
            KitchenObject.CreateInstance(plateKitchenObjectSO, Container);
            progressTracker.SetProgress(0);
        }

        private void Update() {
            if (IsFull() || !gameManager.IsGamePlaying)
                return;
            progressTracker.AddWorkDone(Time.deltaTime);
        }


        public override void Interact(Player player) {
            var wasFull = Container.IsFull();
        
            if (player.HasKitchenObject()) {
                if (Container.HasSpace() && player.CurrentKitchenObject is PlateKitchenObject plate) {
                    // if the plate has something on it already, don't put it on the stack
                    if (plate.IngredientsContainer.HasAny())
                        return;

                    // the player puts a plate on the stack
                    player.CurrentKitchenObject.SetContainer(Container);
                    if (IsFull())
                        progressTracker.SetProgress(0);
                    InvokeOnInteract(new InteractableEvent(player));
                }
                else if (Container.HasAny()) {
                    // if the plate on the Plates Counter use as ingredient the CurrentKitchenObject of the player
                    // we move the player's CurrentKitchenObject on top of the plate
                    // and move the plate as the player new CurrentKitchenObject
                    if (Container.KitchenObject.InteractWith(player.CurrentKitchenObject)) {
                        Container.KitchenObject.SetContainer(player.Container);
                    
                        if(wasFull)
                            progressTracker.SetProgress(0);
                    
                        InvokeOnInteract(new InteractableEvent(player));
                    }
                }
            }
            else if(player.Container.IsEmpty()) {
                if (Container.HasAny()) {
                    // the player takes a plate from the stack
                
                    Container.KitchenObject.SetContainer(player.Container);

                    if (wasFull)
                        progressTracker.SetProgress(0);

                    InvokeOnInteract(new InteractableEvent(player));
                }
            }
        }

    }
}
