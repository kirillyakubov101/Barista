using UnityEngine;

namespace Barista.Machines
{
    public abstract class BeverageMachine : MonoBehaviour
    {
        [SerializeField] protected Animator m_animator;
        [SerializeField] protected GameObject m_beveragePrefab;
        [SerializeField] protected float m_beverageMakeTime = 3f;
       

        public virtual void MakeBeverage()
        {
            m_animator.enabled = true;
        }

        protected virtual bool IsStandEmpty()
        {
            return m_beveragePrefab.activeSelf;
        }

    }
}
