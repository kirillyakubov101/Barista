using UnityEngine;
using MyUtils;
using System.Collections;
using Barista.Sounds;
using Barista.Food;

namespace Barista.Machines
{
    public class Stove : Singleton<Stove>
    {
        [SerializeField] private Animator m_animator;
        [SerializeField] private float m_prepareTime = 3f;
        [SerializeField] private PreparedFood m_preparedFood;
        [SerializeField] private GameObject m_dummyFood;
        [SerializeField] private ParticleSystem m_OilVfx;

        private bool m_canHeat = true;

        readonly int hashIndex = Animator.StringToHash("Cook");

        public bool CanHeat { get => m_canHeat;}

        public void AddFoodToStove()
        {
            m_canHeat = false;
            m_animator.Play(hashIndex);
            m_dummyFood.SetActive(true);

            SoundHandler.Instance.PlayStoveSound(true);
            m_OilVfx.Play();
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
            m_preparedFood.ShowPreparedFood();
            m_dummyFood.SetActive(false);
            m_OilVfx.Stop();

            SoundHandler.Instance.PlayStoveSound(false);
        }
    }
}
