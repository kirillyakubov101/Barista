using Barista.Core;
using System.Collections;
using UnityEngine;

namespace Barista.Food
{
    public class SimpleTake : Item, ISelectable
    {
        [SerializeField] protected GameObject m_graphics;
        [SerializeField] protected GameObject m_placeHolder;
        [SerializeField] protected float m_RefilTime = 3f;
        [SerializeField] protected ProgressMeter m_progressMeter;

        protected bool isRefilling = false;
        private float m_foodRefilSpeed = 2f;

        public void Select()
        {
            if (isRefilling) { return; }
            base.m_state = ItemState.SELECTED;
            m_outline.DisplayOutline(true);
        }

        public void Deselect()
        {
            base.m_state = ItemState.NORMAL;
            m_outline.DisplayOutline(false);
        }

        public virtual void DoAction()
        {
            base.m_state = ItemState.REFILL;  
            m_graphics.SetActive(false);
            if(m_placeHolder != null)
            {
                m_placeHolder.SetActive(true);
            }
            
            Cart.Instance.PopulateCart(GetFoodType());
        }

        public ItemState State()
        {
            return m_state;
        }

        public void Refil()
        {
            if (isRefilling) { return; }
            StartCoroutine(RefilProcess());
        }

        public IEnumerator RefilProcess()
        {
            isRefilling = true;

            Deselect();

            yield return m_progressMeter.ProgressFillProcess(m_RefilTime);
            yield return RefilAppearAnimation();
        }

        private IEnumerator RefilAppearAnimation()
        {
            //hide the placeholder
            if (m_placeHolder != null)
            {
                m_placeHolder.SetActive(false);
            }

            //show the graphic and make it 0 scale
            m_graphics.transform.localScale = Vector3.zero;
            m_graphics.SetActive(true);

            Vector3 addition = new Vector3(Time.deltaTime * m_foodRefilSpeed, Time.deltaTime * m_foodRefilSpeed, Time.deltaTime * m_foodRefilSpeed);

            while(m_graphics.transform.localScale.z < 1f)
            {
                m_graphics.transform.localScale += addition;
                yield return null;
            }

           m_graphics.transform.localScale = Vector3.one;
           isRefilling = false;
        }

       
    }
}
