using UnityEngine;
using System;
using MyUtils;
using Barista.Input;

namespace Barista.MyCamera
{

    public class CameraController : Singleton<CameraController>
    {
        [SerializeField] private Camera[] m_cameras;

        public static event Action<Camera> OnCameraSwitch;

        private int m_activeCameraIndex = 1;

        private void Start()
        {
            OnCameraSwitch?.Invoke(m_cameras[1]);
        }

        private void OnEnable()
        {
            InputMasterControls.Instance.OnHorizontalMove += HandleHorizontalMoveAction;
        }

        private void OnDisable()
        {
            if(InputMasterControls.Instance)
            {
                InputMasterControls.Instance.OnHorizontalMove -= HandleHorizontalMoveAction;
            }
        }

        private void HandleHorizontalMoveAction(float value)
        {
            if (value > 0) { m_activeCameraIndex = (m_activeCameraIndex + 1) % 3; }
            else { m_activeCameraIndex = (m_activeCameraIndex - 1 + 3) % 3; }

            Rotate();
        }

        private void Rotate()
        {
            for (int i = 0; i < m_cameras.Length; i++)
            {
                if(m_activeCameraIndex != i)
                {
                    m_cameras[i].gameObject.SetActive(false);
                }
                else
                {
                    m_cameras[i].gameObject.SetActive(true);

                    OnCameraSwitch.Invoke(m_cameras[i]);
                }
            }
        }

    }
}

