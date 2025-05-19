using UnityEngine;

namespace CookieNoir.VDayXR
{
    public class MainCameraFacer : MonoBehaviour
    {
        private void Update()
        {
            var mainCamera = Camera.main;
            if (mainCamera == null)
            {
                return;
            }
            var cameraTransform = mainCamera.transform;
            transform.forward = cameraTransform.forward;// LookAt(transform.position + cameraTransform.rotation * Vector3.back, cameraTransform.rotation * Vector3.up);
        }
    }
}
