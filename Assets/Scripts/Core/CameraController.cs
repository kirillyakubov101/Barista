using UnityEngine;
using System;
using MyUtils;
using Barista.Input;
using UnityEngine.Events;

namespace Barista.MyCamera
{

    public class CameraController : Singleton<CameraController>
    {
        [SerializeField] private Camera[] m_cameras;
        [SerializeField] private UnityEvent<bool> OnCenterCameraSwitchEvent;

        public static event Action<Camera> OnCameraSwitch;

        private int m_activeCameraIndex = 1;

        //return the main camera transform for the emotes
        public Transform GetCenterCameraTransform()
        {
            return m_cameras[1].transform;
        }
        

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


            //fixing the submit/remove panel being targeted by none center camera (even if the center camera is off)
            if(m_activeCameraIndex != 1)
            {
                OnCenterCameraSwitchEvent?.Invoke(false);
            }
            else
            {
                OnCenterCameraSwitchEvent?.Invoke(true);
            }

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

