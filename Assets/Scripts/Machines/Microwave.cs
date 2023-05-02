using UnityEngine;
using MyUtils;
using System.Collections;

namespace Barista.Machines
{
    public class Microwave : Singleton<Microwave>
    {
        [SerializeField] private GameObject m_preparedFood;
        [SerializeField] private GameObject m_dummyFood;
        [SerializeField] private float m_prepareTime = 3f;
        [SerializeField] private Animator m_animator;

        readonly int hashIndex1 = Animator.StringToHash("Micro");
        readonly int hashIndex2 = Animator.StringToHash("OpenMicro"); 
        readonly int hashIndex3 = Animator.StringToHash("CloseMicro");

        private bool m_canMicro = true;

        public void AddFoodToMicro()
        {
            if (!m_canMicro) { return; }
            m_canMicro = false;
            m_animator.Play(hashIndex1);
            m_dummyFood.SetActive(true);

            StartCoroutine(ProcessFoodPrep());
        }

        private IEnumerator ProcessFoodPrep()
        {
            yield return new WaitForSeconds(m_prepareTime);

            MicroDone();
        }

        private void MicroDone()
        {
            m_animator.Play(hashIndex2);
            m_dummyFood.SetActive(false);
            m_preparedFood.SetActive(true);
        }

        public void CloseMicro()
        {
            m_animator.Play(hashIndex3);
            m_canMicro = true;
        }


    }
}
