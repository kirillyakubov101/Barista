using UnityEngine;
using MyUtils;
using System.Collections;
using Barista.Sounds;

namespace Barista.Machines
{
    public class Microwave : Singleton<Microwave>
    {
        [SerializeField] private GameObject m_preparedFood;
        [SerializeField] private GameObject m_dummyFood;
        [SerializeField] private float m_prepareTime = 3f;
        [SerializeField] private Animator m_animator;

        [SerializeField] Material m_preparationScreenMat;
        [SerializeField] Material m_ReadyScreenMat;
        [SerializeField] private Transform m_preparationScreen;

        readonly int hashIndex1 = Animator.StringToHash("Micro");
        readonly int hashIndex2 = Animator.StringToHash("OpenMicro"); 
        readonly int hashIndex3 = Animator.StringToHash("CloseMicro");

        private bool m_canMicro = true;

        public bool CanMicro { get => m_canMicro; }

        public void AddFoodToMicro()
        {
            if (!m_canMicro) { return; }
            SoundHandler.Instance.PlayMicrowaveDoorOpen(true);
            m_preparationScreen.GetComponent<MeshRenderer>().material = m_preparationScreenMat;
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
            m_preparationScreen.GetComponent<MeshRenderer>().material = m_ReadyScreenMat;
        }

        public void CloseMicro()
        {
            m_animator.Play(hashIndex3);
            m_canMicro = true;
            PlayCloseSound();
        }

        //Animation events
        private void PlayCloseSound()
        {
            SoundHandler.Instance.PlayMicrowaveDoorClose(true);
        }


    }
}
