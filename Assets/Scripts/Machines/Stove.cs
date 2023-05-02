using UnityEngine;
using MyUtils;
using System.Collections;
using System;

namespace Barista.Machines
{
    public class Stove : Singleton<Stove>
    {
        [SerializeField] private Animator m_animator;
        [SerializeField] private float m_prepareTime = 3f;
        [SerializeField] private GameObject m_preparedFood;
        [SerializeField] private GameObject m_dummyFood;

        private bool m_canHeat = true;

        readonly int hashIndex1 = Animator.StringToHash("Cook");

        public void AddFoodToStove()
        {
            if (!m_canHeat) { return; }
            m_canHeat = false;

            m_animator.Play(hashIndex1);
            m_dummyFood.SetActive(true);

            StartCoroutine(ProcessCooking());
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
            m_canHeat = true;
        }
    }
}
