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


        //Let all the "Look At Camera" UIs know about camera switch
        //TODO: Not sure this is a MUST
        private void Start()
        {
            OnCameraSwitch?.Invoke(m_cameras[1]);
        }


        private void Update()
        {
            horizontalValue = (int)Input.GetAxisRaw(InputX);
            if (horizontalValue != 0 && time > pressCd)
            {
                //nextIndex = (currentIndex + 1) % collectionSize
                //nextIndex = (currentIndex - 1 + collectionSize) % collectionSize;
               
                if(horizontalValue > 0)
                    index = (index + 1) % 3;
                else
                    index = (index - 1 + 3) % 3;

                Rotate();

                time = 0f;
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
        }

    }
}

