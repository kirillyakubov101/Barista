using Barista.MyCamera;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Barista
{
    public class Raycaster : MonoBehaviour
    {
        [SerializeField] private LayerMask m_Layer;
        [SerializeField] private float m_RayCastDistance = 100f;

        private Camera m_Camera;
        private CameraController m_CamController;
        private RaycastHit m_Hitinfo;

        private void Awake()
        {
            m_CamController = GetComponent<CameraController>();
        }

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
            m_Camera = cam;
        }

        public void RayCastFromMouse(out bool hit, out RaycastHit hitInfo)
        {
            Ray mouseRay = m_Camera.ScreenPointToRay(Mouse.current.position.ReadValue());

            hit = Physics.Raycast(mouseRay.origin, mouseRay.direction, out hitInfo, m_RayCastDistance, m_Layer);
        }
    }
}
