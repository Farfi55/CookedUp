using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour {
    private Camera mainCamera;

    void Start() {
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    private void LateUpdate() {
        Vector3 targetPosition = transform.position + mainCamera.transform.rotation * Vector3.back;
        transform.LookAt(targetPosition);

    }
}
