using System.Collections;
using UnityEngine;

namespace Barista
{
    public class Vehicle : MonoBehaviour
    {
        [SerializeField] private float m_speed = 2f;
        [SerializeField] private Transform m_body;
       
        private GameObject m_currentCarModel = null;

        private void OnEnable()
        {
            if (CarFactory.Instance)
            {
                CarFactory.Instance.OnCarsLoaded += BeginPlay;
            }
                
        }

        private void OnDisable()
        {
            if(CarFactory.Instance)
            {
                CarFactory.Instance.OnCarsLoaded -= BeginPlay;
            }
        }

        private void BeginPlay()
        {
            LoadCarModel();

            // Start driving after the assets are loaded
            StartCoroutine(Drive());
        }


        private IEnumerator Drive()
        {
            while (true)
            {
                transform.Translate(Vector3.forward * m_speed * Time.deltaTime);
                yield return null;
            }
        }

        public void ChangeDirection()
        {
            // Change direction and speed when triggered
            transform.Rotate(0f, 180f, 0f);
            m_speed = Random.Range(5f, 9f);

            // Load a new car model to replace the current one
            LoadCarModel();
        }

       

        private void LoadCarModel()
        {
            // Release the previous instance (if any) before instantiating a new car
            if (m_currentCarModel != null)
            {
                CarFactory.Instance.CleanGameObject(m_currentCarModel);
            }

            CarFactory.Instance.SpawnNewCar(ReceiveNewCar, m_body);
        }

        private void ReceiveNewCar(GameObject newCar)
        {
            m_currentCarModel = newCar;
        }
    }
}
