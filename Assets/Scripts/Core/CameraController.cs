using System.Collections.Generic;
using UnityEngine;
using System;
using MyUtils;

namespace Barista.MyCamera
{

    public class CameraController : Singleton<CameraController>
    {
        [SerializeField] private Camera[] m_cameras;
        [SerializeField] private float pressCd = 1f;

        public static event Action<Camera> OnCameraSwitch;

        private const string InputX = "Horizontal";
        private int horizontalValue;
        private float time = 0;
        private int index = 1;
        private int prevIndex = 1;

        private enum CameraState
        {
            LEFT, CENTER, RIGHT
        }

        Dictionary<CameraState, Camera> m_positions = new Dictionary<CameraState, Camera>();

        private void Start()
        {
            m_positions.Add(CameraState.LEFT, m_cameras[0]);
            m_positions.Add(CameraState.CENTER, m_cameras[1]);
            m_positions.Add(CameraState.RIGHT, m_cameras[2]);

            OnCameraSwitch?.Invoke(m_cameras[1]);
        }

        private CameraState IntToEnum(int val)
        {
            switch(val)
            {
                case 0: return CameraState.LEFT;
                case 1: return CameraState.CENTER;
                case 2: return CameraState.RIGHT;
                default: return CameraState.CENTER;  
            }
        }


        private void Update()
        {
            horizontalValue = (int)Input.GetAxisRaw(InputX);
            if (horizontalValue != 0 && time > pressCd)
            {
                index += horizontalValue;
                index = Mathf.Clamp(index, 0, 2);
                if(prevIndex != index)
                {
                    Rotate();
                }
                
                time = 0;
            }
            if(time < pressCd + 2f)
            {
                time += Time.deltaTime;
            }
            
        }

        private void Rotate()
        {
            for (int i = 0; i < m_cameras.Length; i++)
            {
                if(index != i)
                {
                    m_cameras[i].gameObject.SetActive(false);
                }
                else
                {
                    m_cameras[i].gameObject.SetActive(true);

                    OnCameraSwitch.Invoke(m_cameras[i]);
                }
            }

            prevIndex = index;
        }

    }
}

