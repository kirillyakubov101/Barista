using Barista.Core;
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

        public void Select()
        {
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

        public virtual async void Refil()
        {
            if (isRefilling) { return; }

            isRefilling = true;
            await m_progressMeter.ProgressFillProcess(m_RefilTime);
            isRefilling = false;

            base.m_state = ItemState.NORMAL;
            m_graphics.SetActive(true);

            if (m_placeHolder != null)
            {
                m_placeHolder.SetActive(false);
            }
           
            m_outline.DisplayOutline(false);
        }
    }
}
