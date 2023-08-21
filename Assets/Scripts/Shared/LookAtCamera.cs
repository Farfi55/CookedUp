using UnityEngine;

namespace Shared
{
    public class LookAtCamera : MonoBehaviour {
        private Camera mainCamera;

        [SerializeField] private LookAtMode lookAtMode = LookAtMode.CameraFormard;


        void Start() {
            mainCamera = Camera.main;
        }

        // Update is called once per frame
        private void LateUpdate() {
            switch (lookAtMode) {
                case LookAtMode.LookAt:
                    transform.LookAt(mainCamera.transform);
                    break;
                case LookAtMode.LookAtInverted:
                    transform.LookAt(mainCamera.transform);
                    transform.Rotate(0, 180, 0);
                    break;
                case LookAtMode.CameraFormard:
                    transform.rotation = Quaternion.LookRotation(mainCamera.transform.forward);
                    break;
                case LookAtMode.CameraBackward:
                    transform.rotation = Quaternion.LookRotation(-mainCamera.transform.forward);
                    break;
            }

        }

        public enum LookAtMode {
            LookAt,
            LookAtInverted,
            CameraFormard,
            CameraBackward
        }
    }
}
