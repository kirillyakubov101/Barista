using UnityEngine;
using MyUtils;
using System.Collections;

namespace Barista.Machines
{
    public class Stove : Singleton<Stove>
    {
        [SerializeField] private Animator m_animator;
        [SerializeField] private float m_prepareTime = 3f;
        [SerializeField] private GameObject m_preparedFood;
        [SerializeField] private GameObject m_dummyFood;

        private bool m_canHeat = true;

        readonly int hashIndex = Animator.StringToHash("Cook");

        public bool CanHeat { get => m_canHeat;}

        public void AddFoodToStove()
        {
            m_canHeat = false;
            m_animator.Play(hashIndex);
            m_dummyFood.SetActive(true);

            StartCoroutine(ProcessCooking());
        }

        public void EnableStove()
        {
            m_canHeat = true;
        }

        private IEnumerator ProcessCooking()
        {
            yield return new WaitForSeconds(m_prepareTime);

            StoveDone();
        }

        private void StoveDone()
        {
            m_preparedFood.SetActive(true);
            m_dummyFood.SetActive(false);
        }
    }
}
