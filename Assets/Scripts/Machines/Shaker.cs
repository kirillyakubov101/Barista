using UnityEngine;

namespace Barista.Machines
{
    public class Shaker : BeverageMachine
    {
        [SerializeField] private Collider m_collder;
        [SerializeField] private GameObject m_glassVisual;

        readonly int animHashInt = Animator.StringToHash("Pour");
        

        public override void MakeBeverage()
        {
            if (IsStandEmpty()) { return; }
            base.MakeBeverage();
            m_glassVisual.SetActive(true);
            base.m_animator.Play(animHashInt);
            m_collder.enabled = false;
        }


        //Animation event
        private void EnableCollider()
        {
            m_collder.enabled = true;
            m_glassVisual.SetActive(false);
            m_beveragePrefab.SetActive(true);
        }
    }
}
