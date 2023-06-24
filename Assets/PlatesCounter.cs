using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatesCounter : BaseCounter {

    [SerializeField] private float timeToSpawnPlate = 0.5f;
    private float remainingTimeToSpawnPlate = 0f;

    [SerializeField] private int maxPlates = 3;

    private int platesCount = 0;

    [SerializeField] private KitchenObjectSO plateKitchenObjectSO;


    private void Update() {

        remainingTimeToSpawnPlate -= Time.deltaTime;

        if (remainingTimeToSpawnPlate <= 0f && platesCount < maxPlates) {
            remainingTimeToSpawnPlate = timeToSpawnPlate;
            var plate = KitchenObject.CreateInstance(plateKitchenObjectSO, null);
        }

    }

}
