using Barista.MyCamera;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Barista
{
    public class Raycaster : MonoBehaviour
    {
        [SerializeField] private LayerMask m_Layer;
        [SerializeField] private float m_RayCastDistance = 100f;

        public event Action<bool> OnRaycastHitChange;

        private Camera m_Camera;
        private CameraController m_CamController;
        private RaycastHit m_HitInfo;
        private bool m_hasHit = false;
        private bool m_hasHitChanged = false;

        public RaycastHit Hitinfo { get => m_HitInfo; }
        public bool HasHit { get => m_hasHit; }

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

        private void FixedUpdate()
        {
            RayCastFromMouse();
        }

        private void AssignCamera(Camera cam)
        {
            m_Camera = cam;
        }

        private void RayCastFromMouse()
        {
            Ray mouseRay = m_Camera.ScreenPointToRay(Mouse.current.position.ReadValue());

            m_hasHit = Physics.Raycast(mouseRay.origin, mouseRay.direction, out m_HitInfo, m_RayCastDistance, m_Layer);

            UpdateHitChangeMode();
        }

        private void UpdateHitChangeMode()
        {
            if (m_hasHit && !m_hasHitChanged)
            {
                m_hasHitChanged = true;
                OnRaycastHitChange.Invoke(true);
            }
            else if (!m_hasHit && m_hasHitChanged)
            {
                m_hasHitChanged = false;
                OnRaycastHitChange.Invoke(false);
            }
        }
    }
}
