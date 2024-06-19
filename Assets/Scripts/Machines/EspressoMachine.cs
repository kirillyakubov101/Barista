using Barista.Sounds;
using System.Collections;
using UnityEngine;

namespace Barista.Machines
{
    public class EspressoMachine : BeverageMachine
    {
        [SerializeField] private GameObject m_CoffeeProcessModels;
        [SerializeField] private GameObject m_CoffeeVisualAmount;

        private readonly Vector3 m_startVisualCoffeeAmount = new Vector3(0f, -0.0685f, 0f);
        private readonly Vector3 m_endVisualCoffeeAmount = Vector3.zero;

        //coffee machine
        readonly int animHashInt = Animator.StringToHash("MakeCoffee");

        public override void MakeBeverage()
        {
            if (IsStandEmpty()) { return; }
            SoundHandler.Instance.PlayCoffeeMachine(true);
            m_CoffeeProcessModels.SetActive(true);
            base.MakeBeverage();
            base.m_animator.Play(animHashInt);

            StartCoroutine(CoffeeMakingProcess());
        }

        protected override bool IsStandEmpty()
        {
            return m_CoffeeProcessModels.activeSelf || base.IsStandEmpty();
        }
      

        private IEnumerator CoffeeMakingProcess()
        {
            float timer = 0f;
            m_CoffeeVisualAmount.transform.localPosition = m_startVisualCoffeeAmount;

            while (timer <=  m_beverageMakeTime)
            {
                timer += Time.deltaTime;
                // Calculate the normalized time (0 to 1) based on the elapsed time and duration
                m_CoffeeVisualAmount.transform.localPosition = Vector3.Lerp(m_CoffeeVisualAmount.transform.localPosition, m_endVisualCoffeeAmount, (timer / m_beverageMakeTime) * Time.deltaTime);

                yield return null;
            }

            m_animator.enabled = false;
            m_CoffeeProcessModels.SetActive(false);
            m_beveragePrefab.SetActive(true);
            SoundHandler.Instance.PlayCoffeeMachine(false);
        }
    }
}
