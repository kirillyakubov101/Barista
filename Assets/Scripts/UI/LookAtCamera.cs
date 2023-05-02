using UnityEngine;
using Barista.MyCamera;

namespace Barista.UI
{
    public class LookAtCamera : MonoBehaviour
    {
        private Camera m_camera;

        private void OnEnable()
        {
            CameraController.OnCameraSwitch += AssignCamera;
        }

        private void OnDestroy()
        {
            CameraController.OnCameraSwitch -= AssignCamera;
        }

        private void AssignCamera(Camera cam)
        {
            m_camera = cam;
        }

        private void LateUpdate()
        {
            transform.forward = m_camera.transform.forward;
        }
    }
}
