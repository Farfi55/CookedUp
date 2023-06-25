using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatesCounter : BaseCounter {

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
        if (IsFull())
            return;
        progressTracker.AddWorkDone(Time.deltaTime);
    }


    public override void Interact(Player player) {

        if (player.HasKitchenObject()) {
            if (Container.HasSpace() && player.CurrentKitchenObject is PlateKitchenObject plate) {
                // if the plate has something on it already, don't put it on the stack
                if (plate.Container.HasAny())
                    return;

                // the player puts a plate on the stack
                player.CurrentKitchenObject.SetContainer(Container);
                if (IsFull())
                    progressTracker.SetProgress(0);
                InvokeOnInteract(new InteractableEvent(player));
            }
        }
        else {
            if (Container.HasAny()) {
                // the player takes a plate from the stack
                var wasFull = Container.IsFull();
                Container.KitchenObject.SetContainer(player.Container);

                if (wasFull)
                    progressTracker.SetProgress(0);

                InvokeOnInteract(new InteractableEvent(player));
            }
        }
    }

}
